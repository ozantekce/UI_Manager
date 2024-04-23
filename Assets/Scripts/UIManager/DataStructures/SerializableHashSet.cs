using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{
    [Serializable]
    public class SerializableHashSet<T>
    {

        [SerializeField] private int _count;

        [SerializeField] private List<T> _list = new List<T>();


        // Non-serialized Dictionary to hold the index of each item in the list
        private Dictionary<T, int> _indexMap;

        public SerializableHashSet()
        {
            RebuildIndexMap();
        }

        private void RebuildIndexMap()
        {
            _count = _list.Count;
            _indexMap = new Dictionary<T, int>(_count);
            for (int i = 0; i < _count; i++)
            {
                _indexMap[_list[i]] = i;
            }
        }

        public void Add(T item)
        {
            if (!_indexMap.ContainsKey(item))
            {
                if (_count < _list.Count)
                {
                    _list[_count] = item;
                }
                else
                {
                    _list.Add(item);
                }

                _indexMap[item] = _count;
                _count++;
            }
        }

        public bool Remove(T item)
        {
            if (_indexMap.TryGetValue(item, out int index))
            {
                // Swap the item to remove with the last item
                _count--;
                if (index != _count)
                {
                    T lastItem = _list[_count];
                    _list[index] = lastItem;
                    _indexMap[lastItem] = index;
                }

                _indexMap.Remove(item);
                return true;
            }

            return false;
        }

        public bool Contains(T item)
        {
            return _indexMap.ContainsKey(item);
        }

        public int Count => _count;

        public List<T> Values { get => _list; }

        public T this[int index] => _list[index];

        public T GetFirst()
        {
            if (Count == 0) return default(T);
            return _list[0];
        }



    }


}

