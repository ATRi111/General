using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class Sample : MonoBehaviour
{
    private AudioController audioController;
    private ObjectManager objectManager;
    private MyAudioSource audioSource;

    private void Awake()
    {
        audioController = ServiceLocator.Instance.GetService<AudioController>();
        objectManager = ServiceLocator.Instance.GetService<ObjectManager>();
        audioSource = audioController.CreateAudio("AudioSource/とおりゃんせ",transform,false);
        audioSource.Play(audioSource.TotalTime - 10f);
    }
}
