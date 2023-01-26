using UnityEngine.Events;

namespace Services
{
    public interface IGlobalGameCycle : IService
    {
        void AttachToGameCycle(EInvokeMode mode, UnityAction callBack);
        void RemoveFromGameCycle(EInvokeMode mode, UnityAction callBack);
    }
}