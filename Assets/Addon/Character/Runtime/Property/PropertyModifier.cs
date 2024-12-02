namespace Character
{
    public enum EModifyTiming
    {
        DirectAdd,
        DirectMultiply,
        FinalAdd,
        FinalMultiply,
    }

    /// <summary>
    /// Ӱ�����ԵĴ���
    /// </summary>
    [System.Serializable]
    public class PropertyModifier
    {
        /// <summary>
        /// �仯��
        /// </summary>
        public float value;
        /// <summary>
        /// ��ȡ�������õķ���
        /// </summary>
        public FindPropertySO so;
        /// <summary>
        /// �仯��ʽ
        /// </summary>
        public EModifyTiming timing;

        public void Register(CharacterProperty property)
        {
            switch (timing)
            {
                case EModifyTiming.DirectAdd:
                    property.DirectAdd += Add;
                    break;
                case EModifyTiming.DirectMultiply:
                    property.DirectMultiply += Multiply;
                    break;
                case EModifyTiming.FinalAdd:
                    property.FinalAdd += Add;
                    break;
                case EModifyTiming.FinalMultiply:
                    property.FinalMultiply += Multiply;
                    break;
            }

        }

        public void Unregister(CharacterProperty property)
        {
            switch (timing)
            {
                case EModifyTiming.DirectAdd:
                    property.DirectAdd -= Add;
                    break;
                case EModifyTiming.DirectMultiply:
                    property.DirectMultiply -= Multiply;
                    break;
                case EModifyTiming.FinalAdd:
                    property.FinalAdd -= Add;
                    break;
                case EModifyTiming.FinalMultiply:
                    property.FinalMultiply -= Multiply;
                    break;
            }
        }

        private void Add(CharacterProperty property)
        {
            property.Add(value);
        }

        private void Multiply(CharacterProperty property)
        {
            property.Multiply(1f + value);
        }
    }
}

