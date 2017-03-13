﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;
using SimpleJSON;

/// <summary>
/// Defines the Import Twine window and a few contextual menu actions to trigger import.
/// </summary>
public class TwineImporterUI : EditorWindow
{
	private string jsonString = "Copy and paste your Twine JSON here...";
	private string prefabDestinationDirectory = "Assets";

	/// <summary>
	/// Defines the "Import Twine Data" menu item and its action.
	/// If triggered from a context menu on an HTML asset, the asset is automatically selected for import.
	/// </summary>
	[MenuItem("Assets/Import Twine Data...")]
	static void ShowTwineImportWindow ()
	{
		// create and show window
		var window = EditorWindow.GetWindow<TwineImporterUI> ();
		window.Show ();
	}

	/// <summary>
	/// Draws the GUI of the import window
	/// </summary>
	void OnGUI ()
	{
		GUILayout.Label ("Import Twine Data", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Twine JSON text:");

		if (GUILayout.Button ("Paste from clipboard")) 
		{
			this.jsonString = GUIUtility.systemCopyBuffer;
		}

		GUILayout.EndHorizontal ();
	
		this.jsonString = GUILayout.TextArea (this.jsonString, GUILayout.MinHeight(20), GUILayout.MaxHeight(200));

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Twine story prefab destination:");

		// button which selects a prefab destination
		if (GUILayout.Button (prefabDestinationDirectory))
		{
			string fullPath = EditorUtility.OpenFolderPanel ("Select destination directory...", prefabDestinationDirectory, "");

			// obnoxiously, the OpenFilePanel returns a full file path,
			// and Unity will only play nice with a relative one so we must convert
			string projectDirectory = Directory.GetParent (Application.dataPath).ToString ();

			prefabDestinationDirectory = GetRelativePath (fullPath, projectDirectory);

			// double check we'll have access to this file
			if (!(prefabDestinationDirectory.StartsWith ("Assets/") || prefabDestinationDirectory.StartsWith ("Assets\\")))
			{
				EditorUtility.DisplayDialog ("Can't Load Asset", "The folder must be part of your Unity project's assets.", "OK");
			}

		}

		GUILayout.EndHorizontal ();

		bool isValid = isValidJson ();

		if (!isValid) 
		{
			PrairieGUI.warningLabel ("The JSON entered is invalid. Copy and paste JSON from Twison to import.");
		}


		// button to send to importer
		GUI.enabled = (this.jsonString != "" && isValid);
		if (GUILayout.Button ("Import"))
		{
			SendToImporter(jsonString, prefabDestinationDirectory);
			this.Close ();
		}
	}

	/// <summary>
	/// Converts a absolute path to a relative path from some other folder
	/// </summary>
	/// <returns>A relative path with the `folder` as the base</returns>
	/// <param name="filespec">The full path to a file inside of `folder`</param>
	/// <param name="folder">The folder to act as the root for the new path</param>
	private string GetRelativePath(string filespec, string folder)
	{
		Uri pathUri = new Uri(filespec);
		// Folders must end in a slash
		if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
		{
			folder += Path.DirectorySeparatorChar;
		}
		Uri folderUri = new Uri(folder);
		return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
	}

	void SendToImporter (string jsonString, string prefabDestinationDirectory) {
		TwineJsonParser.ImportFromString (jsonString, prefabDestinationDirectory);
	}

	private bool isValidJson()
	{
		try 
		{
			JSONNode parsedJson = JSON.Parse(this.jsonString);

			// Even if the JSON parses and gets to this point, ensure 
			//  that it has a "startnode" attribute. Valid Twison must 
			//  have a startnode indicator!
			return parsedJson["startnode"] != null;
		} catch 
		{
			return false;
		}

	}
}
