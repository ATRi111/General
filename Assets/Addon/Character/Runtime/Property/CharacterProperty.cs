using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    /// <summary>
    /// ��ɫ���ԣ������ڱ�ʾ�ܶ�������Ӱ�죬�����仯����
    /// </summary>
    public class CharacterProperty
    {
        public float defaultValue;
        [SerializeField]
        protected float currentValue;
        public float CurrentValue => currentValue;
        public int IntValue => Mathf.RoundToInt(currentValue);

        public Action<CharacterProperty> DirectAdd;         //ֱ�Ӽ���
        public Action<CharacterProperty> DirectMultiply;    //ֱ�ӳ���
        public Action<CharacterProperty> FinalAdd;          //���ռ���
        public Action<CharacterProperty> FinalMultiply;     //���ճ���

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