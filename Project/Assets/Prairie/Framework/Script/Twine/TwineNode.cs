using UnityEngine;
using System.Collections;

public class TwineNode : MonoBehaviour {

	[HideInInspector]
	public string pid;
	public new string name;
	public string[] tags;
	public string content;
	public string[] childrenNames;
	public GameObject[] children;

	/// <summary>
	/// Trigger the interactions associated with this Twine Node.
	/// </summary>
	/// <param name="interactor"> The interactor acting on this Twine Node, typically a player. </param>
	public void StartInteractions(GameObject interactor) {
		foreach (GameObject gameObject in objectsToTrigger) {
			gameObject.InteractAll(interactor);
		}
	}
}
