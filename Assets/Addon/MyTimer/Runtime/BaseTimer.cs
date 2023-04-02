namespace MyTimer
{
    [System.Serializable]
    /// <summary>
    /// �����������仯
    /// </summary>
    public class Circulation<TValue, TLerp> : Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        public Circulation()
        {
            AfterCompelete += MyOnComplete;
        }

        private void MyOnComplete(TValue _)
        {
            (Target, Origin) = (Origin, Target);
            Restart(true);
        }
    }

    [System.Serializable]
    /// <summary>
    /// �����ķ����仯
    /// </summary>
    public class Repeataion<TValue, TLerp> : Timer<TValue, TLerp> where TLerp : ILerp<TValue>, new()
    {
        public Repeataion()
        {
            AfterCompelete += MyOnComplete;
        }

        private void MyOnComplete(TValue _)
        {
            Restart(true);
        }
    }

    [System.Serializable]
    /// <summary>
    /// ��ʹ��ֵ���������Ե��÷���
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
    /// ����ʱ
    /// </summary>
    public class TimerOnly : Timer<float, CurrentTime>
    {
        public void Initialize(float duration, bool start = true)
        {
            base.Initialize(0f, duration, duration, start);
        }
    }
}