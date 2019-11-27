using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioMixer MainMixer;
    public AudioMixerGroup SFXGroup;

    public Sound[] sounds;

    public static AudioManager instance;


    private void Awake()
    {
        InitializeAudioManager();
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.Log("Sound name not found");
            return;
        }

        s.source.Play();
    }

    public void PlayButtonClickSound()
    {
        PlaySound("UXClick");
    }

    public void InitializeAudioManager()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this.gameObject);


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;

            if (s.IsSFX)
            {
                s.source.outputAudioMixerGroup = SFXGroup;
            }
        }
    }

}
