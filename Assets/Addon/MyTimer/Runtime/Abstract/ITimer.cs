namespace MyTimer
{
    public interface ITimer
    {
        bool Completed { get; }
        float Duration { get; }
        bool Paused { get; set; }
        float Percent { get; }
        float Time { get; }

        void ForceComplete();
        void Restart(bool fixedTime = false);
    }
}