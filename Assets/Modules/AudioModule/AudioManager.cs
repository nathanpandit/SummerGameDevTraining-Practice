using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] List<Audio> audios = new();
    public AudioSource MusicSource = new AudioSource();
    public AudioSource SoundSource = new AudioSource();

    public new void Awake()
    {
        MusicSource = gameObject.AddComponent<AudioSource>();
        SoundSource = gameObject.AddComponent<AudioSource>();
    }


    public void PlaySound(SoundType audioType)
    {
        Audio audioToPlay = audios.Find(a => a.soundType == audioType);
        AudioClip clipToPlay = audioToPlay.clip;

        if (clipToPlay == null)
        {
            Debug.LogWarning("Clip to play is null");
            return;
        }
        
        SoundSource.clip = clipToPlay;
        
        Debug.Log($"Playing sound {audioType}");
        SoundSource.Play();
    }
    
    public void PlayMusic(MusicType audioType)
    {
        Audio audioToPlay = audios.Find(a => a.musicType == audioType);
        AudioClip clipToPlay = audioToPlay.clip;

        if (clipToPlay == null)
        {
            Debug.LogWarning("Clip to play is null");
            return;
        }
        MusicSource.clip = clipToPlay;
        MusicSource.loop = true;
        Debug.Log($"Playing music {audioType}");
        MusicSource.Play();
    }

    public void StopPlayingMusic()
    {
        MusicSource.Stop();
        Debug.Log($"Stopped playing music: {MusicSource.clip.name}");
    }
}
