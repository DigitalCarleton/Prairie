using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TwineJsonParser {

	public const string PRAIRIE_DECISION_TAG = "prairie_decision";

	/// <summary>
	/// Imports the provided file in full to the current project.
	/// </summary>
	/// <param name="file">The Twine JSON file to import.</param>
	public static void ImportFile (TextAsset file)
	{
		Debug.Log ("Importing "+file.name+"...");

		ReadJson (file.text);

		Debug.Log ("Done!");
	}

	public static void ImportFromString(string jsonString)
	{
		Debug.Log ("Importing JSON...");

		ReadJson (jsonString);

		Debug.Log ("Done!");
	}
		
	public static void ReadJson (string jsonString)
	{
		// parse using `SimpleJSON`
		JSONNode parsedJson = JSON.Parse(jsonString);
		JSONArray parsedArray = parsedJson["passages"].AsArray;

		// parent game object which will be the story prefab
		GameObject parent = new GameObject("Story");

		// Now, let's make GameObject nodes out of every twine/json node.
		//	Also, for easy access when setting up our parent-child relationships,
		//	we'll keep two dictionaries, linking names --> JSONNodes and names --> GameObjects
		Dictionary<string, JSONNode> twineNodesJsonByName = new Dictionary<string, JSONNode>();
		Dictionary<string, GameObject> twineGameObjectsByName = new Dictionary<string, GameObject>();

		for (int i = 0; i < parsedArray.Count; i++)
		{
			JSONNode storyNode = parsedArray [i];
			GameObject twineNodeObject = MakeGameObjectFromStoryNode (storyNode);

			// Bind this node to the parent "Story" object
			twineNodeObject.transform.SetParent (parent.transform);

			// Store this node and its game object in our dictionaries:
			twineNodesJsonByName [twineNodeObject.name] = storyNode;
			twineGameObjectsByName [twineNodeObject.name] = twineNodeObject;

			if (i == 0) 
			{
				// Enable/activate the first node in the story by default:
				twineNodeObject.GetComponent<TwineNode> ().enabled = true;
			}
		}

		// normalize the positions to one another
		NormalizePositioning (parent);

		// link nodes to their children
		MatchChildren (twineNodesJsonByName, twineGameObjectsByName);

		// "If the directory already exists, this method does not create a new directory..."
		// From the C# docs
		System.IO.Directory.CreateDirectory ("Assets/Ignored");
		// save a prefab to disk, and then remove the GameObject from the scene
		PrefabUtility.CreatePrefab ("Assets/Ignored/Story.prefab", parent);
		GameObject.DestroyImmediate (parent);
	}

	/// <summary>
	/// Turns a JSON-formatted Twine node into a GameObject with all the relevant data in a TwineNode component.
	/// </summary>
	/// <returns>GameObject of single node.</returns>
	/// <param name="storyNode">A Twine Node, in JSON format</param>
	public static GameObject MakeGameObjectFromStoryNode (JSONNode storyNode)
	{
		#if UNITY_EDITOR

		GameObject nodeGameObject = new GameObject(storyNode["name"]);
		nodeGameObject.AddComponent<TwineNode> ();

		// Save additional Twine data on a Twine component
		TwineNode twineNode = nodeGameObject.GetComponent<TwineNode> ();
		twineNode.pid = storyNode["pid"];
		twineNode.name = storyNode["name"];

		twineNode.tags = GetDequotedStringArrayFromJsonArray(storyNode["tags"]);

		twineNode.content = RemoveTwineLinks (storyNode["text"]);

		// Upon creation of this node, ensure that it is a decision node if it has
		//	the decision tag:
		twineNode.isDecisionNode = (twineNode.tags != null && twineNode.tags.Contains (PRAIRIE_DECISION_TAG));

		// Relative Twine location --> Unity coordinates
		JSONNode position = storyNode["position"];
		float twineX = position["x"].AsFloat;	// `AsFloat` returns 0.0f on failure, which is acceptable
		float twineY = position["y"].AsFloat;
		// map Twine arrangement x and y to Unity x and z
		nodeGameObject.transform.localPosition = new Vector3(twineX, 0, twineY);

		// Start all twine nodes as deactivated at first:
		twineNode.Deactivate();

		return nodeGameObject;

		#endif
	}

	public static void MatchChildren(Dictionary<string, JSONNode> twineNodesJsonByName, Dictionary<string, GameObject> gameObjectsByName)
	{
		foreach(KeyValuePair<string, GameObject> entry in gameObjectsByName)
		{
			string nodeName = entry.Key;
			GameObject nodeObject = entry.Value;

			TwineNode twineNode = nodeObject.GetComponent<TwineNode> ();
			JSONNode jsonNode = twineNodesJsonByName [nodeName];
		
			// Iterate through the links and establish object relationships:
			JSONNode nodeLinks = jsonNode ["links"];

			// TODO: This is only for displaying in the inspector! Remove this once you've changed the inspector to use the linkMap dictionary!
			twineNode.children = new GameObject[nodeLinks.Count];

			for (int i = 0; i < nodeLinks.Count; i++) {
				JSONNode link = nodeLinks [i];

				string linkName = link ["name"];
				GameObject linkDestination = gameObjectsByName [link ["link"]];
	
				if (twineNode.linkMap.ContainsKey (linkName)) {
					twineNode.linkMap.Remove (linkName);
				}
				twineNode.linkMap.Add (linkName, linkDestination);

				// Remember parent:
				linkDestination.GetComponent<TwineNode> ().parents.Add (nodeObject);

				// Add children to list of children.
				// TODO: This is only for displaying in the inspector! Remove this once you've changed the inspector to use the linkMap dictionary!
				twineNode.children[i] = linkDestination;
			}
		}
	}

	/// <summary>
	/// Twine's coordinate system is different in scale and local root than Unity's.
	/// Updates the Twine node's coordinates to sensible Unity values.
	/// </summary>
	/// <param name="parent">The top level story node.</param>
	private static void NormalizePositioning(GameObject parent)
	{
		// find ranges of `x` and `z` values
		float minX = float.MaxValue;
		float maxX = float.MinValue;

		float minZ = float.MaxValue;
		float maxZ = float.MinValue;

		foreach (Transform childTransform in parent.transform)
		{
			GameObject child = childTransform.gameObject;

			float x = child.transform.localPosition.x;
			float z = child.transform.localPosition.z;

			// update min if less than
			minX = x < minX ? x : minX;
			minZ = z < minZ ? z : minZ;

			// update max if greater than
			maxX = x > maxX ? x : maxX;
			maxZ = z > maxZ ? z : maxZ;
		}

		// anchor coordinate system to (0,0)
		//
		// such that a hypothetical object at (minX, minZ) will be at (0,0)
		// and other objects will maintain their original distances to that object
		foreach (Transform childTransform in parent.transform)
		{
			GameObject child = childTransform.gameObject;

			float newX = child.transform.localPosition.x - minX;
			float newZ = child.transform.localPosition.z - minZ;
			float originalY = child.transform.localPosition.y;

			child.transform.localPosition = new Vector3 (newX, originalY, newZ);
		}

		// adjust scale of parent (spacing between nodes) based on max size of Twine bounding box
		float TWINE_TO_UNITY_SCALE_RATIO = 20.0f;

		float maxTwineRange = Mathf.Max (maxX - minX, maxZ - minZ);
		float adjustedScale = TWINE_TO_UNITY_SCALE_RATIO / maxTwineRange;

		parent.transform.localScale = new Vector3 (adjustedScale, 1, adjustedScale);
	}
		
	/// <summary>
	/// Takes a string list of strings from the JSON, and makes it an array of
	/// strings, where each string is distinct from the other instead of one
	/// giant string.
	/// Boolean allows children to have more sliced off of each string than tags.
	/// The 4 within the if statement is to slice off the brackets around the children: [[children]].
	/// The 2 comes because we want to slice the brackets off of the tag: [tag].
	/// </summary>
	/// <param name="node">the JSONNode, which just holds the tag or childrenNames content.</param>
	/// <param name="parseChildren">If set to <c>true</c>, slice more off the ends of child names.</param>
	public static string[] Serialize (JSONNode node, bool parseChildren)
	{
		string[] nodeList = new string[node.Count];
		for (int i = 0; i < node.Count; i++)
		{
			string nodeString = node [i].ToString();
			if (parseChildren)
			{
				int stringLength = nodeString.Length - 4;
				nodeString = nodeString.Substring (2, stringLength);
			}
			nodeString = nodeString.Substring (1, nodeString.Length - 2);
			nodeList[i] = (nodeString);
		}
		return nodeList;
	}
		
	/// <summary>
	/// Strips the list of children off the content,
	/// because we really only want the content.
	/// </summary>
	/// <returns>The content without children atached.</returns>
	/// <param name="content">Content with children attached.</param>
	public static string RemoveTwineLinks (string content)
	{
		string[] substrings = content.Split ('[');
		return substrings[0];
	}

	static string[] GetDequotedStringArrayFromJsonArray (JSONNode jsonNode)
	{
		if (jsonNode == null) {
			return null;
		}

		string[] stringArray = new string[jsonNode.Count];
		for (int i = 0; i < jsonNode.Count; i++)
		{
			string quotedString = jsonNode [i].ToString();
			string dequotedString = quotedString.Replace ('"', ' ').Trim ();
			stringArray [i] = dequotedString;
		}

		return stringArray;
	}
}
