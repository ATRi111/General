using UnityEngine.Events;

namespace Services
{
    public interface IGlobalGameCycle : IService
    {
        void AttachToGameCycle(EInvokeMode mode, UnityAction callBack);
        void RemoveFromGameCycle(EInvokeMode mode, UnityAction callBack);
    }

    public enum EInvokeMode
    {
        /// <summary>
        /// 每次FixedUpdate调用
        /// </summary>
        FixedUpdate,
        /// <summary>
        /// 下次FixedUpdate调用
        /// </summary>
        NextFixedUpdate,
        /// <summary>
        /// 每次Update调用
        /// </summary>
        Update,
        /// <summary>
        /// 下次Update调用
        /// </summary>
        NextUpdate,
        /// <summary>
        /// 每次LateUpdate调用
        /// </summary>
        LateUpdate,
        /// <summary>
        /// 下次LateUpdate调用
        /// </summary>
        NextLateUpdate,
    }
}