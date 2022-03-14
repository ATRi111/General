using ObjectPool;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private AudioPlayer audioPlayer;
    private ObjectManager objectManager;
    private MyAudioSource audioSource;

    private void Awake()
    {
        audioPlayer = ServiceLocator.Instance.GetService<AudioPlayer>();
        objectManager = ServiceLocator.Instance.GetService<ObjectManager>();
        audioSource = audioPlayer.CreateAudio("AudioSource/とおりゃんせ", transform, false);
        audioSource.Play(audioSource.TotalTime - 10f);
    }
}
