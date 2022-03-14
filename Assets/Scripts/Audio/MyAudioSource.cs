using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MyAudioSource : MonoBehaviour
{
    protected AudioSource audioSource;
    public float TotalTime => audioSource.clip.length;
    public float Time => audioSource.time;
    public float RemainingTime => TotalTime - Time;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        yield return new WaitForSeconds(RemainingTime);
        Destroy(gameObject);
    }
}
