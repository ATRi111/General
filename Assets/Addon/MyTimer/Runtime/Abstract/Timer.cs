using UnityEngine;
using UnityEngine.Events;

namespace MyTimer
{
    //Timer�����ڴ��沿��Э�̣��ʺϴ��������ǿ�ġ���Ҫ�������ʵĻ���Ҫ���������رյ�Э��

    /// <summary>
    /// ����һ����ʱ��ı仯
    /// </summary>
    /// <typeparam name="TValue">�仯�����еķ���ֵ����</typeparam>
    /// <typeparam name="TLerp">���㷵��ֵ�ķ���</typeparam>
    [System.Serializable]
    public class Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        private GameCycle gameCycle;

        [SerializeField]
        protected bool paused;

        /// <summary>
        /// �Ƿ���ͣ������Timerǰ��һ��Ҫȷ����Paused==true
        /// </summary>
        public bool Paused
        {
            get => paused;
            set
            {
                if (paused != value)
                {
                    paused = value;
                    if (value)
                    {
                        BeforePause?.Invoke(Current);
                        gameCycle.RemoveFromGameCycle(EInvokeMode.Update, Update);
                    }
                    else
                    {
                        BeforeResume?.Invoke(Current);
                        gameCycle.AttachToGameCycle(EInvokeMode.Update, Update);
                    }
                }
            }
        }

        [SerializeField]
        protected bool completed;
        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool Completed
        {
            get => completed;
            protected set
            {
                if (completed != value)
                {
                    completed = value;
                    if (value)
                    {
                        AfterCompelete?.Invoke(Current);
                    }
                }
            }
        }

        /// <summary>
        /// ������ʱ��
        /// </summary>
        public float Time { get; protected set; }
        /// <summary>
        /// ����İٷֱȣ�0��1)
        /// </summary>
        public float Percent => Mathf.Clamp01(Time / Duration);
        /// <summary>
        /// ��ʱ��
        /// </summary>
        public float Duration { get; protected set; }
        /// <summary>
        /// ��ֵ
        /// </summary>
        public TValue Origin { get; protected set; }
        /// <summary>
        /// ��ֵ
        /// </summary>
        public TValue Target { get; protected set; }

        public ILerp<TValue> Lerp { get; protected set; }
        /// <summary>
        /// ��ǰֵ
        /// </summary>
        public TValue Current => Lerp.Value(Origin, Target, Percent, Time, Duration);

        /// <summary>
        /// ��ͣʱ����
        /// </summary>
        public event UnityAction<TValue> BeforePause;
        /// <summary>
        /// ����/�����ͣʱ����
        /// </summary>
        public event UnityAction<TValue> BeforeResume;
        /// <summary>
        /// ��ʱ��ʱ����
        /// </summary>
        public event UnityAction<TValue> AfterCompelete;
        /// <summary>
        /// δ��ͣʱÿ֡����
        /// </summary>
        public event UnityAction<TValue> OnTick;

        public Timer()
        {
            Lerp = new TLerp();
            paused = true;
        }

        /// <summary>
        /// ΪMyTimer���ó�ʼ���Լ��Ƿ���������
        /// </summary>
        public virtual void Initialize(TValue origin, TValue target, float duration, bool start = true)
        {
            gameCycle = GameCycle.Instance;
            Duration = duration;
            Origin = origin;
            Target = target;
            if (start)
                Restart();
        }

        protected void Update()
        {
            Time += UnityEngine.Time.deltaTime;
            OnTick?.Invoke(Current);
            if (Time >= Duration)
            {
                Paused = true;
                Completed = true;
            }
        }

        /// <param name="fixedTime">��Ϊtrue�ɱ����ۻ����</param>
        public void Restart(bool fixedTime = false)
        {
            if (fixedTime)
                Time -= Duration;
            else
                Time = 0;
            Paused = false;
            Completed = false;
        }

        /// <summary>
        /// ʹ��ʱ�����̵�ʱ��
        /// </summary>
        public void ForceComplete()
        {
            Time = Duration;
            OnTick?.Invoke(Current);
            Paused = true;
            Completed = true;
        }

        public override string ToString()
        {
            return $"Paused:{Paused},Completed:{Completed},Origin:{Origin},Target:{Target},Duration:{Duration}";
        }
    }
}