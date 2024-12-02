using System;
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

        public Action<CharacterProperty> DirectAdd;         //直接加算
        public Action<CharacterProperty> DirectMultiply;    //直接乘算
        public Action<CharacterProperty> FinalAdd;          //最终加算
        public Action<CharacterProperty> FinalMultiply;     //最终乘算

        public void Refresh()
        {
            currentValue = defaultValue;
            DirectAdd?.Invoke(this);
            DirectMultiply?.Invoke(this);
            FinalAdd?.Invoke(this);
            FinalMultiply?.Invoke(this);
        }

        public void Clear()
        {
            DirectAdd = null;
            DirectMultiply = null;
            FinalAdd = null;
            FinalMultiply = null;
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