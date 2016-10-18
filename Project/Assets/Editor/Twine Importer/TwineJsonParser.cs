using UnityEngine;
using System.Collections;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TwineJsonParser {

	// Keys for JSON:
	private readonly static string NodeIdKey = "pid";
	private readonly static string NodeNameKey = "name";


	public static void ReadJson (string jsonString) {
		JSONNode parsedJson = JSON.Parse(jsonString);
		JSONArray parsedArray = parsedJson.AsArray;
		Debug.Log (parsedJson);

		// Demo code, printing the ID and name of each node:
		foreach (JSONNode storyNode in parsedArray) {
			//Debug.Log ("Node ID: " + storyNode[NodeIdKey]);
			//Debug.Log ("Node name: " + storyNode[NodeNameKey]);
			#if UNITY_EDITOR
			Object emptyObj;
			string obj_name = storyNode[NodeNameKey];
			string fileLocation = "Assets/Ignored/" + obj_name + ".prefab";
			emptyObj = PrefabUtility.CreateEmptyPrefab(fileLocation);

			GameObject tempObj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			tempObj.name = obj_name;
			PrefabUtility.ReplacePrefab(tempObj, emptyObj, ReplacePrefabOptions.ConnectToPrefab);
			emptyObj = GameObject.AddComponent(NodeInfo);
			#endif
		}

	}


}
