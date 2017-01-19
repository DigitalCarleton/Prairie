using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ListTester))]
public class ListTesterInspector : Editor {

	ListTester Tester;

	public override void OnInspectorGUI ()
	{
		Tester = (ListTester)target;
		EditorList.Show (Tester.objects);
//		EditorGUILayout.TextField(Tester.objects);
//		EditorGUILayout.PropertyField (serializedObject.FindProperty ("integers"), true);
//		EditorList.Show (serializedObject.FindProperty ("integers"));

	}
}
