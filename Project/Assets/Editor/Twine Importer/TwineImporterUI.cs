using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Defines the Import Twine window and a few contextual menu actions to trigger import.
/// </summary>
public class TwineImporterUI : EditorWindow {

	public TextAsset targetFile;

	[MenuItem("Assets/Import Twine Data...")]
	static void ShowTwineImportWindow () {

		// if triggered while a text asset is selected, populate it as the target file
		TextAsset selectedFile = null;
		if (Selection.activeObject != null && Selection.activeObject.GetType () == typeof(TextAsset)) {
			selectedFile = (TextAsset) Selection.activeObject;
		}

		// create and show window
		var window = EditorWindow.GetWindow<TwineImporterUI> ();
		window.targetFile = selectedFile;
	}

	void OnGUI () {
		GUILayout.Label ("Import Twine Data", EditorStyles.boldLabel);

		// display info about the selected file
		targetFile = (TextAsset) EditorGUILayout.ObjectField ("Selcted File:", targetFile, typeof(TextAsset), false);

		// send it to importer
		if (GUILayout.Button ("Import") && this.targetFile != null) {
			SendToImporter (this.targetFile);
		}
	}

	/// <summary>
	/// Sends an html file to the Twine importer for input
	/// </summary>
	/// <param name="filePath">The file path of the Twine html to be imported</param>
	void SendToImporter (TextAsset file) {
		// TwineImporter.Import (file);
		Debug.Log ("Importing "+file.name+"...");
	}

}
