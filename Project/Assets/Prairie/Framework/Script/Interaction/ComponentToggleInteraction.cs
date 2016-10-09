using UnityEngine;
using System.Collections;

public class ComponentToggleInteraction : Interaction 
{
	public Behaviour target;

	public override void Interact ()
	{
		if (target != null)
		{
			target.enabled = !target.enabled;
		}
	}
}
