using UnityEngine;

namespace MyTimer
{
    public class SineWave : Repeataion<float, Sine>
    {
        public float Amplitude => (Target - Origin) / 2;
        public float Angle
        {
            get => Percent * 360f;
            set
            {
                value %= 360f;
                if (value < 0f)
                    value += 360f;
                Time = Duration / 360f * value;
            }

        }
        public float Phase => Angle * Mathf.Deg2Rad;
    }
}