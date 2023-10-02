using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(SceneLoader))]  
public class GUISceneLoaderAudio : MonoBehaviour
{
    GUISceneLoader loader;
    AudioSource audioSource;

    public List<AudioSet> StartClips;
    public List<AudioSet> UpdateClips;
    public List<AudioSet> MethodClips;

    private void Awake()
    {
        loader = GetComponent<GUISceneLoader>();
        if (GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else { audioSource = gameObject.AddComponent<AudioSource>(); }

        PlayClips(StartClips ,loader.StartAudio);
        PlayClips(UpdateClips, loader.UpdateAudio);
        PlayClips(MethodClips, loader.MethodAudio);
    }

    void PlayClips(List<AudioSet> bGMSet ,IObservable<int> observable)
    {
        if(bGMSet != null)
        {
            observable.Subscribe(n => { 
                if(bGMSet[n] != null) { bGMSet[n].Play(audioSource); }
            }).AddTo(this);
        }
    }
}
