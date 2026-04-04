using System.Collections.Generic;

namespace Character
{
    //按游戏实际需求修改此枚举，此枚举中的顺序即表示结算顺序
    public enum EModifierBucket
    {
        DirectAdd,
        DirectMultiply,
        FinalAdd,
        FinalMultiply,
    }

    public enum EModifierType
    {
        Add,
        Multiply,
    }

    //表示一个乘区或加区，同一乘区内的所有增幅直接相加
    [System.Serializable]
    public class ModifierBucket
    {
        static Dictionary<EModifierBucket, EModifierType> bucketToType = new();
        public static EModifierType BucketToType(EModifierBucket bucket)
            => bucketToType[bucket];

        static ModifierBucket()
        {
            bucketToType[EModifierBucket.DirectAdd] = EModifierType.Add;
            bucketToType[EModifierBucket.DirectMultiply] = EModifierType.Multiply;
            bucketToType[EModifierBucket.FinalAdd] = EModifierType.Add;
            bucketToType[EModifierBucket.FinalMultiply] = EModifierType.Multiply;
        }

        public EModifierBucket bucketID;
        public List<PropertyModifier> modifiers = new();
        public readonly EModifierType modifierType;

        public ModifierBucket(EModifierBucket bucketID)
        {
            this.bucketID = bucketID;
            modifierType = BucketToType(bucketID);
        }

        public void Register(PropertyModifier modifier)
        {
            modifiers.Add(modifier);
        }
        public void Unregister(PropertyModifier modifier)
        {
            modifiers.Remove(modifier);
        }
        public void Clear()
        {
            modifiers.Clear();
        }

        public void Apply(CharacterProperty property)
        {
            float totalValue = 0;
            for (int i = 0; i < modifiers.Count; i++)
            {
                totalValue += modifiers[i].value;
            }
            switch (modifierType)
            {
                case EModifierType.Add:
                    property.Add(totalValue);
                    break;
                case EModifierType.Multiply:
                    property.Multiply(1f + totalValue);
                    break;
            }
        }
    }
}

