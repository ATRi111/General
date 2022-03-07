using UnityEngine;
using System;

/// <summary>
/// ����һ����ʱ���ƽ������ı仯
/// </summary>
public class Transformation<T>
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
    public bool Completed => Timer >= Duration;
    public Transformation() { }
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

public class ColorChange : Transformation<Color>
{
    public override Color Current => Color.Lerp(Origin, Target, Timer / Duration);
    public ColorChange() { }
    public ColorChange(Color origin, Color target, float duration) : base(origin, target, duration) { }
}

public class BezierCurve : Transformation<Vector3>
{
    private readonly Vector3[] Points;
    public BezierCurve(Vector3[] points, float duration)
    {
        int count = points.Length;
        if (count < 2)
            throw new ArgumentException();
        Origin = points[0];
        Target = points[count - 1];
        Points = points;
        Duration = duration;
    }

    public override Vector3 Current => MathTool.BezierLerp(Points, Timer / Duration);
}