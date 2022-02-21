using UnityEngine;

namespace StateMathine
{
    public class StateMachineBase : MonoBehaviour
    {
        private StateManager stateManager;
        public StateMachineBase(EState init)
        {
            stateManager = ServiceLocator.Instance.GetService<StateManager>(EService.StateManager);
            current = init;
            state = stateManager.GetState(init);
        }

        private EState current;
        private State state;

        public void SetState(EState next)
        {
            State nextState = stateManager.GetState(next);
            state.OnExit(next);
            nextState.OnEnter(current);
            current = next;
        }

        private void Update()
        {
            state.OnUpdate();
        }

        private void FixedUpdate()
        {
            state.OnFixedUpdate();
        }
    }
}

