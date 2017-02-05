using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

/// <summary>
/// Defines the Import Twine window and a few contextual menu actions to trigger import.
/// </summary>
public class TwineImporterUI : EditorWindow
{
	private string jsonString = "";

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

		GUILayout.Label ("Twine JSON text:");
	
		this.jsonString = GUILayout.TextArea (this.jsonString, GUILayout.MinHeight(20), GUILayout.MaxHeight(50));

		// button to send to importer
		GUI.enabled = (this.jsonString != "");
		if (GUILayout.Button ("Import"))
		{
			SendToImporter(jsonString);
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

	void SendToImporter (string jsonString) {
		// TODO: handle errors -- malformed json, etc.
		TwineJsonParser.ImportFromString (jsonString);
	}
}
