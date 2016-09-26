using UnityEngine;
using System.Collections;

public class FirstPersonInteractor : MonoBehaviour
{
	public float interactionRange = 3;
	public bool interactionAvaliable = false;

	private Camera viewpoint;

	// Use this for initialization
	void Start () {
		viewpoint = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		if (GetAvaliableInteractions ().Length > 0) {
			interactionAvaliable = true;
		} else {
			interactionAvaliable = false;
		}

		if (Input.GetKeyDown(KeyCode.F)) {
			Interact();
		}
	}

	void OnGUI() {
		float size = interactionAvaliable ? 10 : 200;
		Rect pos = new Rect (Screen.width/2, Screen.height/2, size, size);
		GUI.Box (pos, "");
	}

	void Interact () {
		if (interactionAvaliable) {
			Interactable[] interactions = GetAvaliableInteractions();
			for (int i=0; i<interactions.Length; i++) {
				interactions[i].Interact();
			}
		}
	}

	private Interactable[] GetAvaliableInteractions () {
		RaycastHit hit;

		Vector3 origin = viewpoint.transform.position;
		Vector3 fwd = viewpoint.transform.TransformDirection (Vector3.forward);

		if (Physics.Raycast (origin, fwd, out hit, interactionRange)) {
			GameObject obj = hit.transform.gameObject;
			Interactable[] interactions = obj.GetComponents<Interactable> ();
			return interactions;
		} else {
			Interactable[] emptyList = new Interactable[0];
			return emptyList;
		}
	}
}