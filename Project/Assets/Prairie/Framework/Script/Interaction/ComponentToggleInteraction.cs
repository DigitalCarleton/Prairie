using UnityEngine;
using System.Collections;

public class ComponentToggleInteraction : Interaction 
{
	public Behaviour target;

	protected override void PerformAction ()
	{
		if (target != null)
		{
			target.enabled = !target.enabled;
		}
	}
}
