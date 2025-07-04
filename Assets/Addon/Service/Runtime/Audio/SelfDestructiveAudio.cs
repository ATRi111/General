using System;
using UnityEngine;

namespace Services.Audio
{
    /// <summary>
    /// 控制音频播放到某一时刻后自毁，禁用或摧毁此脚本即中止自毁
    /// </summary>
    public class SelfDestructiveAudio : MonoBehaviour
    {
        public AudioSource audioSource;
        public float timing;
        public Action BeforeDestroy;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (audioSource.time >= timing || audioSource.time == 0 && audioSource.isPlaying == false)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            BeforeDestroy?.Invoke();
            BeforeDestroy = null;
        }
    }
}