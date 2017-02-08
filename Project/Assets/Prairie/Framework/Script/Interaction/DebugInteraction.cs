using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Interactions/Debug")]
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
