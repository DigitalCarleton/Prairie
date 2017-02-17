using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Prompt))]
public class PromptEditor : Editor {

	Prompt prompt;

	public override void OnInspectorGUI ()
	{
		prompt = (Prompt)target;

		GUIContent label = new GUIContent ("Prompt Text", "Text displayed when a player can interact with this object.");
		prompt.firstPrompt = EditorGUILayout.TextField (label, prompt.firstPrompt);

		if (string.IsNullOrEmpty(prompt.firstPrompt.Trim()))
		{
            PrairieGUI.hintLabel("No prompt will be displayed in game.");
		} else
        {
            GUIContent cyclicLabel = new GUIContent("Cyclic Prompt", "Does this prompt have two cycling values? (i.e. open, close)");
            prompt.isCyclic = EditorGUILayout.Toggle(cyclicLabel, prompt.isCyclic);
            if (prompt.isCyclic)
            {

                GUIContent secondLabel = new GUIContent("Second Prompt", "Second prompt to display, will toggle between this and first prompt.");
                prompt.secondPrompt = EditorGUILayout.TextField(secondLabel, prompt.secondPrompt);
                if (string.IsNullOrEmpty(prompt.secondPrompt.Trim()))
                {
                    PrairieGUI.hintLabel("Second prompt will be ignored.");
                }
            }
        }

		if (GUI.changed) {
			EditorUtility.SetDirty(prompt);
		}
	}
}
