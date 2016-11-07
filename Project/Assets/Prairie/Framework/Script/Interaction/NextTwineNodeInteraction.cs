using UnityEngine;
using System.Collections;

public class NextTwineNodeInteraction : Interaction 
{
	public GameObject nextTwineNodeObject;

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

