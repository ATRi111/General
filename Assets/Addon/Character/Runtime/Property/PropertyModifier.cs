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

