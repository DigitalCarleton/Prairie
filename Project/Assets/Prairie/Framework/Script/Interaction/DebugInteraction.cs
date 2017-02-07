using UnityEngine;
using System.Collections;

public class DebugInteraction : PromptInteraction 
{
	protected override void PerformAction () 
	{
		Debug.Log ("Interacted with " + this.gameObject.name);
	}

	override public string defaultPrompt {
		get {
			return "Debug to Console";
		}
	}

}
