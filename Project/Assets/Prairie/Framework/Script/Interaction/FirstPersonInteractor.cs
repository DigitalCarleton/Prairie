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
	public List<Annotation> areaAnnotationsInRange = new List<Annotation>();

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
		if (areaAnnotationsInRange.Count != 0)
		{
			for (int a = 0; a < areaAnnotationsInRange.Count; a++)
			{
				if (Input.GetKeyDown ((a+1).ToString()))
				{
					areaAnnotationsInRange[a].Interact (this.gameObject);
				}
			}
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
			Annotation annotation = this.highlightedObject.GetComponent<Annotation> ();
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

	private void drawToolbar(List<Annotation> annotations)
	{
		if (annotations.Count != 0)
		{
			float xpos = 0.1f * Screen.width;
			float ypos = 0.7f * Screen.height;
			int button = 0;

			// Make a background box
			GUI.Box(new Rect (xpos, ypos, 250, 120), "Area Annotations");
			xpos += 10;

			// Make list of buttons, paired with annotation summaries
			foreach (Annotation a in annotations)
			{
				ypos += 25;
				button += 1;
				GUI.Button (new Rect (xpos, ypos, 20, 20), string.Format("{0}", button));
				GUI.Label (new Rect (xpos + 30, ypos, 150, 20), a.summary);
			}
		}
	}

	/// --- Handling Interaction ---

	private void AttemptInteract ()
	{
		if (highlightedObject == null) {
			return;
		}
		
		foreach (Interaction i in this.highlightedObject.GetComponents<Interaction> ())
		{
			if (i is Annotation || i is Slideshow)
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
			if (i.enabled && (i is Annotation || i is Slideshow))
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