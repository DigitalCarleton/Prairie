using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Swivel))]
public class SwivelEditor : Editor {

	Swivel swivel;

	public override void OnInspectorGUI()
	{
		swivel = (Swivel)target;
		swivel.openFromLeft = EditorGUILayout.Toggle (new GUIContent("Open from left?", "Door opens from right by default, check this box to open it from the left."), swivel.openFromLeft);
		if (GUI.changed) {
			EditorUtility.SetDirty(swivel);
		}
	}
}
