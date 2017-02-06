using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioInteraction))]
public class AudioInteractionEditor : Editor {

	AudioInteraction audio;
	bool customAudioSource;

	public override void OnInspectorGUI() 
	{
		audio = (AudioInteraction)target;
		audio.repeatable = EditorGUILayout.Toggle ("Replayable?", audio.repeatable);
		audio.audioClip = (AudioClip)EditorGUILayout.ObjectField ("Audio Clip", audio.audioClip, typeof(AudioClip), true);

		customAudioSource = EditorGUILayout.Toggle ("Different Audio Source?", customAudioSource);
		if (customAudioSource) 
		{
			audio.audioSource = (AudioSource)EditorGUILayout.ObjectField ("Audio Source", audio.audioSource, typeof(AudioSource), true);
		}

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
