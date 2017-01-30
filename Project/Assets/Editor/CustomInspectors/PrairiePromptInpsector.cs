using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Prompt))]
public class PrairiePromptInspector : Editor {

	// The reason this is called 'PrairiePromptInspector' is because Unity has
	// a tool internally called 'PromptInspector'.

	Prompt prompt;

	public override void OnInspectorGUI ()
	{
		prompt = (Prompt)target;

		GUIContent label = new GUIContent ("Prompt Text", "Text displayed when a player can interact with this object.");
		prompt.promptText = EditorGUILayout.TextField (label, prompt.promptText);

		if (prompt.promptText == null || prompt.promptText == "")
		{
			// I feel like there should be a warning here, but I don't know what to write
			// PrairieGUI.warningLabel("?")
		}
	}
}
