using System;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// ��ɫ���ԣ������ڱ�ʾ�ܶ�������Ӱ�죬�����仯����
    /// </summary>
    public abstract class Property<T> where T : struct
    {
        public T defaultValue;
        [SerializeField]
        protected T currentValue;
        public T CurrentValue => currentValue;

        public Action<Property<T>> DirectAdd;         //ֱ�Ӽ���
        public Action<Property<T>> DirectMultiply;    //ֱ�ӳ���
        public Action<Property<T>> FinalAdd;          //���ռ���
        public Action<Property<T>> FinalMultiply;     //���ճ���
        public Action<Property<T>> FinalClamp;        //����ȡֵ��Χ

        public abstract void Add(T value);
        public abstract void Multiply(T value);
        public abstract void Clamp(T min, T max);

        public void Refresh()
        {
            currentValue = defaultValue;
            DirectAdd?.Invoke(this);
            DirectMultiply?.Invoke(this);
            FinalAdd?.Invoke(this);
            FinalMultiply?.Invoke(this);
            FinalClamp?.Invoke(this);
        }
    }

    [Serializable]
    public sealed class IntProperty : Property<int>
    {
        public override void Add(int value)
        {
            currentValue += value;
        }

        public override void Multiply(int value)
        {
            currentValue *= value;
        }

        public override void Clamp(int min,int max)
        {
            currentValue = Mathf.Clamp(currentValue, min, max);
        }
    }

    [Serializable]
    public sealed class FloatProperty : Property<float>
    {
        public override void Add(float value)
        {
            currentValue += value;
        }

        public override void Multiply(float value)
        {
            currentValue *= value;
        }

        public override void Clamp(float min, float max)
        {
            currentValue = Mathf.Clamp(currentValue, min, max);
        }
    }
}