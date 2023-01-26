using UnityEngine.Events;

namespace Services.Performance
{
    public class PerformanceManagerBase : Service, IPerformanceManager
    {
        public UnityAction<EDollType> UseDoll => _UseDoll;
        public UnityAction<EDollType> StopUseDoll => _StopUseDoll;

        private UnityAction<EDollType> _UseDoll;
        private UnityAction<EDollType> _StopUseDoll;
    }
}