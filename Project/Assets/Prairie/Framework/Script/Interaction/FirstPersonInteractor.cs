using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class FirstPersonInteractor : MonoBehaviour
{
	// Raycast-related
	public float interactionRange = 3;
	private Camera viewpoint;

	// Selection-related
	private GameObject highlightedObject;
	public List<AnnotationInteraction> areaAnnotationsInRange = new List<AnnotationInteraction>();

	// Control-related
	[HideInInspector]
	private bool drawsGUI = true;

	/// --- Game Loop ---

	void Start ()
	{
		viewpoint = Camera.main;
	}

	void Update ()
	{
		// update our highlighted object
		this.highlightedObject = this.GetHighlightedObject();

		// process input
		if (Input.GetMouseButtonDown (0))
		{
			// left click
			this.AttemptInteract ();
		}
		if (Input.GetMouseButtonDown (1))
		{
			// right click
			this.AttemptReadAnnotation ();
		}
	}

	/// --- GUI ---

	void OnGUI()
	{
		if (!this.drawsGUI)
		{
			// hide all GUI in certain contexts (such as while slideshow is playing, etc.)
			return;
		}


		if (this.highlightedObject != null)
		{
			// draw prompt on highlighted object
			Prompt prompt = this.highlightedObject.GetComponent<Prompt> ();
			if (prompt != null)
			{
				prompt.DrawPrompt();
			}

			// draw potential stub on highlighted annotation object
			AnnotationInteraction annotation = this.highlightedObject.GetComponent<AnnotationInteraction> ();
			if (annotation != null)
			{
				annotation.DrawSummary();
			}
		}
		else
		{
			// draw a crosshair when we have no highlighted object
			Rect frame = new Rect (Screen.width / 2, Screen.height / 2, 10, 10);
			GUI.Box (frame, "");
		}
		
		// draw toolbar with our set of accessable area annotations
		this.drawToolbar(this.areaAnnotationsInRange);
	}

	private void drawToolbar(List<AnnotationInteraction> annotations)
	{
		// TODO: Draw a preview (and input button) for each value in `annotations`

		// do not use `this.areaAnnotationsInRange`
		// This function may move one day, so it'd be better to keep it pure
	}

	/// --- Handling Interaction ---

	private void AttemptInteract ()
	{
		if (highlightedObject == null) {
			return;
		}
		
		foreach (Interaction i in this.highlightedObject.GetComponents<Interaction> ()) 
		{
			if (i is AnnotationInteraction || i is SlideshowInteraction)
			{
				// special cases, handled by `AttemptReadAnnotation`
				continue;
			}

			if (i.enabled)
			{ 
				i.Interact (this.gameObject);		
			} 
		}
	}

	private void AttemptReadAnnotation ()
	{
		if (highlightedObject == null) {
			return;
		}

		foreach (Interaction i in this.highlightedObject.GetComponents<Interaction> ()) 
		{
			if (i.enabled && (i is AnnotationInteraction || i is SlideshowInteraction))
			{
				i.Interact (this.gameObject);
			}
		}
	}

	private GameObject GetHighlightedObject()
	{
		// perform a raycast from the main camera to an object in front of it
		// the object must have a collider to be hit, and an `Interaction` to be added
		// to this interactor's interaction list

		Vector3 origin = viewpoint.transform.position;
		Vector3 fwd = viewpoint.transform.TransformDirection (Vector3.forward);

		RaycastHit hit;		// we pass this into the raycast function and it populates it with a result

		if (Physics.Raycast (origin, fwd, out hit, interactionRange))
		{
			if (hit.collider.isTrigger)
			{
				// ignore non-physical colliders, such as trigger areas
				return null;
			}

			return hit.transform.gameObject;
		}
		else
		{
			return null;
		}
	}

	// --- Changing Player Abilities ---

	public void SetDrawsGUI(bool shouldDraw)
	{
		this.drawsGUI = shouldDraw;
	}

	public void SetCanMove(bool canMove)
	{
		var playerCompTypeA = this.gameObject.GetComponent<FirstPersonController> ();
		var playerCompTypeB = this.gameObject.GetComponent<RigidbodyFirstPersonController> ();

		if (playerCompTypeA != null)
		{
			playerCompTypeA.enabled = canMove;
		}
		if (playerCompTypeB != null)
		{
			playerCompTypeB.enabled = canMove;
		}
	}

}