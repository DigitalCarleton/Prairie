using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Prairie/Twine/Associated Twine Nodes")]
public class AssociatedTwineNodes : PromptInteraction 
{
	// This list is updated through the AssociatedTwineNodesEditor:
	public List<GameObject> associatedTwineNodeObjects = new List<GameObject>();

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

