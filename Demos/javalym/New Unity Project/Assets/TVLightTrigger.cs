using UnityEngine;
using System.Collections;

public class TVLightTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I))
		{
			GetComponent<Renderer>().enabled = true;
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			GetComponent<Renderer>().enabled = false;
		}
	}
}