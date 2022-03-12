using System;
using System.Collections.Generic;
using UnityEngine;
using static ESound;

public class AudioController : Service
{
    [SerializeField]
    private AudioSource[] audioSources;
    private readonly Dictionary<ESound, AudioSource> audioDict = new Dictionary<ESound, AudioSource>();
    private ESound activeBgm = Null;

    private void Start()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
        BuildDict();
    }

    private void BuildDict()
    {
        AudioSource FindSound(ESound eSound)
        {
            string name = eSound.ToName();
            foreach (AudioSource item in audioSources)
            {
                if (item.gameObject.name == name)
                    return item;
            }
            if (name != null)
                Debug.LogWarning($"找不到名为{name}的Audiosource");
            return null;
        }

        foreach (ESound eSound in Enum.GetValues(typeof(ESound)))
        {
            audioDict.Add(eSound, FindSound(eSound));
        }
    }

    public void PlaySound(ESound eSound)
    {
        if (eSound == Null) 
            return;
        AudioSource audio = audioDict[eSound];
        if (audio == null || audio.isPlaying) 
            return;
        audio.Play();
    }
    public void PlaySoundLoop(ESound eSound)
    {
        if (eSound == Null) 
            return;
        AudioSource audio = audioDict[eSound];
        if (audio == null || audio.isPlaying) 
            return;
        audio.loop = true;
        audio.Play();
    }
    public void StopSound(ESound eSound)
    {
        if (eSound == Null) 
            return;
        AudioSource audio = audioDict[eSound];
        if (audio == null) 
            return;
        audio.Stop();
    }
    public void StopAllsounds()
    {
        foreach (AudioSource item in audioSources)
        {
            item.Stop();
        }
    }
    public void PlayBgm(ESound eSound)
    {
        if (eSound == activeBgm || eSound == Null)
            return;
        StopSound(activeBgm);
        PlaySound(eSound);
        activeBgm = eSound;
    }
}
