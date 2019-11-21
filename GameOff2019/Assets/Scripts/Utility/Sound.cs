using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume;

    [Range(0f, 5f)]
    public float Pitch;

    public bool Loop;
    public bool IsBackgroundMusic;

    [HideInInspector]
    public AudioSource source;
}
