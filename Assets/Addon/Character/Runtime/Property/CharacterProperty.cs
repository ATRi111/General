using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [Serializable]
    /// <summary>
    /// 角色属性，适用于表示受多种因素影响，经常变化的量
    /// </summary>
    public class CharacterProperty
    {
        public float defaultValue;
        [SerializeField]
        protected float currentValue;
        public float CurrentValue => currentValue;
        public int IntValue => Mathf.RoundToInt(currentValue);
        [SerializeField]
        private List<ModifierBucket> modifierBuckets = new();

        public CharacterProperty()
        {
            for (int i = 0; i < Enum.GetValues(typeof(EModifierBucket)).Length; i++)
            {
                modifierBuckets.Add(new ModifierBucket((EModifierBucket)i));
            }
        }

        public void Register(PropertyModifier modifier)
        {
            modifierBuckets[(int)modifier.bucket].Register(modifier);
        }
        public void Unregister(PropertyModifier modifier)
        {
            modifierBuckets[(int)modifier.bucket].Unregister(modifier);
        }
        public void Clear()
        {
            for (int i = 0; i < modifierBuckets.Count; i++)
            {
                modifierBuckets[i].Clear();
            }
        }

        public void Refresh()
        {
            currentValue = defaultValue;
            for (int i = 0; i < modifierBuckets.Count; i++)
            {
                modifierBuckets[i].Apply(this);
            }
        }

        public void Add(float value)
        {
            currentValue += value;
        }

        public void Multiply(float value)
        {
            currentValue *= value;
        }
    }
}