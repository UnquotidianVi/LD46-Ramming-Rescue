using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float VolumeMultiplier { get { return volumeMultiplier; } set { volumeMultiplier = value; } }
    private float volumeMultiplier = 1;

    [SerializeField]
    private GameObject soundPlayerHolder;
    private List<AudioSource> soundPlayers = new List<AudioSource>();

    private void Update()
    {
        CheckForFinishedSoundPlayers();
    }

    public void PlaySound(Sound sound)
    {
        GameObject go = new GameObject("Sound Player");
        go.transform.SetParent(soundPlayerHolder.transform);
        AudioSource soundPlayer = go.AddComponent<AudioSource>();
        soundPlayer.clip = sound.audioClips[Random.Range(0, sound.audioClips.Count)];
        soundPlayer.loop = sound.isLooping;
        soundPlayer.pitch = sound.pitch;
        soundPlayer.volume = sound.volume * volumeMultiplier;

        soundPlayer.Play();
        soundPlayers.Add(soundPlayer);
    }

    private void CheckForFinishedSoundPlayers()
    {
        //Sound Players
        for (int i = 0; i < soundPlayers.Count; i++)
        {
            if (soundPlayers[i].isPlaying == false)
            {
                GameObject go = soundPlayers[i].gameObject;
                soundPlayers.Remove(soundPlayers[i]);
                i--;
                Destroy(go);
            }
        }
    }
}
