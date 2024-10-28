using System;
using System.Collections.Generic;

namespace AStar
{
    /// <summary>
    /// 在comparer中使用x-y时为最小堆
    /// </summary>
    public class Heap<T>
    {
        private const int DefaultCapacity = 10;

        private T[] datas;
        private readonly IComparer<T> comparer;
        public int Count { get; private set; }
        public int Capacity => datas.Length;
        public bool IsEmpty => Count == 0;
        public bool IsFull => Count == Capacity;

        public Heap(int capacity = DefaultCapacity, IComparer<T> comparer = null)
        {
            if (capacity < 0)
                throw new IndexOutOfRangeException();
            datas = new T[capacity];
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public bool Push(T value)
        {
            if (Count == datas.Length)
                ResizeItemStore(datas.Length * 2);

            datas[Count++] = value;
            int position = BubbleUp(Count - 1);

            return position == 0;
        }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            T result = datas[0];
            if (Count == 1)
            {
                Count = 0;
                datas[0] = default;
            }
            else
            {
                Count--;
                datas[0] = datas[Count];
                datas[Count] = default;
                BubbleDown();
            }
            return result;
        }

        private void ResizeItemStore(int newSize)
        {
            if (Count < newSize || DefaultCapacity <= newSize)
                return;

            T[] temp = new T[newSize];
            Array.Copy(datas, 0, temp, 0, Count);
            datas = temp;
        }

        public void Clear()
        {
            Count = 0;
            datas = new T[Capacity];
        }

        private void BubbleDown()
        {
            int parent = 0;
            int leftChild = (parent * 2) + 1;
            while (leftChild < Count)
            {
                int rightChild = leftChild + 1;
                int bestChild = (rightChild < Count && comparer.Compare(datas[rightChild], datas[leftChild]) < 0) ?
                    rightChild : leftChild;

                if (comparer.Compare(datas[bestChild], datas[parent]) >= 0)
                    break;

                (datas[bestChild], datas[parent]) = (datas[parent], datas[bestChild]);
                parent = bestChild;
                leftChild = (parent * 2) + 1;
            }
        }

        private int BubbleUp(int startIndex)
        {
            while (startIndex > 0)
            {
                int parent = (startIndex - 1) / 2;
                if(comparer.Compare(datas[startIndex], datas[parent]) >= 0)
                    break;

                (datas[parent], datas[startIndex]) = (datas[startIndex], datas[parent]);
                startIndex = parent;
            }
            return startIndex;
        }
    }
}
