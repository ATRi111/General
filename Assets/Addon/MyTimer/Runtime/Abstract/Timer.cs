using UnityEngine;
using UnityEngine.Events;

namespace MyTimer
{
    public class TimerBase
    {
        [SerializeField]
        protected bool paused;

        [SerializeField]
        protected bool completed;

        [SerializeField]
        protected float time;
        /// <summary>
        /// 经过的时间
        /// </summary>
        public float Time
        {
            get => time;
            set => time = value;
        }
        [SerializeField]
        protected float duration;
        /// <summary>
        /// 总时间
        /// </summary>
        public float Duration => duration;
        /// <summary>
        /// 到达的百分比（0～1)
        /// </summary>
        public float Percent => Mathf.Clamp01(Time / Duration);

        public TimerBase()
        {
            paused = true;
        }
    }

    //Timer类的功能类似DoTween（通过继承来实现各种不同的功能），可用于代替部分协程

    /// <summary>
    /// 描述一段随时间的变化
    /// </summary>
    /// <typeparam name="TValue">变化过程中的返回值类型</typeparam>
    /// <typeparam name="TLerp">计算返回值的方法</typeparam>
    [System.Serializable]
    public class Timer<TValue, TLerp> : TimerBase, ITimer where TLerp : ILerp<TValue>, new()
    {
        protected GameCycle gameCycle;

        /// <summary>
        /// 是否暂停；持有Timer的游戏物体被摧毁前，一定要确Timer的Paused==true
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

        /// <summary>
        /// 是否完成
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
                        AfterComplete?.Invoke(Target);
                    }
                }
            }
        }

        /// <summary>
        /// 初值
        /// </summary>
        public TValue Origin { get; protected set; }
        /// <summary>
        /// 终值
        /// </summary>
        public TValue Target { get; protected set; }

        public ILerp<TValue> Lerp { get; protected set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public TValue Current => Lerp.Value(Origin, Target, Percent, Time, Duration);

        /// <summary>
        /// 暂停时触发;参数:Current
        /// </summary>
        public event UnityAction<TValue> BeforePause;
        /// <summary>
        /// 启动/解除暂停时触发;参数:Current
        /// </summary>
        public event UnityAction<TValue> BeforeResume;
        /// <summary>
        /// 到时间时触发;参数:Target
        /// </summary>
        public event UnityAction<TValue> AfterComplete;
        /// <summary>
        /// 未暂停时每帧触发;参数:Current
        /// </summary>
        public event UnityAction<TValue> OnTick;

        public Timer()
            : base()
        {
            Lerp = new TLerp();
        }

        /// <summary>
        /// 为MyTimer设置初始属性及是否立刻启动
        /// </summary>
        public virtual void Initialize(TValue origin, TValue target, float duration, bool start = true)
        {
            gameCycle = GameCycle.Instance;
            this.duration = duration;
            Origin = origin;
            Target = target;
            if (start)
                Restart();
        }

        protected void Update()
        {
            time += UnityEngine.Time.deltaTime;
            OnTick?.Invoke(Current);
            if (Time >= Duration)
            {
                Paused = true;
                Completed = true;
            }
        }

        /// <param name="fixedTime">设为true可避免累积误差</param>
        public void Restart(bool fixedTime = false)
        {
            if (fixedTime)
                time -= Duration;
            else
                time = 0;
            Paused = false;
            Completed = false;
        }

        /// <summary>
        /// 使计时器立刻到时间
        /// </summary>
        public void ForceComplete()
        {
            time = Duration;
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