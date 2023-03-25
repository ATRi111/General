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

        /// <summary>
        /// 通过prefab创建音源
        /// </summary>
        /// <param name="identifier">用于确定prefab的标识符</param>
        /// <param name="position">播放音频的位置</param>
        /// <param name="parent">将音频设为谁的子物体</param>
        /// <param name="option">自毁选项</param>
        /// <param name="time">此参数的含义取决于option。设为0表示和音频长度一致</param>
        public AudioSource CreateAudioByPrefab(string identifier, Vector3 position, Transform parent = null, EControlOption option = EControlOption.SelfDestructive, float time = 0f)
        {
            if (parent == null)
                parent = transform;
            GameObject asset = assetLoader.Load<GameObject>(identifier);
            AudioSource audioSource = core.CreateAudioByPrefab(asset, position, parent, option, time);
            if (audioSource != null)
                audioSource.Play();
            else
                Debugger.LogError($"无法创建prefab的标识符为{identifier}的音频");
            return audioSource;
        }

        /// <summary>
        /// 通过AudioClip创建音源
        /// </summary>
        /// <param name="identifier">用于确定AudioClip的标识符</param>
        /// <param name="position">播放音频的位置</param>
        /// <param name="parent">将音频设为谁的子物体</param>
        /// <param name="option">自毁选项</param>
        /// <param name="time">此参数的含义取决于option。设为0表示和音频长度一致</param>
        public AudioSource CreateAudioByClip(string identifier, Vector3 position, Transform parent = null, EControlOption option = EControlOption.SelfDestructive, float lifeSpan = 0)
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