using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyTimer
{
    public enum TickGroup
    {
        FixedUpdate,
        Update,
        LateUpdate,
    }

    public class GameCycle : MonoBehaviour
    {
        private static readonly TickGroup[] tickGroups = (TickGroup[])System.Enum.GetValues(typeof(TickGroup));
        private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private static GameCycle instance;

        internal static GameCycle Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameCycleForTimer");
                    instance = obj.AddComponent<GameCycle>();
                }
                return instance;
            }
        }

        private readonly Dictionary<TickGroup, UnityAction> cycle = new Dictionary<TickGroup, UnityAction>();
        private readonly Dictionary<TickGroup, UnityAction> temp = new Dictionary<TickGroup, UnityAction>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            foreach (TickGroup tickGroup in tickGroups)
            {
                cycle.Add(tickGroup, null);
                temp.Add(tickGroup, null);
            }
            StartCoroutine(DelayAttach());
        }

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
        }

        private void FixedUpdate()
        {
            cycle[TickGroup.FixedUpdate]?.Invoke();
        }

        private void LateUpdate()
        {
            cycle[TickGroup.LateUpdate]?.Invoke();
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