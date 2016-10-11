using UnityEngine;
using System.Collections;

public class PlayAudioInteraction : Interaction
{
    public AudioClip audioClip;
    public AudioSource audioSource;
    public bool toggle = false;


    protected override void PerformAction()
    {
        if (audioSource == null)
        {
             audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = audioClip;
        audioSource.Play();
        
    }
}
