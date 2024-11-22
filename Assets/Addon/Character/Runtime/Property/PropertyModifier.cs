namespace Character
{
    public enum EModifyTiming
    {
        DirectAdd,
        DirectMultiply,
        FinalAdd,
        FinalMultiply,
    }

    [System.Serializable]
    public class PropertyModifier
    {
        private CharacterProperty property;

        public float value;
        public FindPropertySO so;
        public EModifyTiming timing;

        public void Bind()
        {
            property = so.FindProperty();
        }
        public void Register()
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

        public void Unregister()
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
            property.Multiply(value);
        }
    }
}

