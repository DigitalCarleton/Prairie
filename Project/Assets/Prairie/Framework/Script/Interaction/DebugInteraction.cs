using UnityEngine;
using System.Collections;

public class DebugInteraction : Interaction 
{
	public override void Interact () 
	{
		Debug.Log ("Interacted with " + this.gameObject.name);
	}

}
