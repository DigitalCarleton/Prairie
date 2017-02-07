using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TwineNode))]
public class TwineNodeEditor : Editor {


	TwineNode node;

	public override void OnInspectorGUI ()
	{
		node = (TwineNode)target;

		node.isDecisionNode = EditorGUILayout.Toggle ("Decision node?", node.isDecisionNode);
		node.objectsToTrigger = PrairieGUI.drawObjectList ("Objects To Trigger", node.objectsToTrigger);
		EditorGUILayout.LabelField ("Name", node.name);
		EditorGUILayout.LabelField ("Content");
		EditorGUI.indentLevel += 1;
		node.content = EditorGUILayout.TextArea (node.content);
		EditorGUI.indentLevel -= 1;
		node.children = PrairieGUI.drawObjectListReadOnly ("Children", node.children);
		GameObject[] parentArray = node.parents.ToArray ();
		parentArray = PrairieGUI.drawObjectListReadOnly ("Parents", parentArray);
		node.parents = new List<GameObject> (parentArray);

		// Save changes to the TwineNode if the user edits something in the GUI:
		if (GUI.changed)
		{
			EditorUtility.SetDirty( target );
		}

	}
}
