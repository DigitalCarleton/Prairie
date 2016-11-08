using UnityEngine;
using System.Collections;

/// <summary>
/// When an instance of `BroadcastTriggerInteraction` is interacted with,
/// all GameObjects with a `BroadcastListener` are queried.
/// If the `eventName` of the `BroadcastTriggerInteraction` and `BroadcastListener` match,
/// then all the interactions on the GameObject associated with the `BroadcastListener` are fired.
/// </summary>
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
				listener.gameObject.InteractAll (this.rootInteractor);
			}
		}
	}

}
