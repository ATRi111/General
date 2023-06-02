namespace AStar
{
    public enum ENodeType
    {
        /// <summary>
        /// 未访问过的方块
        /// </summary>
        Blank,
        /// <summary>
        /// 路径确定过至少一次的方块
        /// </summary>
        Close,
        /// <summary>
        /// 路径待确定的方块
        /// </summary>
        Open,
        /// <summary>
        /// 最终路径中的点
        /// </summary>
        Route,
        /// <summary>
        /// 无法通行节点
        /// </summary>
        Obstacle,
    }
}