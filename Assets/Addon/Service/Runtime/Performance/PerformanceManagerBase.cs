using UnityEngine.Events;

namespace Services
{
    public class PerformanceManagerBase : Service
    {
        public UnityAction<EDollType> UseDoll;
        public UnityAction<EDollType> StopUseDoll;
    }
}