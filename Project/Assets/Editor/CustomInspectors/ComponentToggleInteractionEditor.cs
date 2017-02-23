using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComponentToggleInteraction))]
public class ComponentToggleInteractionEditor : Editor {

	ComponentToggleInteraction componentToggle;

	public void Awake()
	{
		this.componentToggle = (ComponentToggleInteraction)target;
	}

	public override void OnInspectorGUI ()
	{
		// Configuration:
		bool _repeatable = EditorGUILayout.Toggle ("Repeatable?", componentToggle.repeatable);
		Behaviour[] _targets = PrairieGUI.drawObjectList<Behaviour> ("Behaviours To Toggle:", componentToggle.target);

		// Save:
		if (GUI.changed) {
			Undo.RecordObject(componentToggle);
			componentToggle.repeatable = _repeatable;
			componentToggle.target = _targets;
		}

		// Warnings (after properties have been updated):
		this.DrawWarnings();
	}

	public void DrawWarnings()
	{
		foreach (Behaviour behaviour in componentToggle.target) 
		{
			if (behaviour == null) 
			{
				PrairieGUI.warningLabel ("You have one or more empty slots in your list of toggles.  Please fill these slots or remove them.");
				break;
			}
		}
	}
}
