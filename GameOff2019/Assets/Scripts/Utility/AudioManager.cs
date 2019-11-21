using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
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
            s.source.loop = s.Loop;
        }
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


    private void Start()
    {

        foreach (Sound s in sounds)
        {
            if (s.IsBackgroundMusic)
            {
                PlaySound(s.Name);
            }
        }

    }

    public void PlayButtonClickSound()
    {
        PlaySound("UXClick");
    }


}
