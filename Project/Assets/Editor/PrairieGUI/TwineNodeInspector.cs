using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TwineNode))]
public class TwineNodeInspector : Editor {

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		PrairieGUI.drawList (serializedObject.FindProperty ("objectsToTrigger"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("name"));
		PrairieGUI.drawList (serializedObject.FindProperty ("tags"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("content"));
		PrairieGUI.drawList (serializedObject.FindProperty ("children"));
		PrairieGUI.drawList (serializedObject.FindProperty ("parents"));
		serializedObject.ApplyModifiedProperties ();
	}
}
