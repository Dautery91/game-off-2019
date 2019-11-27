using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer MainMixer;

    //private AudioMixerGroup musicGroup;
    //private AudioMixerGroup sfxGroup;

    public void SetMasterVolume(float volume)
    {
        MainMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        MainMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        MainMixer.SetFloat("SFXVolume", volume);
    }
}
