using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FirstPersonInteractor))]
public class FirstPersonInteractorInspector : Editor {

	FirstPersonInteractor player;

	public override void OnInspectorGUI ()
	{
		player = (FirstPersonInteractor)target;

		// Principle Configuration:
		GUIContent label = new GUIContent("Interaction Range", "The maximum distance a player can be from an object and interact with it.");
		player.interactionRange = EditorGUILayout.FloatField(label, player.interactionRange);

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
