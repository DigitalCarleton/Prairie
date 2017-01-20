using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrairieGUI {

	// Things involved in drawList
	private static GUIContent
		addRowContent = new GUIContent("+", "Add Row"),
		deleteRowContent = new GUIContent("-", "Delete Row");

	private static GUILayoutOption minusButtonWidth = GUILayout.Width(20f);
	private static GUILayoutOption plusButtonWidth = GUILayout.Width(60f);

	public static T[] drawList <T> (string title, T[] array) where T : Object
	{
		EditorGUILayout.PrefixLabel (title);
		EditorGUI.indentLevel += 1;
		List<T> list = new List<T> (array);
		for (int i = 0; i < list.Count; i++)
		{
			EditorGUILayout.BeginHorizontal ();
			list [i] = (T)EditorGUILayout.ObjectField ("Element " + i, list [i], typeof(T), true);
			if (GUILayout.Button (deleteRowContent, EditorStyles.miniButton, minusButtonWidth))
			{
				list.RemoveAt (i);
			}
			EditorGUILayout.EndHorizontal ();
		}
		if (GUILayout.Button(addRowContent, plusButtonWidth))
		{
			list.Add (null);
		}
		EditorGUI.indentLevel -= 1;

		return (T[])list.ToArray ();
	}
}
