using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FirstPersonInteractor))]
public class FirstPersonInteractorEditor : Editor {

	FirstPersonInteractor player;

	public void Awake()
	{
		player = (FirstPersonInteractor)target;
	}

	public override void OnInspectorGUI ()
	{
		// Principle Configuration:
		GUIContent rangeLabel = new GUIContent("Interaction Range", "The max distance a player can be from an object and interact with it.");
		float _interactionRange = EditorGUILayout.FloatField(rangeLabel, player.interactionRange);
		
		GUIContent annotationsLabel = new GUIContent("Enable Annotations", "If disabled, historical annotations are not shown in game.");
		bool _annotationsEnabled = EditorGUILayout.Toggle(annotationsLabel, player.annotationsEnabled);

		// Save:
		if (GUI.changed) {
			Undo.RecordObject(player, "Modify Player");
			player.interactionRange = _interactionRange;
			player.annotationsEnabled = _annotationsEnabled;
		}

		// Warnings:
		this.DrawWarnings ();
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
