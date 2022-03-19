using UnityEngine;

[System.Serializable]
/// <summary>
/// ����һ����ʱ���ƽ������ı仯
/// </summary>
public class MyTimer<T>
{
    private readonly GameCycle gameCycle;

    [SerializeField]
    private bool paused = true;
    /// <summary>
    /// �Ƿ���ͣ��������ʱĬ����ͣ,����Timerǰ��һ��Ҫ������ͣ
    /// </summary>
    public bool Paused
    {
        get => paused;
        set
        {
            if (paused != value)
            {
                if (value)
                    gameCycle.RemoveFromGameCycle(EUpdateMode.Update, OnUpdate);
                else
                    gameCycle.AttachToGameCycle(EUpdateMode.Update, OnUpdate);
                paused = value;
            }
        }
    }

    [SerializeField]
    private bool completed;
    /// <summary>
    /// �Ƿ����
    /// </summary>
    public bool Completed
    {
        get => completed;
        protected set
        {
            if(completed != value)
            {
                if (value)
                {
                    JustCompleted = true;
                    gameCycle.AttachToGameCycle(EUpdateMode.NextLateUpdate, OnComplete);
                }
                completed = value;
            }
        }
    }
    /// <summary>
    /// �Ƿ�����һ��Update֮����һ��LateUpdate֮ǰ���
    /// </summary>
    public bool JustCompleted { get; protected set; }

    /// <summary>
    /// ������ʱ��
    /// </summary>
    public float Timer { get; protected set; }
    /// <summary>
    /// ����İٷֱȣ�0��1)
    /// </summary>
    public float Percent => Mathf.Clamp(Timer / Duration, 0f, 1f); 
    /// <summary>
    /// ��ʱ��
    /// </summary>
    public float Duration { get; protected set; }
    /// <summary>
    /// ��ֵ
    /// </summary>
    public T Origin { get; protected set; }
    /// <summary>
    /// ��ֵ
    /// </summary>
    public T Target { get; protected set; }
    /// <summary>
    /// ��ǰֵ
    /// </summary>
    public virtual T Current => default;

    public MyTimer()
    {
        gameCycle = Service.Get<GameCycle>();
        paused = true;
        completed = false;
    }

    /// <summary>
    /// ΪMyTimer���ó�ʼ���Լ��Ƿ�������Ĭ��������
    /// </summary>
    public virtual void Initialize(T origin, T target, float duration, bool start = true)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
        Paused = !start;
    }

    protected virtual void OnUpdate()
    {
        Timer += Time.deltaTime;
        if(Timer >= Duration)
        {
            Completed = true;
            Paused = true;
        }
    }

    private void OnComplete()
    {
        JustCompleted = false;
    }

    /// <summary>
    /// ���¿�ʼ��ʱ
    /// </summary>
    public virtual void Restart()
    {
        Paused = false;
        Timer = 0;
        Completed = false;
    }

    /// <summary>
    /// ǿ�ƿ�������
    /// </summary>
    public void Complete()
    {
        Timer = Duration;
        Paused = true;
        Completed = true;
    }
}

/// <summary>
/// �����������仯
/// </summary>
public class Circulation<T> : MyTimer<T> where T : struct
{
    protected override void OnUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer >= Duration)
        {
            Timer -= Duration;
            T temp = Origin;
            Origin = Target;
            Target = temp;
        }
    }
}

/// <summary>
/// �����ķ����仯
/// </summary>
public class Repeataion<T> : MyTimer<T> where T : struct
{
    protected override void OnUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer >= Duration)
        {
            Timer -= Duration;
        }
    }
}

