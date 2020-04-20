using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class Sound : ScriptableObject
{
    public List<AudioClip> audioClips = new List<AudioClip>();
    public bool isLooping;
    public float volume;
    public float pitch;
}
