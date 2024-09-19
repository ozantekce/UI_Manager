using System;
using System.Collections;
using System.Collections.Generic;

namespace UIManager
{

    public class Heap<T> : IEnumerable<T> where T : IComparable<T>
    {
        private const int INIT_CAPACITY = 11;
        private T[] heap;
        private int CAPACITY;
        private int size;


        public Heap(int capacity)
        {
            this.CAPACITY = capacity;
            heap = new T[capacity];
        }

        public Heap()
        {
            this.CAPACITY = INIT_CAPACITY;
            heap = new T[CAPACITY];
        }

        public void Insert(T element)
        {

            if (size < CAPACITY)
            {
                heap[size] = element;

                int current = size;
                size++;
                while (heap[current].CompareTo(heap[GetParentIndex(current)]) < 0)
                {
                    Swap(heap, current, GetParentIndex(current));
                    current = GetParentIndex(current);
                }
            }
            else
            {
                // GROW
                Grow(CAPACITY + 1);
                Insert(element);
            }

        }

        public T Remove()
        {

            if (!IsEmpty())
            {
                size--;
                return ExtractMin(heap, size); ;
            }
            else
            {
                throw new Exception("Heap is empty");
                //throw new RuntimeException("Heap is empty");
            }

        }


        public T Peek()
        {
            return heap[0];
        }


        private T ExtractMin(T[] heap, int n)
        {
            T min = heap[0];
            heap[0] = heap[n];
            n = n - 1;
            Heapify(heap, 0, n);
            return min;
        }


        private void Heapify(T[] heap, int i, int n)
        {

            int smallest = i;
            int leftChildIndex = GetLeftChildIndex(i);
            int rightChildIndex = GetRightChildIndex(i);

            if (leftChildIndex <= n
                && heap[leftChildIndex].CompareTo(heap[smallest]) < 0)
            {
                smallest = leftChildIndex;
            }
            //heap[GetRightChildIndex(i)].val < heap[smallest].val
            if (rightChildIndex <= n
                && heap[rightChildIndex].CompareTo(heap[smallest]) < 0)
            {
                smallest = rightChildIndex;
            }

            if (smallest != i)
            {
                Swap(heap, i, smallest);
                Heapify(heap, smallest, n);
            }

        }


        private void Swap(T[] heap, int i1, int i2)
        {
            T holder = heap[i1];
            heap[i1] = heap[i2];
            heap[i2] = holder;
        }


        private void Grow(int minCapacity)
        {
            int oldCapacity = heap.Length;
            int newCapacity = oldCapacity + (oldCapacity >> 1);
            if (newCapacity - minCapacity < 0)
                newCapacity = minCapacity;
            Array.Resize(ref heap, newCapacity);
            CAPACITY = newCapacity;
        }



        public bool Remove(T item)
        {
            int index = Array.IndexOf(heap, item, 0, size);

            if (index == -1)
            {
                return false;
            }

            size--;
            Swap(heap, index, size);

            Heapify(heap, index, size - 1);

            int parentIndex = GetParentIndex(index);
            if (index > 0 && heap[index].CompareTo(heap[parentIndex]) < 0)
            {
                // Heapify up
                while (index > 0 && heap[index].CompareTo(heap[parentIndex]) < 0)
                {
                    Swap(heap, index, parentIndex);
                    index = parentIndex;
                    parentIndex = GetParentIndex(index);
                }
            }

            return true;
        }



        public bool Contains(T item)
        {
            for (int i = 0; i < size; i++)
            {
                if (heap[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }


        public void Clear()
        {
            heap = new T[CAPACITY];
            size = 0;
        }


        // IEnumerable<T> implementation to support foreach
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return heap[i];
            }
        }

        // Non-generic enumerator for compatibility with IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public int Count => size;

        public bool IsEmpty() => size == 0;


        private int GetLeftChildIndex(int index) => index * 2 + 1;
        private int GetRightChildIndex(int index) => index * 2 + 2;
        private int GetParentIndex(int index) => (index - 1) / 2;

    }


}

