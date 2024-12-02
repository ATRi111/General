using UnityEngine;

namespace MyTimer
{
    public class UniformFoldLineMotion : Timer<Vector3, FoldLineLerp>
    {
        public void Initialize(Vector3[] points, float length, float duration, bool start = true)
        {
            base.Initialize(points[0], points[^1], duration, start);
            (Lerp as FoldLineLerp).Initialize(points, length);
        }
    }
}