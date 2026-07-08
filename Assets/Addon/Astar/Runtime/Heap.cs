using System;
using System.Collections.Generic;

namespace AStar
{
    /// <summary>
    /// 能够通过元素的引用反查下标的堆，因为需要支持原地更新元素的位置
    /// </summary>
    public class Heap<T>
    {
        private const int DefaultCapacity = 10;

        private T[] datas;
        private readonly IComparer<T> comparer;
        private readonly Dictionary<T, int> indexDict;
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
            indexDict = new Dictionary<T, int>();
        }

        public bool Push(T value)
        {
            if (Count == datas.Length)
                ResizeItemStore(datas.Length * 2);

            datas[Count] = value;
            indexDict[value] = Count;
            Count++;
            int position = BubbleUp(Count - 1);

            return position == 0;
        }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            T result = datas[0];
            indexDict.Remove(result);
            if (Count == 1)
            {
                Count = 0;
                datas[0] = default;
            }
            else
            {
                Count--;
                Move(Count, 0);
                datas[Count] = default;
                BubbleDown();
            }
            return result;
        }

        public void Update(T value)
        {
            if (!indexDict.TryGetValue(value, out int index))
                return;    //不在堆里（比如已经被弹出过），忽略
            BubbleUp(index);
        }

        private void ResizeItemStore(int newSize)
        {
            if (newSize < Count || newSize < DefaultCapacity)
                return;

            T[] temp = new T[newSize];
            Array.Copy(datas, 0, temp, 0, Count);
            datas = temp;
        }

        public void Clear()
        {
            Count = 0;
            datas = new T[Capacity];
            indexDict.Clear();
        }

        /// <summary>把下标from处的元素搬到下标to（覆盖式移动，不是交换），并同步更新反向索引</summary>
        private void Move(int from, int to)
        {
            datas[to] = datas[from];
            indexDict[datas[to]] = to;
        }

        /// <summary>交换两个下标处的元素，并同步更新反向索引</summary>
        private void Swap(int a, int b)
        {
            (datas[a], datas[b]) = (datas[b], datas[a]);
            indexDict[datas[a]] = a;
            indexDict[datas[b]] = b;
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

                Swap(bestChild, parent);
                parent = bestChild;
                leftChild = (parent * 2) + 1;
            }
        }

        private int BubbleUp(int startIndex)
        {
            while (startIndex > 0)
            {
                int parent = (startIndex - 1) / 2;
                if (comparer.Compare(datas[startIndex], datas[parent]) >= 0)
                    break;

                Swap(startIndex, parent);
                startIndex = parent;
            }
            return startIndex;
        }
    }
}
