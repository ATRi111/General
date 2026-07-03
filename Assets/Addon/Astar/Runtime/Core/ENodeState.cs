namespace AStar
{
    public enum ENodeState
    {
        /// <summary>
        /// 非备选也未被选中过的节点
        /// </summary>
        Blank,
        /// <summary>
        /// 被选中过的节点
        /// </summary>
        Close,
        /// <summary>
        /// 下一步的备选节点
        /// </summary>
        Open,
    }
}