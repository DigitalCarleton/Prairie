using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Interactions/Trigger Area")]
[RequireComponent(typeof(Collider))]
public class TriggerArea : MonoBehaviour
{

    public GameObject[] additionalTargets;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		foreach (GameObject target in additionalTargets)
		{
			// Draw blue line(s) between the trigger area and all additional objects it interacts with (i.e. triggers)
			Gizmos.DrawLine (transform.position, target.transform.position);
		}
	}

	void OnTriggerEnter(Collider other)
	{

		// ensure we're being triggered by a player
		FirstPersonInteractor playerInteractor = other.gameObject.GetComponent<FirstPersonInteractor> ();
		if (playerInteractor == null)
		{
			return;
		}

		// Trigger all the Interactions on this object
		this.gameObject.InteractAll (playerInteractor.gameObject);

		// Trigger all the Interactions on our additional targets
		foreach (GameObject target in additionalTargets)
		{
			target.InteractAll (playerInteractor.gameObject);
		}
	}
    
}
