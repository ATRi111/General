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
            AfterCompelete += AfterComplete_;
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
    public class Repeataion<TValue, TLerp> : Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        public Repeataion()
        {
            AfterCompelete += AfterComplete_;
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
    public class Metronome : Repeataion<float, DefaultValue<float>>
    {
        public virtual void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, 0f, duration, start);
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