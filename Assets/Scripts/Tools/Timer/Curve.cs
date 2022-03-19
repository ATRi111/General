using UnityEngine;

public class BezierCurve : MyTimer<Vector3>
{
    private Vector3[] Points;
    public void Initialize(Vector3[] points, float duration, bool start = true)
    {
        base.Initialize(points[0], points[points.Length - 1], duration, start);
        Points = points;
    }

    public override Vector3 Current => MathTool.BezierLerp(Points, Percent);
}
