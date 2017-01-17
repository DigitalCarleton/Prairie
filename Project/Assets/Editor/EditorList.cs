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

	public static void Show (SerializedProperty list)
	{
		if (!list.isArray)
		{
			EditorGUILayout.HelpBox (list.name + " is neither an array nor a list!", MessageType.Error);
			return;
		}
		EditorGUILayout.PropertyField (list);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded)
		{
			for (int i = 0; i < list.arraySize; i++)
			{
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i));
				if (GUILayout.Button (deleteRowContent, EditorStyles.miniButton, minusButtonWidth))
				{
					int oldSize = list.arraySize;
					list.DeleteArrayElementAtIndex (i);
					if (list.arraySize == oldSize)
					{
						list.DeleteArrayElementAtIndex (i);
					}
				}
				EditorGUILayout.EndHorizontal ();
			}
			if (GUILayout.Button (addRowContent, plusButtonWidth))
			{
				list.arraySize += 1;
			}
		}
		EditorGUI.indentLevel -= 1;
	}
}
