namespace Services.Event
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
        /// <summary>
        /// 玩家开始控制某个角色，参数：所控制的角色的Brain
        /// </summary>
        OnHumanControl,
        /// <summary>
        /// 显示信息，参数：引发事件的对象，屏幕坐标，信息内容
        /// </summary>
        ShowMessage,
        /// <summary>
        /// 隐藏信息，参数：引发事件的对象
        /// </summary>
        HideMessage,
        /// <summary>
        /// 战斗开始前
        /// </summary>
        BeforeBattle,
        /// <summary>
        /// 战斗结束后
        /// </summary>
        AfterBattle,
        /// <summary>
        /// 时间变动，参数：当前全局时间
        /// </summary>
        OnTick,
    }
}