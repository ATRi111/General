namespace MyTimer
{
    public class SineWave : Repetition<float, Sine>
    {
        public float Angle
        {
            get => Percent * 360f;
            set
            {
                value %= 360f;
                if (value < 0f)
                    value += 360f;
                time = Duration / 360f * value;
            }

        }

        public new void Initialize(float minValue, float maxValue, float duration, bool start = true)
        {
            (Lerp as Sine).amplitude = (maxValue - minValue) / 2;
            float origin = (maxValue + minValue) / 2;
            base.Initialize(origin, origin, duration, start);
        }
    }
}