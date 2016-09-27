using UnityEngine;
using System.Collections;

public class TriggeredInteractor : MonoBehaviour {

	public Interactable target;

	public bool oneTimeOnly = true;
	private int timesActivated = 0;

	void OnTriggerEnter(Collider other)
	{
		if ((oneTimeOnly && timesActivated < 1) || !oneTimeOnly) {
			target.Interact ();
			timesActivated++;
		}
	}

}
