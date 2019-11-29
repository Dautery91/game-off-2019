using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioMixer MainMixer;
    public AudioMixerGroup SFXGroup;
    public AudioMixerGroup BGMusicGroup;

    public Sound[] sounds;

    public static AudioManager instance;
    private bool isInitialized = false;

    private BackgroundMusic backgroundMusic = null;

    private string musicName = null;
    private string prevMusicName = null;

    private void Awake()
    {
        InitializeAudioManager();
        BackgroundMusicSwitch();
    }

    private void OnLevelWasLoaded(int level)
    {
        BackgroundMusicSwitch();
    }

    private void BackgroundMusicSwitch()
    {
        backgroundMusic = FindObjectOfType<BackgroundMusic>();

        if (backgroundMusic != null)
        {
            musicName = backgroundMusic.BackgroundTrackName;

            if (musicName != prevMusicName)
            {
                StopSound(prevMusicName);
                PlaySound(musicName);
                prevMusicName = musicName;
            }

        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            
            return;
        }

        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            
            return;
        }

        s.source.Stop();

    }

    public void PlayButtonClickSound()
    {
        PlaySound("UXClick");
    }

    public void PlayButtonClickBackSound()
    {
        PlaySound("UXClickBack");
    }

    public void InitializeAudioManager()
    {
        if (!isInitialized)
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
                    s.source.playOnAwake = false;
                    s.source.outputAudioMixerGroup = SFXGroup;

                    if (s.ShouldLoop)
                    {
                        s.source.loop = true;
                    }

                }

                if (s.IsBackgroundMusic)
                {
                    s.source.outputAudioMixerGroup = BGMusicGroup;
                    s.source.loop = true;
                    s.source.playOnAwake = false;
                    //s.source.playOnAwake = false;
                    //s.source.Play();
                }
            }



            isInitialized = true;
        }


    }

}
