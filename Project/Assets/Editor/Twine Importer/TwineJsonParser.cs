using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TwineJsonParser {

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
		
	public static void ReadJson (string jsonString)
	{
		// parse using `SimpleJSON`
		JSONNode parsedJson = JSON.Parse(jsonString);
		JSONArray parsedArray = parsedJson.AsArray;
		GameObject[] twineNodes = new GameObject[parsedArray.Count];
		
		// parent game object which will be the story prefab
		GameObject parent = new GameObject("Story");

		// make GameObject nodes out of every twine/json node
		int count = 0;
		foreach (JSONNode storyNode in parsedArray)
		{
			GameObject twineNode = MakeGameObjectFromStoryNode (parent, storyNode);
			twineNodes [count] = twineNode;

			if (count == 0) 
			{
				// Enable/activate the first node in the story by default:
				twineNode.GetComponent<TwineNode> ().enabled = true;
			}

			++count;
		}

		// normalize the positions to one another
		NormalizePositioning (parent);

		// create dictionary for fast lookup when matching nodes to their children
		Dictionary<string, GameObject> objDict = new Dictionary<string, GameObject>();
		foreach (GameObject node in twineNodes)
		{
			objDict.Add (node.name, node);
		}
		// link nodes to their children
		MatchChildren (twineNodes, objDict);

		// "If the directory already exists, this method does not create a new directory..."
		// From the C# docs
		System.IO.Directory.CreateDirectory ("Assets/Ignored");
		// save a prefab to disk, and then remove the GameObject from the scene
		PrefabUtility.CreatePrefab ("Assets/Ignored/Story.prefab", parent);
		GameObject.DestroyImmediate (parent);
	}
		
	/// <summary>
	/// Finds the children of each of the gameObjects from the other gameObjects.
	/// Iterates through the list of nodes, gets an array of children, and assigns
	/// gameObject children to the node that match the names ofthe array of children.
	/// </summary>
	/// <param name="nodes">Array of nodes.</param>
	/// <param name="objDict">Dictionary of nodes, with key of node name.</param>
	public static void MatchChildren (GameObject[] nodes, Dictionary<string, GameObject> objDict)
	{
		foreach (GameObject node in nodes)
		{
			string[] children = node.GetComponent<TwineNode> ().childrenNames;
			node.GetComponent <TwineNode> ().children = new GameObject[children.Length];
			//node.GetComponent <TwineNode> ().parentList = new List<GameObject> ();
			int childCount = 0;
			foreach (string childName in children)
			{
				// add children
				node.GetComponent <TwineNode> ().children[childCount] = objDict[childName];
				++childCount;
				// add parent
				GameObject childNode = GameObject.Find (childName);
				Debug.Log (childNode);
				Debug.Log (childNode.GetComponent <TwineNode> ().parents); 
				childNode.GetComponent <TwineNode> ().parents.Add (node);
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
	/// Takes the twine/JSON node, and turns it into a game object with all the relevant data.
	/// </summary>
	/// <returns>GameObject of single node.</returns>
	/// <param name="parent">Parent.</param>
	/// <param name="storyNode">JSON story node.</param>
	public static GameObject MakeGameObjectFromStoryNode (GameObject parent, JSONNode storyNode)
	{
		#if UNITY_EDITOR

		GameObject tempObj = new GameObject(storyNode["name"]);
		tempObj.AddComponent<TwineNode> ();

		// Save additional Twine data on a Twine component
		TwineNode data = tempObj.GetComponent<TwineNode> ();
		data.pid = storyNode["pid"];
		data.name = storyNode["name"];
		data.tags = Serialize (storyNode["tags"], false);
		data.content = StripChildren (storyNode["content"]);
		data.childrenNames = Serialize (storyNode["childrenNames"], true);

		// Relative Twine location --> Unity coordinates
		JSONNode position = storyNode["position"];
		float twineX = position["x"].AsFloat;	// `AsFloat` returns 0.0f on failure, which is acceptable
		float twineY = position["y"].AsFloat;
		// map Twine arrangement x and y to Unity x and z
		tempObj.transform.localPosition = new Vector3(twineX, 0, twineY);

		// Start all twine nodes as deactivated at first:
		data.Deactivate();

		// Bind to parent "Story" object
		tempObj.transform.SetParent (parent.transform);
		return tempObj;

		#endif
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
	public static string StripChildren (string content)
	{
		string[] substrings = content.Split ('[');
		return substrings[0];
	}
}
