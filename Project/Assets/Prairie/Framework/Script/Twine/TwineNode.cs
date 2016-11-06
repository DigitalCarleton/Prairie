using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwineNode : MonoBehaviour {

	public GameObject[] objectsToTrigger;

	[HideInInspector]
	public string pid;
	public new string name;
	public string[] tags;
	public string content;
	public GameObject[] children;
	public GameObject[] triggeredObjects;
	[HideInInspector]
	public string[] childrenNames;
	public List<GameObject> parents = new List<GameObject> ();

	/// <summary>
	/// Trigger the interactions associated with this Twine Node.
	/// </summary>
	/// <param name="interactor"> The interactor acting on this Twine Node, typically a player. </param>
	public void StartInteractions(GameObject interactor) 
	{
		if (this.enabled) 
		{
			foreach (GameObject gameObject in objectsToTrigger) 
			{
				gameObject.InteractAll (interactor);
			}
		}
	}

	public void Activate() 
	{
		this.enabled = true;

		// Show body text of story node here?
		Debug.Log(this.content);
	}

	public void Deactivate() 
	{
		this.enabled = false;
	}

	/// <summary>
	/// Check if this Twine Node has an active parent node.
	/// </summary>
	/// <returns><c>true</c>, if there is an active parent node, <c>false</c> otherwise.</returns>
	public bool HasActiveParentNode() 
	{
		foreach (GameObject parent in parents) 
		{
			if (parent.GetComponent<TwineNode> ().enabled) 
			{
				return true;
			}
		}

		return false;
	}

	public void DeactivateAllParents()
	{
		foreach (GameObject parent in parents) 
		{
			parent.GetComponent<TwineNode> ().Deactivate ();
		}
	}
}
