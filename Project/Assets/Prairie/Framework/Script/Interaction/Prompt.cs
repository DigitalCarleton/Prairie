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
			GUILayout.Box ("Click to " + promptText); 
		GUI.EndGroup(); 
	}

}

