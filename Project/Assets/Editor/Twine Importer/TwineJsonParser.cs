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
			tempObj.AddComponent<NodeInfo> ();
			var data = tempObj.GetComponent<NodeInfo> ();
			data.pid = storyNode[NodeIdKey];
			data.name = storyNode[NodeNameKey];
			//data.tags = storyNode["tags"].AsArray.;
			data.content = storyNode["content"];
			//data.childrenNames = storyNode["childrenNames"];
			PrefabUtility.ReplacePrefab(tempObj, emptyObj, ReplacePrefabOptions.ConnectToPrefab);
			#endif
		}

	}


}
