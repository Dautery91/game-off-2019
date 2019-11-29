using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;

    public AudioClip Clip;

    public bool IsSFX;
    public bool ShouldLoop;
    public bool IsBackgroundMusic;

    [Range(0f, 1f)]
    public float Volume;

    [Range(0f, 5f)]
    public float Pitch;

    [HideInInspector]
    public AudioSource source;
}
