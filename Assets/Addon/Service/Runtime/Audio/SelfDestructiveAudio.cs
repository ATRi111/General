using UnityEngine;

namespace Services.Audio
{
    /// <summary>
    /// ������Ƶ���ŵ�ĳһʱ�̺��Ի٣����û�ݻٴ˽ű�����ֹ�Ի�
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