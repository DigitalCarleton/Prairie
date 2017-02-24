using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Prairie/Twine/Associated Twine Nodes")]
public class AssociatedTwineNodes : PromptInteraction 
{
	// This list is updated through the AssociatedTwineNodesEditor:
	public List<GameObject> associatedTwineNodeObjects = new List<GameObject>();

	void OnDrawGizmosSelected()
	{
		// Draw a green line between a Twine node and its subsequent Twine nodes
		Gizmos.color = Color.green;
		foreach (GameObject twineNodeObject in associatedTwineNodeObjects) {
			Gizmos.DrawLine (transform.position, twineNodeObject.transform.position);
		}
	}

	protected override void PerformAction () 
	{
		foreach (GameObject twineNodeObject in associatedTwineNodeObjects) {
			TwineNode twineNode = twineNodeObject.GetComponent<TwineNode> ();

			if (twineNode != null) {
				// Activate the node!
				if (twineNode.Activate (this.rootInteractor))
				{
					return;
				}
			}
		}
	}

	override public string defaultPrompt {
		get {
			return "Progress the Story";
		}
	}
}

