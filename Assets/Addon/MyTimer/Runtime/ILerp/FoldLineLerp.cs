using System;
using UnityEngine;

namespace MyTimer
{
    public class FoldLineLerp : ILerp<Vector3>
    {
        private Vector3[] points;
        private float[] ps;     //到达各点的时的进度百分比

        public void Initialize(Vector3[] points, float length)
        {
            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("折线段至少应当有两个点");
            this.points = new Vector3[count];
            ps = new float[count];
            float sum = 0f;
            ps[0] = 0f;
            for (int i = 1; i < count; i++)
            {
                sum += (points[i] - points[i - 1]).magnitude;
                ps[i] = sum / length;
            }
            Array.Copy(points, this.points, count);
        }

        public Vector3 Value(Vector3 origin, Vector3 target, float percent, float time, float duration)
        {
            int i;
            for (i = 0; percent > ps[i] && i < ps.Length; i++) ;
            float t = (percent - ps[i - 1]) / (ps[i] - ps[i - 1]);
            return Vector3.Lerp(points[i - 1], points[i], t);
        }
    }
}