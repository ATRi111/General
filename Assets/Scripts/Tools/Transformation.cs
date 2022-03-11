using UnityEngine;

/// <summary>
/// 描述一段随时间推进发生的变化
/// </summary>
public class Transformation<T> where T : struct
{
    /// <summary>
    /// 暂停时，OnFixedUpdate无行为
    /// </summary>
    public bool Paused { get; set; }
    /// <summary>
    /// 经过的时间
    /// </summary>
    public float Timer { get; set; }
    /// <summary>
    /// 总时间
    /// </summary>
    public float Duration { get; protected set; }
    /// <summary>
    /// 初值
    /// </summary>
    public T Origin { get; protected set; }
    /// <summary>
    /// 终值
    /// </summary>
    public T Target { get; protected set; }
    /// <summary>
    /// 当前值
    /// </summary>
    public virtual T Current => default;
    public virtual bool Completed => Timer >= Duration;
    public Transformation(T origin, T target, float duration)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
    }
    //创建类实例后，在fiexedupdate中调用此方法
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
/// 基本的往复变化
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
/// 基本的反复变化
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
