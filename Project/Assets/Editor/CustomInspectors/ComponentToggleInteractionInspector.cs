using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComponentToggleInteraction))]
public class ComponentToggleInteractionInspector : Editor {

	ComponentToggleInteraction componentToggle;

	public override void OnInspectorGUI()
	{
		componentToggle = (ComponentToggleInteraction)target;
		componentToggle.target= PrairieGUI.drawObjectList ("Behaviours To Toggle", componentToggle.target);
	}
}
