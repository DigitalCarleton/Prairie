using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComponentToggleInteraction))]
public class ComponentToggleInteractionEditor : Editor {

	ComponentToggleInteraction componentToggle;

	public override void OnInspectorGUI ()
	{
		this.componentToggle = (ComponentToggleInteraction)target;

		componentToggle.repeatable = EditorGUILayout.Toggle ("Repeatable?", componentToggle.repeatable);
		componentToggle.target = PrairieGUI.drawObjectList<Behaviour> ("Behaviours To Toggle:", componentToggle.target);

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
