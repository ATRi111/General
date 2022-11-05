using UnityEngine;

namespace MyTimer
{
    public class QuaternionLerp : ILerp<Quaternion>
    {
        public Quaternion Value(Quaternion origin, Quaternion target, float percent, float time, float duration)
        {
            return Quaternion.Lerp(origin, target, duration);
        }
    }
}