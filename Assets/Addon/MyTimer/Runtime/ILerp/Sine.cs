using UnityEngine;

namespace MyTimer
{
    public class Sine : ILerp<float>
    {
        public float amplitude;

        public float Value(float origin, float target, float percent, float time, float duration)
        {
            float phase = percent * 2f * Mathf.PI;
            return amplitude * Mathf.Sin(phase) + origin;
        }
    }
}