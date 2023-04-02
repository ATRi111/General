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
        /// 启动对话，参数：对话列表
        /// </summary>
        StartDialog,
        /// <summary>
        /// 启动世界空间对话，参数：对话列表
        /// </summary>
        StartWorldSpaceDialog,
    }
}