using UnityEngine;
using System.Collections;

public class ComponentToggleInteraction : Interaction 
{
	public Behaviour[] target;

	protected override void PerformAction ()
	{
		for (int i = 0; i < target.Length; i++)
		{
			target[i].enabled = !target[i].enabled;
		}
	}
}
