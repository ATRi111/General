using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioSource : ObjectPool.MyObject
{
    protected AudioSource audioSource;
    protected float RemainingTime => audioSource.clip.length - audioSource.time;

    public override void OnCreate()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = true;
    }

    public void Play(float time = 0f)
    {
        audioSource.time = time;
        audioSource.Play();
        StopAllCoroutines();
        StartCoroutine(LifeCycle());
    }
    public void Continue()
    {
        audioSource.Play();
        StopAllCoroutines();
        StartCoroutine(LifeCycle());
    }
    public void Pause()
    {
        audioSource.Pause();
        StopAllCoroutines();
    }
    public void Stop()
    {
        audioSource.Stop();
        StopAllCoroutines();
    }

    private IEnumerator LifeCycle()
    {
        yield return RemainingTime;
        Destroy(gameObject);
    }
}
