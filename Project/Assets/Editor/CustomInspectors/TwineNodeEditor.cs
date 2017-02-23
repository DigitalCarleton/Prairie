using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TwineNode))]
public class TwineNodeEditor : Editor {


	TwineNode node;

	public void Awake()
	{
		this.node = (TwineNode) target;
	}

	public override void OnInspectorGUI ()
	{
		// Configuration:
		bool _isDecisionNode = EditorGUILayout.Toggle ("Decision node?", node.isDecisionNode);
		GameObject[] _objectsToTrigger = PrairieGUI.drawObjectList ("Objects To Trigger", node.objectsToTrigger);

		EditorGUILayout.LabelField ("Name", node.name);
		EditorGUILayout.LabelField ("Content");
		EditorGUI.indentLevel += 1;
		string _content = EditorGUILayout.TextArea (node.content);
		EditorGUI.indentLevel -= 1;

		// Read-Only Display:
		PrairieGUI.drawObjectListReadOnly ("Children", node.children);
		PrairieGUI.drawObjectListReadOnly ("Parents", node.parents.ToArray ());

		// Save changes to the TwineNode if the user edits something in the GUI:
		if (GUI.changed) {
			Undo.RecordObject(node, "Modify Twine Node");
			node.isDecisionNode = _isDecisionNode;
			node.objectsToTrigger = _objectsToTrigger;
			node.content = _content;
		}
	}
}
