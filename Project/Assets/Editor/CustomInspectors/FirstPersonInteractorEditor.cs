using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FirstPersonInteractor))]
public class FirstPersonInteractorEditor : Editor {

	FirstPersonInteractor player;

	public override void OnInspectorGUI ()
	{
		player = (FirstPersonInteractor)target;

		// Principle Configuration:
		GUIContent rangeLabel = new GUIContent("Interaction Range", "The max distance a player can be from an object and interact with it.");
		player.interactionRange = EditorGUILayout.FloatField(rangeLabel, player.interactionRange);
		
		GUIContent annotationsLabel = new GUIContent("Enable Annotations", "If disabled, historical annotations are not shown in game.");
		player.annotationsEnabled = EditorGUILayout.Toggle(annotationsLabel, player.annotationsEnabled);

		// Warnings:
		this.DrawWarnings ();
		EditorUtility.SetDirty(player);
	}

	public void DrawWarnings()
	{
		GameObject owner = player.gameObject;
		var playerCompTypeA = owner.GetComponent<FirstPersonController> ();
		var playerCompTypeB = owner.GetComponent<RigidbodyFirstPersonController> ();

		if (playerCompTypeA == null && playerCompTypeB == null)
		{
			// Neither player types are attached to this gameObject!
			PrairieGUI.warningLabel("No Controller is attached to this game object.");
			PrairieGUI.warningLabel("Please add a `FirstPersonController` or `RigidbodyFirstPersonController` to this game object, to ensure your player can move around in the scene.");
			PrairieGUI.warningLabel("If these components are unavaliable, you may need to import the 'Characters' package, from Unity's Standard Assets.");
		}
	}
}
