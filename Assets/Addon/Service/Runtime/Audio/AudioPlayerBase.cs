using UnityEngine;
using UnityEngine.Audio;

namespace Services
{
    public class AudioPlayerBase : Service
    {
        [Other]
        protected AssetLoader assetLoader;

        [SerializeField]
        protected AudioMixer audioMixer;
        protected AudioPlayerCore core;

        protected override void Awake()
        {
            base.Awake();
            core = new AudioPlayerCore();
        }

        /// <summary>
        /// 创建MyAudioSource，默认不会循环，会自我销毁；
        /// 不应持续引用会自我销毁的MyAudioSource
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="parent">将音源设为transform的子物体，可以为null</param>
        /// <param name="play">创建时立即播放</param>
        public MyAudioSource CreateAudio(string name, Transform parent = null, bool play = true)
        {
            if (parent == null)
                parent = transform;
            GameObject asset = assetLoader.Load<GameObject>(name);
            MyAudioSource audioSource = core.CreateAudio(asset, parent);
            audioSource.SelfDestructive = true;
            if (play)
                audioSource.Play();
            return audioSource;
        }

        /// <param name="name"></param>
        /// <param name="percent">0%对应AudioMixer中的-80，100%对应0</param>
        public void SetVolume(string name, float percent)
        {
            percent = Mathf.Clamp(percent, 0.01f, 1f);
            core.SetVolume(audioMixer, name, percent);
        }
    }
}