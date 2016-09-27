using UnityEngine;
using System.Collections;

public class ToggleEnabledInteraction : Interactable
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

