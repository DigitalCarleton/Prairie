using UnityEngine;
using System.Collections;

public abstract class Interaction : MonoBehaviour
{
	public string prompt;
	public bool repeatable = true;
    protected GameObject trigger;

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

    protected void SetPlayerFrozen(bool isFrozen)
    {
        trigger.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = !isFrozen;
        trigger.GetComponent<FirstPersonInteractor>().enabled = !isFrozen;
    }

}
