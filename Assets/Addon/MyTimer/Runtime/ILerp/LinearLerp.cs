using UnityEngine;

namespace MyTimer
{
    public class FloatLerp : ILerp<float>
    {
        public float Value(float origin, float target, float percent, float time, float duration)
        {
            return Mathf.Lerp(origin, target, percent);
        }
    }
}
