using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TwineJsonParser {

	// Keys for JSON:
	private readonly static string NodeIdKey = "pid";
	private readonly static string NodeNameKey = "name";

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
		Object[] prefabNodes = new Object[parsedArray.Count];

		// Demo code, printing the ID and name of each node:
		int count = 0;
		foreach (JSONNode storyNode in parsedArray) {
			//Debug.Log ("Node ID: " + storyNode[NodeIdKey]);
			//Debug.Log ("Node name: " + storyNode[NodeNameKey]);
			Object prefabNode = MakePrefab(storyNode);
			prefabNodes [0] = prefabNode;
		}
		// make dictionary
		Dictionary<string, Object> objDict = new Dictionary<string, Object>();
		foreach (Object node in prefabNodes) {
			objDict.Add (node.name, node);
		}

		// findChildren(array, dictionary)
	}

	public static void FindChildren (Object[] nodes, Dictionary<string, Object> objDict) {
		foreach (Object node in nodes) {
			string[] children = node.getComponent<NodeInfo> ();
			//foreach (string child in )
		}
	}

	public static Object MakePrefab (JSONNode storyNode) {
		#if UNITY_EDITOR
		Object emptyObj;
		string obj_name = storyNode[NodeNameKey];
		string fileLocation = "Assets/Ignored/" + obj_name + ".prefab";
		emptyObj = PrefabUtility.CreateEmptyPrefab(fileLocation);

		GameObject tempObj = GameObject.CreatePrimitive (PrimitiveType.Cube);
		tempObj.name = obj_name;
		tempObj.AddComponent<NodeInfo> ();
		var data = tempObj.GetComponent<NodeInfo> ();
		data.pid = storyNode["pid"];
		data.name = storyNode["name"];
		data.tags = Serialize (storyNode["tags"], false);
		data.content = storyNode["content"];
		data.childrenNames = Serialize (storyNode["childrenNames"], true);
		PrefabUtility.ReplacePrefab(tempObj, emptyObj, ReplacePrefabOptions.ConnectToPrefab);
		return emptyObj;
		#endif
	}

	public static string[] Serialize (JSONNode node, bool parseChildren) {
		//Debug.Log (node.Count);
		string[] nodeList = new string[node.Count];
		for (int i = 0; i < node.Count; i++) {
			//Debug.Log (node [i].ToString());
			string nodeString = node [i].ToString();
			if (parseChildren) {
				Debug.Log (nodeString);
				int stringLength = nodeString.Length - 4;
				nodeString = nodeString.Substring (2, stringLength);
				Debug.Log (nodeString);
			}
			nodeString = nodeString.Substring (1, nodeString.Length - 2);
			nodeList[i] = (nodeString);
		}
		return nodeList;
	}
}
