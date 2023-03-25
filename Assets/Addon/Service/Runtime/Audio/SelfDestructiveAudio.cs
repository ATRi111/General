using UnityEngine;

namespace Services.Audio
{
    /// <summary>
    /// 控制音频播放到某一时刻后自毁，禁用或摧毁此脚本即中止自毁
    /// </summary>
    public class SelfDestructiveAudio : MonoBehaviour
    {
        private AudioSource audioSource;
        public float timing;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (audioSource.time >= timing)
                Destroy(gameObject);
        }
    }
}