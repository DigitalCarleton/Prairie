using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Swivel))]
public class SwivelEditor : Editor {

	Swivel swivel;

	public void Awake()
	{
		this.swivel = (Swivel) target;
	}

	public override void OnInspectorGUI()
	{
		bool _openFromLeft = EditorGUILayout.Toggle (new GUIContent("Open from left?", "Door opens from right by default, check this box to open it from the left."), swivel.openFromLeft);
		
		if (GUI.changed) {
			Undo.RecordObject(swivel, "Modify Swivel");
			swivel.openFromLeft = _openFromLeft;
		}
	}
}
