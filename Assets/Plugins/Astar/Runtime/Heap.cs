using System;
using System.Collections.Generic;

namespace AStar
{
    //在Comparer中使用x-y时为最小堆
    public class Heap<T>
    {
        private readonly IComparer<T> comparer;
        private T[] datas;

        public int Count { get; private set; }
        public bool IsEmpty => Count == 0;

        public Heap(int capacity, IComparer<T> _comparer = null)
        {
            comparer = _comparer ?? Comparer<T>.Default;
            datas = new T[capacity];
        }

        public void Push(T data)
        {
            if (Count >= datas.Length)
                Array.Resize(ref datas, Count * 2);
            datas[Count] = data;
            MoveUp(Count++);
        }
        public T Pop()
        {
            T ret = Top();
            datas[0] = datas[--Count];
            if (Count > 0)
                MoveDown(0);
            return ret;
        }
        public T Top()
        {
            if (Count <= 0)
                throw new InvalidOperationException();
            return datas[0];
        }

        //上滤
        private void MoveUp(int index)
        {
            T v = datas[index];
            for (int n2 = index >> 1; index > 0 && comparer.Compare(v, datas[n2]) < 0; index = n2, n2 >>= 1)
                datas[index] = datas[n2];
            datas[index] = v;
        }
        //下滤
        private void MoveDown(int index)
        {
            T v = datas[index];
            for (int n2 = index << 1; n2 < Count; index = n2, n2 <<= 1)
            {
                if (n2 + 1 < Count && comparer.Compare(datas[n2 + 1], datas[n2]) < 0)
                    n2++;
                if (comparer.Compare(v, datas[n2]) <= 0)
                    break;
                datas[index] = datas[n2];
            }
            datas[index] = v;
        }
    }
}