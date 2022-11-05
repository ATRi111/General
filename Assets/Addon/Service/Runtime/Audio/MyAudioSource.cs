using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    [RequireComponent(typeof(AudioSource))]
    public class MyAudioSource : MonoBehaviour
    {
        /// <summary>
        /// �������
        /// </summary>
        public UnityAction<MyAudioSource> Complete;

        protected GlobalGameCycle gameCycle;
        /// <summary>
        /// ���MyAudioSource��AudioSource��API�����˷�װ��Ӧ����MyAudioSource�е�API
        /// </summary>
        public AudioSource AudioSource { get; protected set; }

        public float TotalTime => AudioSource.clip.length;
        public float CurrentTime => AudioSource.time;

        public bool Loop
        {
            get => AudioSource.loop;
            set
            {
                AudioSource.loop = value;
                if (value)
                    selfDestructive = false;
            }
        }

        private bool selfDestructive;
        public bool SelfDestructive
        {
            get => selfDestructive;
            set
            {
                if (value != selfDestructive)
                {
                    if (value)
                    {
                        if (Loop)
                        {
                            Debug.LogWarning("ѭ���е���Ƶ�����Ի�");
                            return;
                        }
                        gameCycle.AttachToGameCycle(EInvokeMode.Update, SelfDestroy);
                        selfDestructive = true;
                    }
                    else
                    {
                        gameCycle.RemoveFromGameCycle(EInvokeMode.Update, SelfDestroy);
                        selfDestructive = false;
                    }
                }
            }
        }

        private void Awake()
        {
            gameCycle = ServiceLocator.Get<GlobalGameCycle>();
            AudioSource = GetComponent<AudioSource>();
            if (AudioSource.playOnAwake)
                Debug.LogWarning($"{gameObject}��playOnAwake��Ӧ��Ϊtrue");
        }

        public virtual void Play(float time = 0f, bool loop = false)
        {
            AudioSource.time = time;
            Loop = loop;
            AudioSource.Play();
        }
        public virtual void Continue()
            => AudioSource.UnPause();
        public virtual void Pause()
            => AudioSource.Pause();
        public virtual void Stop()
            => AudioSource.Stop();

        private void SelfDestroy()
        {
            if (TotalTime - CurrentTime < 0.01f)
                Destroy();
        }

        public virtual void Destroy()
        {
            Complete?.Invoke(this);
            Complete = null;
            gameCycle.RemoveFromGameCycle(EInvokeMode.Update, SelfDestroy);
            Destroy(gameObject);
        }
    }
}