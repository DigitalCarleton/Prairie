using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Prompt))]
public class PromptEditor : Editor {

	Prompt prompt;

	public override void OnInspectorGUI ()
	{
		prompt = (Prompt)target;

		if (prompt.isTwinePrompt)
        {
            TwinePromptGUI();
        }
        else
        {
            SimplePromptGUI();
        }

		if (GUI.changed) {
			EditorUtility.SetDirty(prompt);
		}
	}

    public void SimplePromptGUI()
    {
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
    }

    public void TwinePromptGUI()
    {
        EditorGUILayout.Label("Twine Prompts");

        // get list of twine nodes we need prompts for, the keys of our dictionary
        AssociatedTwineNodes associatedNodes = this.prompt.gameObject.GetComponent<AssociatedTwineNodes> ();
        List<string> twineNodeNames = new List<string> ();
        foreach (GameObject twineNodeObject in associatedNodes.associatedTwineNodeObjects)
        {
            TwineNode twineNode = twineNodeObject.GetComponent<TwineNode> ();
            twineNodeNames.Add(twineNode.name);
        }

        // build dictionary and get a value for each key
        Dictionary<string, string> twinePrompts = new Dictionary<string, string> ();
        foreach (string nodeName in twineNodeNames)
        {
            string previousValue = this.prompt.twinePrompts[nodeName];
            if (previousValue == null)
            {
                previousValue = ""  // initialize a missing value
            }

            GUIContent label = new GUIContent (nodeName, "Text displayed when a player can progress the story to this twine node.");
            twinePrompts[nodeName] = EditorGUILayout.TextField(label, previousValue);
        }

        // save dictionary back to the prompt
        this.prompt.twinePrompts = twinePrompts;
    }

}
