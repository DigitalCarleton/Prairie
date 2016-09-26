using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public virtual void Interact () {
		Debug.Log ("Abstract Interaction");
	}
}

