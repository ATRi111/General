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
        /// 卸载场景前，参数：即将卸载的场景号
        /// </summary>
        BeforeUnLoadScene,
        /// <summary>
        /// 卸载场景后（至少一帧以后），参数：刚卸载完的场景号
        /// </summary>
        AfterUnLoadScene,
    }
}