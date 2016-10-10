using UnityEngine;
using System.Collections;

public class DebugInteraction : Interaction 
{
	protected override void PerformAction () 
	{
		Debug.Log ("Interacted with " + this.gameObject.name);
	}

}
