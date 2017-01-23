using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Code from tutorial: http://catlikecoding.com/unity/tutorials/editor/custom-list/

public class EditorList {

	private static GUIContent
		addRowContent = new GUIContent("+", "Add Row"),
		deleteRowContent = new GUIContent("-", "Delete Row");

	private static GUILayoutOption minusButtonWidth = GUILayout.Width(20f);
	private static GUILayoutOption plusButtonWidth = GUILayout.Width(60f);

	public static T[] Show <T> (T[] array) where T : Object
	{
		EditorGUI.indentLevel += 1;
		List<T> list = new List<T> (array);
		for (int i = 0; i < list.Count; i++)
		{
			EditorGUILayout.BeginHorizontal ();
			list[i] = (T)EditorGUILayout.ObjectField("Element " + i, list[i], typeof(T), true);
			if (GUILayout.Button (deleteRowContent, EditorStyles.miniButton, minusButtonWidth))
			{
				list.RemoveAt (i);
			}
			EditorGUILayout.EndHorizontal ();
		}
		if (GUILayout.Button (addRowContent, plusButtonWidth))
		{
			list.Add (null);
		}
		EditorGUI.indentLevel -= 1;

		return (T[])list.ToArray ();
	}
}
