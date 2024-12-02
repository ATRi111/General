namespace MyTimer
{
    [System.Serializable]
    /// <summary>
    /// 基本的往复变化
    /// </summary>
    public class Circulation<TValue, TLerp> : Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        public Circulation()
        {
            AfterComplete += AfterComplete_;
        }

        protected virtual void AfterComplete_(TValue _)
        {
            (Target, Origin) = (Origin, Target);
            Restart(true);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 基本的反复变化
    /// </summary>
    public class Repetition<TValue, TLerp> : Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        public Repetition()
        {
            AfterComplete += AfterComplete_;
        }

        protected virtual void AfterComplete_(TValue _)
        {
            Restart(true);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 不使用值，仅周期性调用方法
    /// </summary>
    public class Metronome : Repetition<float, CurrentTime>
    {
        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, duration, duration, start);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 仅计时
    /// </summary>
    public class TimerOnly : Timer<float, CurrentTime>
    {
        public void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, duration, duration, start);
        }
    }
}