namespace AStar
{
    public enum ENodeState
    {
        /// <summary>
        /// 未计算FCost的节点
        /// </summary>
        Blank,
        /// <summary>
        /// 从起点出发的最短路径已经确定的节点
        /// </summary>
        Close,
        /// <summary>
        /// 已计算FCost而最短路径未确定的节点
        /// </summary>
        Open,
    }
}