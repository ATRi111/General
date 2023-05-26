using Services.Asset;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    public class AudioPlayerBase : Service, IAudioPlayer
    {
        [AutoService]
        protected IAssetLoader assetLoader;

        protected AudioPlayerCore core;

        protected override void Awake()
        {
            base.Awake();
            core = new AudioPlayerCore();
        }

        public AudioSource CreateAudioByPrefab(string identifier, Vector3 position, Transform parent = null, EControlOption option = EControlOption.SelfDestructive, float lifeSpan = 0f)
        {
            if (parent == null)
                parent = transform;
            GameObject asset = assetLoader.Load<GameObject>(identifier);
            AudioSource audioSource = core.CreateAudioByPrefab(asset, position, parent, option, lifeSpan);
            if (audioSource != null)
                audioSource.Play();
            else
                Debugger.LogError($"无法创建prefab的标识符为{identifier}的音频");
            return audioSource;
        }

        public AudioSource CreateAudioByClip(string identifier, Vector3 position, Transform parent = null, EControlOption option = EControlOption.SelfDestructive, float lifeSpan = 0f)
        {
            if (parent == null)
                parent = transform;
            AudioClip clip = assetLoader.Load<AudioClip>(identifier);
            AudioSource audioSource = core.CreateAudioByClip(clip, position, parent, option, lifeSpan);
            if (audioSource != null)
                audioSource.Play();
            else
                Debugger.LogError($"无法创建AudioClip的标识符为{identifier}的音频");
            return audioSource;
        }

        public void SetVolume(AudioMixer audioMixer, string parameter, float volume)
        {
            volume = Mathf.Clamp(volume, 0.01f, 1f);
            core.SetVolume(audioMixer, name, volume);
        }
    }
}