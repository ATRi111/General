using UnityEngine.Events;

namespace Services.Performance
{
    public interface IPerformanceManager : IService
    {
        UnityAction<EDollType> StopUseDoll { get; }
        UnityAction<EDollType> UseDoll { get; }
    }
}