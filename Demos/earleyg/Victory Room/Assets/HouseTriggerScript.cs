using UnityEngine;
using System.Collections;

public class HouseTriggerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("yes");

	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) {
		print ("You did it!");
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}
			
}
