using UnityEngine;

/// <summary>
/// ����һ����ʱ���ƽ������ı仯
/// </summary>
public class Transformation<T> where T : struct
{
    /// <summary>
    /// ��ͣʱ��OnFixedUpdate����Ϊ
    /// </summary>
    public bool Paused { get; set; }
    /// <summary>
    /// ������ʱ��
    /// </summary>
    public float Timer { get; set; }
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
    public virtual bool Completed => Timer >= Duration;
    public Transformation(T origin, T target, float duration)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
    }
    //������ʵ������fiexedupdate�е��ô˷���
    public virtual void OnFixedUpdate()
    {
        if (Paused)
            return;
        Timer += Time.fixedDeltaTime;
        if (Completed)
            Paused = true;
    }

    public virtual void Restart()
    {
        Paused = false;
        Timer = 0;
    }
}
/// <summary>
/// �����������仯
/// </summary>
/// <typeparam name="T"></typeparam>
public class Circulation<T> : Transformation<T> where T : struct
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (Completed)
        {
            Timer -= Duration;
            T temp = Origin;
            Origin = Target;
            Target = temp;
            Paused = false;
        }
    }

    public Circulation(T origin, T target, float duration) : base(origin, target, duration) { }
}
/// <summary>
/// �����ķ����仯
/// </summary>
/// <typeparam name="T"></typeparam>
public class Repeataion<T> : Transformation<T> where T : struct
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (Completed)
        {
            Timer -= Duration;
            Paused = false;
        }
    }
    public Repeataion(T origin, T target, float duration) : base(origin, target, duration) { }
}

public class BezierCurve : Transformation<Vector3>
{
    private readonly Vector3[] Points;
    public BezierCurve(Vector3[] points, float duration) : base(points[0], points[points.Length - 1], duration)
    {
        int count = points.Length;
        Points = points;
    }

    public override Vector3 Current => MathTool.BezierLerp(Points, Timer / Duration);
}
