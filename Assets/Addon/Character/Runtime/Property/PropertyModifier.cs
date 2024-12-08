namespace Character
{
    public enum EModifyMethod
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
        /// �ṩ"��ȡ�������õķ���"��SO
        /// </summary>
        public FindPropertySO so;
        /// <summary>
        /// �仯��ʽ
        /// </summary>
        public EModifyMethod method;

        public void Register(CharacterProperty property)
        {
            switch (method)
            {
                case EModifyMethod.DirectAdd:
                    property.DirectAdd += Add;
                    break;
                case EModifyMethod.DirectMultiply:
                    property.DirectMultiply += Multiply;
                    break;
                case EModifyMethod.FinalAdd:
                    property.FinalAdd += Add;
                    break;
                case EModifyMethod.FinalMultiply:
                    property.FinalMultiply += Multiply;
                    break;
            }

        }

        public void Unregister(CharacterProperty property)
        {
            switch (method)
            {
                case EModifyMethod.DirectAdd:
                    property.DirectAdd -= Add;
                    break;
                case EModifyMethod.DirectMultiply:
                    property.DirectMultiply -= Multiply;
                    break;
                case EModifyMethod.FinalAdd:
                    property.FinalAdd -= Add;
                    break;
                case EModifyMethod.FinalMultiply:
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

