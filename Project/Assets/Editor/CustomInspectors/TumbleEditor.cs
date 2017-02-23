using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tumble))]
public class TumbleEditor : Editor {

	Tumble tumble;

	public void Awake()
	{
		this.tumble = (Tumble) target;
	}

	public override void OnInspectorGUI()
	{
		// Configuration:
		EditorGUILayout.LabelField ("Use I, J, K and L keys to rotate object, and Escape to put it down.");
		EditorGUILayout.LabelField ("Distance from player:");
		float _distance = EditorGUILayout.Slider(tumble.distance, 0, 10);

		EditorGUILayout.LabelField ("Rotation speed:");
		float _speed = EditorGUILayout.Slider(tumble.speed, 1, 30);
		
		// Save:
		if (GUI.changed) {
			Undo.RecordObject(tumble, "Modify Tumble");
			tumble.distance = _distance;
			tumble.speed = _speed;
		}
	}
}
