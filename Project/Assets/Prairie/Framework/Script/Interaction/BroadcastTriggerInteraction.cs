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
			if (string.Equals(listener.eventName, this.eventName, System.StringComparison.CurrentCultureIgnoreCase))
			{
				listener.OnEventFires (this.trigger);
			}
		}
	}

}
