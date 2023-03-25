namespace Services
{
    public enum EEvent
    {
        /// <summary>
        /// ���س���ǰ���������������صĳ�����
        /// </summary>
        BeforeLoadScene,
        /// <summary>
        /// ���س���������һ֡�Ժ󣩣��������ռ��غõĳ�����
        /// </summary>
        AfterLoadScene,
        /// <summary>
        /// ��Ϸ״̬�ı䣬��������Ϸ״̬
        /// </summary>
        GameStateChange,
        /// <summary>
        /// �����źţ��������ź�����
        /// </summary>
        Signal,
        /// <summary>
        /// ��ʼ�Ի����������Ի�ͼ��
        /// </summary>
        StartDialog,
        /// <summary>
        /// �����Ի�
        /// </summary>
        EndDialog,
        /// <summary>
        /// QTE��ʼ��������������Ƿ���QTE��ʼ
        /// </summary>
        QTE,
        /// <summary>
        /// ��Ҫ������Ҫ�����źţ��������Ƿ���Ҫ�����ź�
        /// </summary>
        RequireSignal,
    }
}