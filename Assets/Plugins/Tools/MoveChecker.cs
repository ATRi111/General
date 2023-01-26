using System.Collections;
using UnityEngine;

namespace Tools
{
    public class MoveChecker
    {
        private MonoBehaviour mono;
        private readonly Transform m_transform;
        private Vector3 position_previous;

        public bool Moved { get; private set; }
        public Vector3 DeltaPosition { get; private set; }
        public Vector3 Direction => DeltaPosition.normalized;

        private Coroutine co;
        private bool active;
        public bool Active
        {
            get => active;
            set
            {
                if (active != value)
                {
                    if (value)
                        co = mono.StartCoroutine(AfterFixeddUpdate());
                    else
                        mono.StopCoroutine(co);
                    active = value;
                }
                if (value)
                    position_previous = m_transform.position;
                DeltaPosition = Vector3.zero;
            }
        }

        public Vector3 InstantaneousVelocity => DeltaPosition / Time.fixedDeltaTime;

        public MoveChecker(MonoBehaviour mono)
        {
            this.mono = mono;
            m_transform = mono.transform;
            Active = true;
        }

        private IEnumerator AfterFixeddUpdate()
        {
            for (; ; )
            {
                Moved = position_previous != m_transform.position;
                DeltaPosition = m_transform.position - position_previous;
                position_previous = m_transform.position;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}