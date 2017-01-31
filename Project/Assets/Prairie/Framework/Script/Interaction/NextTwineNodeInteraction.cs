using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NextTwineNodeInteraction : Interaction 
{
	public List<GameObject> nextTwineNodeObjects;
	public List<int> selectedTwineNodeIndices = new List<int>(){0};

	public void UpdateTwineNodeObjectsFromIndices(TwineNode[] nodes)
	{

		if (this.nextTwineNodeObjects != null) {
			// Clear the list of twine nodes and re-add them based on the new indices:
			this.nextTwineNodeObjects.Clear ();
		} else {
			// Make sure it exists!
			this.nextTwineNodeObjects = new List<GameObject> ();
		}
		foreach (int index in this.selectedTwineNodeIndices) {
			GameObject twineNodeObject = nodes [index].gameObject;
			this.nextTwineNodeObjects.Add (twineNodeObject);
		}
	}

	void OnDrawGizmosSelected()
	{
		// Draw a green line between a Twine node and its subsequent Twine nodes
		Gizmos.color = Color.green;
		foreach (GameObject twineNodeObject in nextTwineNodeObjects) {
			Gizmos.DrawLine (transform.position, twineNodeObject.transform.position);
		}
	}

	protected override void PerformAction () 
	{
		foreach (GameObject twineNodeObject in nextTwineNodeObjects) {
			TwineNode twineNode = twineNodeObject.GetComponent<TwineNode> ();

			if (twineNode != null) {
				// Activate the node!
				twineNode.Activate (this.rootInteractor);
			}
		}
	}

	override public string defaultPrompt {
		get {
			return "Progress the Story";
		}
	}
}

