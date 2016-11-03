using UnityEngine;
using System.Collections;

public class DebugInteraction : Interaction 
{
	void OnDrawGizmos() 
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (new Vector3(transform.position.x, transform.position.y * 2, transform.position.z), transform.localScale.x / 2);
	}

	protected override void PerformAction () 
	{
		Debug.Log ("Interacted with " + this.gameObject.name);
	}

}
