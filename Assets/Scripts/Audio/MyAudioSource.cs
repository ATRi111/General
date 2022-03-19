using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MyAudioSource : MonoBehaviour
{
    protected AudioSource audioSource;
    public float TotalTime => audioSource.clip.length;
    public float CurrentTime => audioSource.time;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(float time = 0f, bool loop = false)
    {
        audioSource.time = time;
        audioSource.loop = loop;
        audioSource.Play();
        StopAllCoroutines();
        if (!loop)
            StartCoroutine(LifeCycle());
    }

    public void Continue()
    {
        audioSource.UnPause();
        StopAllCoroutines();
        if (!audioSource.loop)
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
        yield return new WaitForSeconds(TotalTime - CurrentTime);
        Destroy(gameObject);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
