using System;
using UnityEngine;

namespace MyTimer
{
    public class BezierLerp : ILerp<Vector3>
    {
        private Vector3[] points;
        private Vector3[] copy;     //计算时修改copy,不修改points

        public void Initialize(Vector3[] points)
        {
            int length = points.Length;
            if (length < 2)
                throw new ArgumentException("Bezier曲线至少应当有两个点");
            this.points = new Vector3[length];
            copy = new Vector3[length];
            Array.Copy(points, this.points, length);
        }

        public Vector3 Value(Vector3 origin, Vector3 target, float percent, float time, float duration)
        {
            int length = points.Length;
            Array.Copy(points, copy, length);
            for (int i = length - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    copy[j] = Vector3.Lerp(copy[j], copy[j + 1], percent);
                }
            }
            return copy[0];
        }
    }
}