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

		// For the node to be a decision node, we'll say it has 
		//	to have a "prairie_decision" tag, AND it has to have 
		//	the isDecisionNode checkbox checked.
		bool hasDecisionTag = node.tags.Contains (TwineNode.PRAIRIE_DECISION_TAG);

		if (wasDecisionNode && !(node.isDecisionNode && hasDecisionTag)) {
			// Then ensure there is NO TAG, and the checkbox is UNCHECKED
			List<string> tagList = new List<string>(node.tags);
			tagList.Remove (TwineNode.PRAIRIE_DECISION_TAG);

			node.tags = tagList.ToArray ();
			node.isDecisionNode = false;
		} else if (!wasDecisionNode && (node.isDecisionNode || hasDecisionTag)) {
			// Then ensure there is a TAG, and the checkbox is CHECKED
			if (!hasDecisionTag) {
				// Only add the tag if it isn't already there!
				List<string> tagList = new List<string> (node.tags);
				tagList.Add (TwineNode.PRAIRIE_DECISION_TAG);
				node.tags = tagList.ToArray ();
			}
			node.isDecisionNode = true;
		}

		// Save changes to the TwineNode if the user edits something in the GUI:
		if (GUI.changed)
		{
			EditorUtility.SetDirty( target );
		}

	}
}
