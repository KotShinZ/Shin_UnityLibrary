using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManeger : SingletonMonoBehaviour<AudioSourceManeger> 
{
    public AudioSource audioSource;
    public AudioSource oneshotSource;

    public void PlayBGM(AudioSet audioSet)
    {
        audioSet.Play(audioSource ,true);
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
    public void PlayOneShot(AudioSet audioSet)
    {
        audioSet.Play(oneshotSource , false);
    }
    public void PlayOneShot(AudioClip clip)
    {
        new AudioSet(clip).Play(oneshotSource, false);
    }
}

[System.Serializable]
public class AudioSet
{
    public AudioClip clip;
    public float volume = 0.5f;
    public float delay = 0;

    public AudioSet() { }
    public AudioSet(AudioClip clip)
    {
        this.clip = clip;
    }

    public async void Play(AudioSource source, bool loop = false)
    {
        if (clip != null)
        {
            await UniTask.Delay((int)(delay * 1000));
            source.clip = clip;
            source.volume = volume;
            if (loop)
            {
                source.loop = true;
                source.Play();
            }
            else { source.PlayOneShot(clip); }
        }

    }
}

