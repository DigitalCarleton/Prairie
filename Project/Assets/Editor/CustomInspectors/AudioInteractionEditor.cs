using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioInteraction))]
public class AudioInteractionEditor : Editor {

	AudioInteraction audio;

	public override void OnInspectorGUI() 
	{
		audio = (AudioInteraction)target;
		audio.repeatable = EditorGUILayout.Toggle ("Replayable?", audio.repeatable);
		audio.audioClip = (AudioClip)EditorGUILayout.ObjectField ("Audio Clip", audio.audioClip, typeof(AudioClip), true);

		if (audio.audioClip == null) 
		{
			DrawWarnings ();
		}
	}

	public void DrawWarnings()
	{
		PrairieGUI.warningLabel ("No audio clip attached to object.  Please add an audio clip to the slot above.");
	}
}
