using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioInteraction))]
public class AudioInteractionEditor : Editor {

	AudioInteraction audio;
	bool customAudioSource;

	public void Awake()
	{
		this.audio = (AudioInteraction)target;

		// initialize synthesized property
		this.customAudioSource = false;
		if (this.audio.audioSource != null)
		{
			this.customAudioSource = true;
		}
	}

	public override void OnInspectorGUI() 
	{
		// Configuration:
		bool _repeatable = EditorGUILayout.Toggle ("Replayable?", audio.repeatable);
		AudioClip _audioClip = (AudioClip)EditorGUILayout.ObjectField ("Audio Clip", audio.audioClip, typeof(AudioClip), true);

		AudioSource _audioSource = null;
		customAudioSource = EditorGUILayout.Toggle ("Different Audio Source?", customAudioSource);
		if (customAudioSource) 
		{
			_audioSource = (AudioSource)EditorGUILayout.ObjectField ("Audio Source", audio.audioSource, typeof(AudioSource), true);
		}

		// Save:
		if (GUI.changed) {
			Undo.RecordObject(audio, "Modify Audio Interaction");
			audio.repeatable = _repeatable;
			audio.audioClip = _audioClip;
			audio.audioSource = _audioSource;
		}

		// Warnings (after properties have been updated):
		this.DrawWarnings();
	}

	public void DrawWarnings()
	{
		if (audio.audioClip == null) 
		{
			PrairieGUI.warningLabel ("No audio clip attached to object.  Please add an audio clip to the slot above.");
		}
	}
}
