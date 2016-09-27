using UnityEngine;
using System.Collections;

public class Room_Enter_Trigger : MonoBehaviour {

	private bool displayText = false;
	private GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		guiStyle.fontSize = 80;
		guiStyle.normal.textColor = Color.yellow;
		guiStyle.alignment = TextAnchor.UpperCenter;
		if (displayText == true) {
			GUI.Label (new Rect (Screen.width/2 - 200, Screen.height/2 - 100, 400, 200), "YOU WIN!", guiStyle);
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Collision!");
		if (other.gameObject.CompareTag ("Player")) {
			displayText = true;
		}
	}
}