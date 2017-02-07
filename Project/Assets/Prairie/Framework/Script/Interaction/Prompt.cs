using UnityEngine;
using System.Collections;

public class Prompt : MonoBehaviour 
{
	public string promptText;
	
	public void DrawPrompt()
	{
		// Draw a GUI with the interaction 
		Rect frame = new Rect (Screen.width / 2, Screen.height / 2, Screen.width / 4, Screen.height / 4);
		GUI.BeginGroup(frame);
		GUILayout.Box (promptText); 
		GUI.EndGroup();
	}

	// `Reset()` is called when this component is being added to a
	// GameObject in the Inpsector for the first time, or if the user
	// explicitly clicks the "reset" button on the component.
	//
	// Here, we use it to set a sensible default for the component,
	// based on the presense of interaction components.
	public void Reset()
	{
		this.SetDefaultPrompt ();
	}

	public void SetDefaultPrompt()
	{
		string prompt = "";
		GameObject source = this.gameObject;
		foreach (PromptInteraction i in source.GetComponents<Interaction> ())
		{
			if (i.defaultPrompt != null)
			{
				if (prompt != "") {
					prompt += ", ";		// seperate multiple actions
				}
				prompt += i.defaultPrompt;
			}
		}

		this.promptText = prompt;
	}

}

