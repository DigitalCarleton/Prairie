using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComponentToggleInteraction))]
public class ComponentToggleInteractionEditor : Editor {

	ComponentToggleInteraction componentToggle;

	public override void OnInspectorGUI()
	{
		componentToggle = (ComponentToggleInteraction)target;
		componentToggle.target = PrairieGUI.drawObjectList ("Behaviours To Toggle", componentToggle.target);

		for (int i = 0; i < componentToggle.target.Length; i++) 
		{
			if (componentToggle.target [i] == null) 
			{
				DrawWarnings ();
				break;
			}
		}
	}

	public void DrawWarnings()
	{
		PrairieGUI.warningLabel ("You have one or more empty slots in your list of toggles.  Please fill these slots or remove them.");
	}
}
