namespace AStar
{
    public enum ENodeType
    {
        /// <summary>
        /// δ���ʹ��ķ���
        /// </summary>
        Blank,
        /// <summary>
        /// ·��ȷ��������һ�εķ���
        /// </summary>
        Close,
        /// <summary>
        /// ·����ȷ���ķ���
        /// </summary>
        Open,
        /// <summary>
        /// ����·���еĵ�
        /// </summary>
        Route,
        /// <summary>
        /// �޷�ͨ�нڵ�
        /// </summary>
        Obstacle,
    }
}