namespace MyStateMachine
{
    public class State
    {
        protected GameCycle gameCycle;
        protected internal StateMachine stateMachine;
        protected internal int enumIndex;

        public State()
        {
            gameCycle = GameCycle.Instance;
            enumIndex = 0;
        }

        public State(int index)
        {
            gameCycle = GameCycle.Instance;
            enumIndex = index;
        }

        /// <summary>
        /// 进入状态时
        /// </summary>
        /// <param name="enumIndex">上一个状态</param>
        protected internal virtual void OnEnter(int enumIndex)
        {
            gameCycle.AttachToGameCycle(EInvokeMode.Update, Update);
            gameCycle.AttachToGameCycle(EInvokeMode.FixedUpdate, FixedUpdate);
            gameCycle.AttachToGameCycle(EInvokeMode.LateUpdate, LateUpdate);
        }

        /// <summary>
        /// 离开状态时
        /// </summary>
        /// <param name="enumIndex">下一个状态</param>
        protected internal virtual void OnExit(int enumIndex)
        {
            gameCycle.RemoveFromGameCycle(EInvokeMode.Update, Update);
            gameCycle.RemoveFromGameCycle(EInvokeMode.FixedUpdate, FixedUpdate);
            gameCycle.RemoveFromGameCycle(EInvokeMode.LateUpdate, LateUpdate);
        }

        /// <summary>
        /// 处于此状态时，每帧自动调用
        /// </summary>
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }
        protected virtual void LateUpdate() { }
    }
}