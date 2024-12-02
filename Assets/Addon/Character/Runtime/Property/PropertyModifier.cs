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
    /// 影响属性的词条
    /// </summary>
    [System.Serializable]
    public class PropertyModifier
    {
        /// <summary>
        /// 变化量
        /// </summary>
        public float value;
        /// <summary>
        /// 获取属性引用的方法
        /// </summary>
        public FindPropertySO so;
        /// <summary>
        /// 变化方式
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

