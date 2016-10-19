using UnityEngine;
using System.Collections;

public class TriggerInteraction : Interaction{

    public Interaction[] TriggerInteractions;

	protected override void PerformAction()
    {
        for (int i = 0; i < TriggerInteractions.Length; i++)
        {
            TriggerInteractions[i].Interact(trigger);
        }
    }
}
