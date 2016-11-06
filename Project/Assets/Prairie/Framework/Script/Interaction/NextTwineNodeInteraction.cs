using UnityEngine;
using System.Collections;

public class NextTwineNodeInteraction : Interaction 
{
	public GameObject nextTwineNode;

	protected override void PerformAction () 
	{
		TwineNode twineNodeComponent = this.nextTwineNode.GetComponent<TwineNode> ();

		if (twineNodeComponent != null && !twineNodeComponent.enabled && twineNodeComponent.HasActiveParentNode()) 
		{
			twineNodeComponent.Activate (this.rootInteractor);
			twineNodeComponent.DeactivateAllParents ();
		}
	}
}

