using UnityEngine;
using System.Collections;

public abstract class Interaction : MonoBehaviour
{
	public string prompt;
	public bool repeatable = true;

	[HideInInspector]
    public GameObject trigger;

	public void Interact (GameObject obj)
	{
        trigger = obj;
		if (this.enabled) {
			PerformAction ();					// run the interaction
			if (!repeatable) {
				this.enabled = false;	// prevent future interactions
			}
		}
	}

	protected abstract void PerformAction ();

}
