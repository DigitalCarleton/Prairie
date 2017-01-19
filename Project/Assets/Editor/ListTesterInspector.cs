using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ListTester))]
public class ListTesterInspector : Editor {

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
//		EditorGUILayout.PropertyField (serializedObject.FindProperty ("integers"), true);
		EditorList.Show (serializedObject.FindProperty ("integers"));

		serializedObject.ApplyModifiedProperties ();
	}
}
