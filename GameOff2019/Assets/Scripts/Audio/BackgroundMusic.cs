using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusic : MonoBehaviour
{
    private AudioClip MusicClip;
    private AudioSource source;

    [HideInInspector]
    public bool isPlaying = false;

    private void Awake()
    {
        source = this.gameObject.GetComponent<AudioSource>();

    }

    public void PlayBackgroundMusic()
    {
        source.Play();
        isPlaying = true;

    }
}
