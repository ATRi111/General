namespace AStar
{
    public enum ENodeState
    {
        /// <summary>
        /// δ����FCost�Ľڵ�
        /// </summary>
        Blank,
        /// <summary>
        /// �������������·���Ѿ�ȷ���Ľڵ�
        /// </summary>
        Close,
        /// <summary>
        /// �Ѽ���FCost�����·��δȷ���Ľڵ�
        /// </summary>
        Open,
    }
}