namespace Services
{
    public enum EEvent
    {
        /// <summary>
        /// 加载场景前，参数：即将加载的场景号
        /// </summary>
        BeforeLoadScene,
        /// <summary>
        /// 加载场景后（等待一帧以后），参数：刚加载好的场景号
        /// </summary>
        AfterLoadScene,
    }
}