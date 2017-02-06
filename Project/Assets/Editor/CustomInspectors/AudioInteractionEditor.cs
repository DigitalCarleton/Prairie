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
	}

}
