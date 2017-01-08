using UnityEngine;
using System.Collections;

public class NextTwineNodeInteraction : Interaction 
{
	public GameObject nextTwineNodeObject;

	void OnDrawGizmosSelected()
	{
		// Draw a green line between a Twine node and its subsequent Twine node
		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, nextTwineNodeObject.transform.position);
	}

	protected override void PerformAction () 
	{
		TwineNode twineNode = this.nextTwineNodeObject.GetComponent<TwineNode> ();

		if (twineNode != null) 
		{
			// Activate the node!
			twineNode.Activate (this.rootInteractor);
		}
	}
}

