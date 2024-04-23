using UnityEngine;

namespace UIManager
{
    [System.Serializable]
    public struct BoundedInteger
    {

        [field: SerializeField] public int Min { get; set; }
        [field: SerializeField] public int Max { get; set; }

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                int tempValue = value;
                if (tempValue < Min) return;
                if (tempValue > Max) return;
                _value = tempValue;
            }
        }

    }


}
