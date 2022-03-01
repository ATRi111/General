using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����һ����ʱ���ƽ������ı仯
/// </summary>
public class Transformation<T>
{
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
    public virtual Color Current => default;
    public bool Completed => Timer >= Duration;
    public Transformation() { }
    public Transformation(float duration, T origin, T target)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
    }
    protected virtual void OnFixedUpdate()
    {
        Timer += Time.fixedDeltaTime;
    }
}

public class ColorChange : Transformation<Color>
{
    public override Color Current => Color.Lerp(Origin, Target, Timer / Duration);
    public ColorChange() { }
    public ColorChange(float duration, Color origin, Color target) : base(duration, origin, target) { }
}