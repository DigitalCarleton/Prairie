using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour
{
	public string prompt;

	public virtual void Interact () {
		Debug.Log ("Abstract Interaction");
	}

}
