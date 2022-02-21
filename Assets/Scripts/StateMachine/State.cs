using UnityEngine;

namespace StateMathine
{
    public class State
    {
        internal virtual void OnEnter(EState prev) { }
        internal virtual void OnUpdate() { }
        internal virtual void OnFixedUpdate() { }
        internal virtual void OnExit(EState next) { }
    }
}

