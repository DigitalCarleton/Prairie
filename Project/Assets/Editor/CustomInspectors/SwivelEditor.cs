using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Swivel))]
public class SwivelEditor : Editor {

	Swivel node;

	public override void OnInspectorGUI()
	{
		node = (Swivel)target;
		node.openFromLeft = EditorGUILayout.Toggle (new GUIContent("Open from left?", "Door opens from right by default, check this box to open it from the left."), node.openFromLeft);
	}
}
