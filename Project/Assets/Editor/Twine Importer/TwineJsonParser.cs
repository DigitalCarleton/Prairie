using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TwineJsonParser {

	// Keys for JSON:
	private readonly static string NodeIdKey = "pid";
	private readonly static string NodeNameKey = "name";


	public static void ReadJson (string jsonString) {
		JSONNode parsedJson = JSON.Parse(jsonString);
		JSONArray parsedArray = parsedJson.AsArray;

		// Demo code, printing the ID and name of each node:
		foreach (JSONNode storyNode in parsedArray) {
			Debug.Log ("Node ID: " + storyNode[NodeIdKey]);
			Debug.Log ("Node name: " + storyNode[NodeNameKey]);
		}

	}


}
