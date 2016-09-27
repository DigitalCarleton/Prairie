using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	private bool win = false;
	GUIStyle guistyle = new GUIStyle();

	void OnGUI() {
		guistyle.fontSize = 75;
		guistyle.normal.textColor = Color.green;
		string wintext = "You win!";
		if (win) {
			GUI.Label (new Rect (10, 10, 100, 50), wintext, guistyle);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			win = true;
		}
	}


}
