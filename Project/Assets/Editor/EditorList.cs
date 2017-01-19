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

	public static void Show (Object[] array)
	{
		EditorGUI.indentLevel += 1;
		List<Object> list = new List<Object> (array);
		for (int i = 0; i < list.Count; i++)
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.TextField ("Element 0: ", list [i].ToString ());
			if (GUILayout.Button (deleteRowContent, EditorStyles.miniButton, minusButtonWidth))
			{
				list.RemoveAt (i);
//				int oldSize = list.Length;
//				list.DeleteArrayElementAtIndex (i);
//				if (list.arraySize == oldSize)
//				{
//					list.DeleteArrayElementAtIndex (i);
//				}
			}
			EditorGUILayout.EndHorizontal ();
		}
		array = list.ToArray ();
//		if (GUILayout.Button (addRowContent, plusButtonWidth))
//		{
//			Object[] newArray = new Object[array.Length + 1];
//			array.CopyTo (newArray, 0);
//			array = newArray;
//		}
		EditorGUI.indentLevel -= 1;
	}
}
