using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TwineJsonParser {

	// Keys for JSON:
	//private readonly static string NodeIdKey = "pid";
	//private readonly static string NodeNameKey = "name";

	/// <summary>
	/// Imports the provided file in full to the current project
	/// </summary>
	/// <param name="file">The Twine JSON file to import</param>
	public static void ImportFile (TextAsset file) {
		Debug.Log ("Importing "+file.name+"...");

		ReadJson (file.text);

		Debug.Log ("Done!");
	}
		
	public static void ReadJson (string jsonString) {
		JSONNode parsedJson = JSON.Parse(jsonString);
		JSONArray parsedArray = parsedJson.AsArray;
		GameObject[] prefabNodes = new GameObject[parsedArray.Count];

		// Demo code, printing the ID and name of each node:
		int count = 0;
		// create empty game object
		GameObject parent = new GameObject("Story");
		foreach (JSONNode storyNode in parsedArray) {
			//Debug.Log ("Node ID: " + storyNode[NodeIdKey]);
			//Debug.Log ("Node name: " + storyNode[NodeNameKey]);
			GameObject prefabNode = MakeGameObjectFromStory(parent, storyNode);
			//Debug.Log (prefabNode);
			prefabNodes [count] = prefabNode;
			++count;
		}
		// make dictionary
		Dictionary<string, GameObject> objDict = new Dictionary<string, GameObject>();
		foreach (GameObject node in prefabNodes) {
			//Debug.Log (node.name);
			//Debug.Log (node);
			objDict.Add (node.name, node);
		}
		FindChildren (prefabNodes, objDict);

		PrefabUtility.CreatePrefab ("Assets/Ignored/Story.prefab", parent);
		GameObject.DestroyImmediate (parent);

		// findChildren(array, dictionary)
	}

	public static void FindChildren (GameObject[] nodes, Dictionary<string, GameObject> objDict) {
		foreach (GameObject node in nodes) {
			string[] children = node.GetComponent<NodeInfo> ().childrenNames;
			node.GetComponent <NodeInfo> ().children = new GameObject[children.Length];
			int count = 0;
			foreach (string child in children) {
				node.GetComponent <NodeInfo> ().children[count] = objDict[child];
				++count;
			}
		}
	}

	public static GameObject MakeGameObjectFromStory (GameObject parent, JSONNode storyNode) {
		#if UNITY_EDITOR

		GameObject tempObj = new GameObject(storyNode["name"]);
		tempObj.AddComponent<NodeInfo> ();
		var data = tempObj.GetComponent<NodeInfo> ();
		data.pid = storyNode["pid"];
		data.name = storyNode["name"];
		data.tags = Serialize (storyNode["tags"], false);
		data.content = StripChildren (storyNode["content"]);
		data.childrenNames = Serialize (storyNode["childrenNames"], true);
		//Object.Destroy (tempObj.GetComponent <BoxCollider> ());
		//Object.Destroy (tempObj.GetComponent <MeshRenderer> ());
		tempObj.transform.SetParent (parent.transform);
		return tempObj;

		#endif
	}

	public static string[] Serialize (JSONNode node, bool parseChildren) {
		//Debug.Log (node.Count);
		string[] nodeList = new string[node.Count];
		for (int i = 0; i < node.Count; i++) {
			//Debug.Log (node [i].ToString());
			string nodeString = node [i].ToString();
			if (parseChildren) {
				//Debug.Log (nodeString);
				int stringLength = nodeString.Length - 4;
				nodeString = nodeString.Substring (2, stringLength);
				//Debug.Log (nodeString);
			}
			nodeString = nodeString.Substring (1, nodeString.Length - 2);
			nodeList[i] = (nodeString);
		}
		return nodeList;
	}

	public static string StripChildren (string content) {
		string[] substrings = content.Split ('[');
		return substrings[0];
	}
}
