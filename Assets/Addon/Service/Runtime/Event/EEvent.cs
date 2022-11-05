namespace Services
{
    public enum EEvent
    {
        /// <summary>
        /// 加载场景前，参数：即将加载的场景号
        /// </summary>
        BeforeLoadScene,
        /// <summary>
        /// 加载场景后（至少一帧以后），参数：刚加载好的场景号
        /// </summary>
        AfterLoadScene,
        /// <summary>
        /// 游戏状态改变，参数：游戏状态
        /// </summary>
        GameStateChange,
        /// <summary>
        /// 发出信号，参数：信号种类
        /// </summary>
        Signal,
        /// <summary>
        /// 开始对话，参数：对话图标
        /// </summary>
        StartDialog,
        /// <summary>
        /// 结束对话
        /// </summary>
        EndDialog,
        /// <summary>
        /// QTE开始或结束，参数：是否是QTE开始
        /// </summary>
        QTE,
        /// <summary>
        /// 需要或不再需要输入信号，参数：是否需要输入信号
        /// </summary>
        RequireSignal,
    }
}