using UnityEngine;
using System.Collections;

public class TriggerInteraction : Interaction{

    public GameObject[] TriggerInteractions;

	protected override void PerformAction()
    {
        for (int i = 0; i < TriggerInteractions.Length; i++)
        {
            foreach (Interaction k in TriggerInteractions[i].GetComponents<Interaction>())
            {
                k.Interact(trigger);
            }
        }
    }
}
