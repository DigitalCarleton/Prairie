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
	[HideInInspector]
	public string[] childrenNames;
	public List<GameObject> parents = new List<GameObject> ();
	public bool isDecisionNode;

	private bool isMinimized = false;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Tab))
		{
			this.ToggleMinimize ();
		}
	}

	public void OnGUI()
	{
		if (this.enabled && !this.isMinimized) {

			float frameWidth = Screen.width / 3;
			Rect frame = new Rect (10, 10, frameWidth, Screen.height);

			GUI.BeginGroup (frame);
			GUIStyle style = new GUIStyle (GUI.skin.box);
			style.wordWrap = true;
			style.fixedWidth = frameWidth;
			GUILayout.Box (this.content, style);
			GUI.EndGroup ();

		} else if (this.enabled && this.isMinimized) 
		{
		
			// Draw minimized GUI instead
			Rect frame = new Rect (10, 10, 10, 10);

			GUI.Box (frame, "");

		}
	}

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

	/// <summary>
	/// Activate this TwineNode (provided it isn't already
	/// 	active/enabled and it has some active parent)
	/// </summary>
	/// <param name="interactor">The interactor.</param>
	public void Activate(GameObject interactor)
	{
		if (!this.enabled && this.HasActiveParentNode()) 
		{
			this.enabled = true;
			this.isMinimized = false;
			this.DeactivateAllParents ();
			this.StartInteractions (interactor);
		}
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

	/// <summary>
	/// Deactivate all parents of this Twine Node.
	/// </summary>
	private void DeactivateAllParents()
	{
		foreach (GameObject parent in parents) 
		{
			parent.GetComponent<TwineNode> ().Deactivate ();
		}
	}

	private void ToggleMinimize()
	{
		this.isMinimized = !this.isMinimized;
	}

	// GIZMOS

	void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		foreach (GameObject child in this.children)
		{
			Gizmos.DrawLine(transform.position, child.transform.position);
		}

		Gizmos.color = Color.gray;

		// TODO: draw text and such...
		// 		 just mark position with a sphere for now...
		Gizmos.DrawSphere(transform.position, 0.1f);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		foreach (GameObject gameObject in objectsToTrigger)
		{
			// Draw cyan line(s) between the current Twine node and the object(s) it triggers
			Gizmos.DrawLine(transform.position, gameObject.transform.position);
		}
	}
}
