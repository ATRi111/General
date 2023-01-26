namespace Character
{
    /// <summary>
    /// һ������Ϊtrue���ᱣ����true��ֱ��������Ϊfalse
    /// </summary>
    public class HoldBool
    {
        private bool boolValue;
        /// <summary>
        /// ��set�����������falseֵ�ᱻ����
        /// </summary>
        public bool BoolValue
        {
            get => boolValue;
            set
            {
                if (value)
                    boolValue = true;
            }
        }

        public HoldBool(bool init)
        {
            boolValue = init;
        }

        /// <summary>
        /// ֻ��ͨ���˷������ܽ�boolValue��Ϊfalse
        /// </summary>
        public void Reset()
        {
            boolValue = false;
        }

        public void SetTrue()
        {
            BoolValue = true;
        }

        public static bool operator true(HoldBool holdBool) => holdBool.boolValue;
        public static bool operator false(HoldBool holdBool) => !holdBool.boolValue;
    }
}