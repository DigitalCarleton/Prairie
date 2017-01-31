using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TwineNode))]
public class TwineNodeInspector : Editor {


	TwineNode node;

	public override void OnInspectorGUI ()
	{
		node = (TwineNode)target;

		bool wasDecisionNode = node.isDecisionNode;

		node.isDecisionNode = EditorGUILayout.Toggle ("Decision node?", node.isDecisionNode);
		node.objectsToTrigger = PrairieGUI.drawObjectList ("Objects To Trigger", node.objectsToTrigger);
		node.name = EditorGUILayout.TextField ("Name", node.name);
		node.tags = PrairieGUI.drawPrimitiveList ("Tags", node.tags);
		node.content = EditorGUILayout.TextField ("Content", node.content);
		node.children = PrairieGUI.drawObjectList ("Children", node.children);
		GameObject[] parentArray = node.parents.ToArray ();
		parentArray = PrairieGUI.drawObjectList ("Parents", parentArray);
		node.parents = new List<GameObject> (parentArray);

		// Save changes to the TwineNode if the user edits something in the GUI:
		if (GUI.changed)
		{
			EditorUtility.SetDirty( target );
		}

	}
}
