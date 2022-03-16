using UnityEngine;

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
