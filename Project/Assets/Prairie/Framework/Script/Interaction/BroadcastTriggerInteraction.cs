using UnityEngine;
using System.Collections;

public class BroadcastTriggerInteraction : Interaction
{

	public string eventName;

	protected override void PerformAction()
	{
		BroadcastListener[] allListeners = Object.FindObjectsOfType (typeof(BroadcastListener)) as BroadcastListener[];

		foreach (BroadcastListener listener in allListeners)
		{
			if (listener.eventName == this.eventName)
			{
				listener.OnEventFires (this.trigger);
			}
		}
	}

}
