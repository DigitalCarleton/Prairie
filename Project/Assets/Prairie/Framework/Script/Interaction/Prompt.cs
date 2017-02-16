using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Utility/Prompt")]
public class Prompt : MonoBehaviour 
{
    private static readonly int FIRST_PROMPT = 1;
    private static readonly int SECOND_PROMPT = 2;

    public bool isCyclic = false;

    public int curPrompt = FIRST_PROMPT;
    public string firstPrompt = "";
    public string secondPrompt = "";
	
    public string GetPrompt()
    {
        if (curPrompt == FIRST_PROMPT)
        {
            return this.firstPrompt;
        }
        else
        {
            return this.secondPrompt;
        }
    }

	public void DrawPrompt()
	{
		// Draw a GUI with the interaction 
        if (!string.IsNullOrEmpty(this.GetPrompt().Trim()))
        {
            Rect frame = new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 4, Screen.height / 4);
            GUI.BeginGroup(frame);
            GUILayout.Box(this.GetPrompt());
            GUI.EndGroup();
        }
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
		foreach (PromptInteraction i in source.GetComponents<PromptInteraction> ())
		{
			if (i.defaultPrompt != null)
			{
				if (prompt != "") {
					prompt += ", ";		// seperate multiple actions
				}
				prompt += i.defaultPrompt;
			}
		}

        this.firstPrompt = prompt;
	}

    public void CyclePrompt()
    {
        
        if (isCyclic && !string.IsNullOrEmpty(this.secondPrompt.Trim()) && !string.IsNullOrEmpty(this.firstPrompt.Trim()))
        {
            if (curPrompt == FIRST_PROMPT)
            {
                curPrompt = SECOND_PROMPT;
            }
            else
            {
                curPrompt = FIRST_PROMPT;
            }
        }
    }

}

