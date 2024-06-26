﻿using UnityEngine;

public class SoundForDialogue : MonoBehaviour
{
    public static SoundForDialogue instance { get; private set; }

    private AudioSource source;

    private void OnEnable()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
