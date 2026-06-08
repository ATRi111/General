using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    [DefaultExecutionOrder(-100)]
    public class GlobalGameCycle : Service, IGlobalGameCycle
    {
        private static readonly TickGroup[] tickGroups = (TickGroup[])System.Enum.GetValues(typeof(TickGroup));
        private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private readonly Dictionary<TickGroup, UnityAction> cycle = new Dictionary<TickGroup, UnityAction>();
        private readonly Dictionary<TickGroup, UnityAction> temp = new Dictionary<TickGroup, UnityAction>();

        protected override void Awake()
        {
            base.Awake();
            foreach (TickGroup tickGroup in tickGroups)
            {
                cycle.Add(tickGroup, null);
                temp.Add(tickGroup, null);
            }
            StartCoroutine(DelayAttach());
        }

        /// <summary>
        /// 用于将非Monobehavior方法加入游戏循环，加入的方法下一帧开始才会被调用
        /// </summary>
        public void AttachToGameCycle(TickGroup tickGroup, UnityAction callBack)
        {
            temp[tickGroup] += callBack;
        }

        public void RemoveFromGameCycle(TickGroup tickGroup, UnityAction callBack)
        {
            cycle[tickGroup] -= callBack;
            temp[tickGroup] -= callBack;
        }

        private void Update()
        {
            cycle[TickGroup.Update]?.Invoke();
            cycle[TickGroup.NextUpdate]?.Invoke();
            cycle[TickGroup.NextUpdate] = null;
        }

        private void FixedUpdate()
        {
            cycle[TickGroup.FixedUpdate]?.Invoke();
            cycle[TickGroup.NextFixedUpdate]?.Invoke();
            cycle[TickGroup.NextFixedUpdate] = null;
        }

        private void LateUpdate()
        {
            cycle[TickGroup.LateUpdate]?.Invoke();
            cycle[TickGroup.NextLateUpdate]?.Invoke();
            cycle[TickGroup.NextLateUpdate] = null;
        }

        private IEnumerator DelayAttach()
        {
            for (; ; )
            {
                foreach (TickGroup tickGroup in tickGroups)
                {
                    cycle[tickGroup] += temp[tickGroup];
                    temp[tickGroup] = null;
                }
                yield return waitForEndOfFrame;
            }
        }
    }
}
