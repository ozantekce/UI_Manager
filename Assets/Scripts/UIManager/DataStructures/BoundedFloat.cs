using UnityEngine;

namespace UIManager
{
    [System.Serializable]
    public struct BoundedFloat
    {

        [field: SerializeField] public float Min { get; set; }
        [field: SerializeField] public float Max { get; set; }

        [SerializeField] private float _value;

        public float Value
        {
            get => _value;
            set
            {
                float tempValue = value;
                if (tempValue < Min) return;
                if (tempValue > Max) return;
                _value = tempValue;
            }
        }

    }


}

