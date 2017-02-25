﻿using System.Collections;
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

	// label suitable for an error or warning about a misconfiguration
	public static void warningLabel(string text) {
		GUIStyle warningLabel = new GUIStyle(GUI.skin.label);
        warningLabel.normal.textColor = Color.red;
        warningLabel.wordWrap = true;

        GUILayout.Label(text, warningLabel);
	}

	// label suitable for a hint, a reminder of functionality
	public static void hintLabel(string text) {
		GUIStyle warningLabel = new GUIStyle(GUI.skin.label);
        warningLabel.normal.textColor = Color.gray;
        warningLabel.wordWrap = true;

        GUILayout.Label(text, warningLabel);
	}

	public static void TextFieldReadOnly (string title, string fieldContents)
	{
		GUI.enabled = false;
		EditorGUILayout.TextField (title, fieldContents);
		GUI.enabled = true;
	}

	public static void drawObjectListReadOnly <T> (string title, T[] array) where T : UnityEngine.Object
	{
		GUI.enabled = false;
		drawObjectList (title, array);
		GUI.enabled = true;
	}

	public static void drawPrimitiveListReadOnly <T> (string title, T[] array) where T : IConvertible
	{
		GUI.enabled = false;
		drawPrimitiveList (title, array);
		GUI.enabled = true;
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

	public static List<int> drawTwineNodeDropdownList (string listTitle, string itemTitle, TwineNode[] nodes, List<int> selectedIndices)
	{
		List<GameObject> objectsWithTwineNode = new List<GameObject> ();
		List<string> twineNodeNames = new List<string> ();

		foreach (TwineNode node in nodes) {
			objectsWithTwineNode.Add (node.gameObject);
			twineNodeNames.Add (node.name);
		}

		EditorGUILayout.PrefixLabel (listTitle);
		EditorGUI.indentLevel += 1;

		string[] dropdownChoices = twineNodeNames.ToArray ();

		for (int i = 0; i < selectedIndices.Count; i++)
		{
			EditorGUILayout.BeginHorizontal ();
			selectedIndices[i] = EditorGUILayout.Popup (itemTitle + ": ", selectedIndices[i], dropdownChoices);

			if (GUILayout.Button (deleteRowContent, EditorStyles.miniButton, minusButtonWidth))
			{
				selectedIndices.RemoveAt (i);
			}
			EditorGUILayout.EndHorizontal ();
		}

		if (nodes.Length != selectedIndices.Count) {
			// Only show button if there are more nodes available to select:
			if (GUILayout.Button (addRowContent, plusButtonWidth)) {
				int firstAvailableIndex = GetFirstIndexNotInList (selectedIndices, nodes.Length - 1);
				selectedIndices.Add (firstAvailableIndex);
			}
		}
		EditorGUI.indentLevel -= 1;

		return selectedIndices;
	}

	/// <summary>
	/// Gets the first index that does not appear in a list of indices.
	/// </summary>
	/// <returns>The first index not in list.</returns>
	/// <param name="indexList">Index list.</param>
	/// <param name="highestPossibleIndex">Highest possible index.</param>
	private static int GetFirstIndexNotInList(List<int> indexList, int highestPossibleIndex) {
		for (int i = 0; i <= highestPossibleIndex; i++) {
			if (!indexList.Contains (i)) {
				return i;
			}
		}

		return 0;
	}
}
