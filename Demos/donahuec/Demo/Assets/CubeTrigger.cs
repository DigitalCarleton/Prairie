using UnityEngine;
using System.Collections;

public class CubeTrigger : MonoBehaviour {

	public GameObject trigger;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnDisable()
	{
		trigger.SetActive (true);
	}
}
