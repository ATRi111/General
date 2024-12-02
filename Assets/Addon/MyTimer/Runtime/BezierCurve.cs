using UnityEngine;

namespace MyTimer
{
    public class BezierCurve : Timer<Vector3, BezierLerp>
    {
        public void Initialize(Vector3[] points, float duration, bool start = true)
        {
            base.Initialize(points[0], points[^1], duration, start);
            (Lerp as BezierLerp).Initialize(points);
        }
    }
}