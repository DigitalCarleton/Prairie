using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class TriggerArea : MonoBehaviour {

    public GameObject[] additionalTargets;

	void OnTriggerEnter(Collider other) {

		var playerInteractor = other.gameObject.GetComponent<FirstPersonInteractor> ();
		if (playerInteractor == null) {
			return;
		}

		// Trigger all the Interactions on this object...
		foreach (Interaction action in this.GetComponents<Interaction>()) {
			action.Interact (playerInteractor.gameObject);
		}

		// Trigger all the Interactions on our additional targets...
		foreach (GameObject t in additionalTargets) {
			foreach (Interaction action in t.GetComponents<Interaction>()) {
				action.Interact(playerInteractor.gameObject);
			}
		}
	}
    
}
