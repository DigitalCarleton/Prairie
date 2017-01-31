using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PrairieGUI {

	// Things involved in drawList
	private static GUIContent
		addRowContent = new GUIContent("+", "Add Row"),
		deleteRowContent = new GUIContent("-", "Delete Row");

	private static GUILayoutOption minusButtonWidth = GUILayout.Width(20f);
	private static GUILayoutOption plusButtonWidth = GUILayout.Width(60f);

	public static void warningLabel(string text) {
		GUIStyle warningLabel = new GUIStyle(GUI.skin.label);
        warningLabel.normal.textColor = Color.red;
        warningLabel.wordWrap = true;

        GUILayout.Label(text, warningLabel);
	}

	public static T[] drawObjectList <T> (string title, T[] array) where T : UnityEngine.Object
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
			list.Add (default(T));
		}
		EditorGUI.indentLevel -= 1;

		return (T[])list.ToArray ();
	}

	public static T[] drawPrimitiveList <T> (string title, T[] array) where T : IConvertible
	{
		EditorGUILayout.PrefixLabel (title);
		EditorGUI.indentLevel += 1;
		List<T> list = new List<T> (array);
		for (int i = 0; i < list.Count; i++)
		{
			EditorGUILayout.BeginHorizontal ();
			if (typeof(T) == typeof(string)) {
				string value = Convert.ToString (list [i]);
				list [i] = (T)Convert.ChangeType(EditorGUILayout.TextField ("Element " + i, value), typeof(T));
			}
			else if (typeof(T) == typeof(int))
			{
				int value = Convert.ToInt32 (list [i]);
				list [i] = (T)Convert.ChangeType (EditorGUILayout.IntField ("Element " + i, value), typeof(T));
			}
			else if (typeof(T) == typeof(double))
			{
				double value = Convert.ToDouble (list [i]);
				list [i] = (T)Convert.ChangeType (EditorGUILayout.DoubleField ("Element " + i, value), typeof(T));
			}
			else if (typeof(T) == typeof(float))
			{
				float value = Convert.ToSingle (list [i]);
				list [i] = (T)Convert.ChangeType (EditorGUILayout.FloatField ("Element " + i, value), typeof(T));
			}
			else if (typeof(T) == typeof(long))
			{
				long value = Convert.ToInt64 (list [i]);
				list [i] = (T)Convert.ChangeType (EditorGUILayout.LongField ("Element " + i, value), typeof(T));
			}
			if (GUILayout.Button (deleteRowContent, EditorStyles.miniButton, minusButtonWidth))
			{
				list.RemoveAt (i);
			}
			EditorGUILayout.EndHorizontal ();
		}
		if (GUILayout.Button(addRowContent, plusButtonWidth))
		{
			list.Add (default(T));
		}
		EditorGUI.indentLevel -= 1;
		return (T[])list.ToArray ();
	}
}
