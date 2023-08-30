using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BoundedFloat
{

    [SerializeField] private float _min;
    [SerializeField] private float _max;
    [SerializeField] private float _value;

    public float Min { get => _min; set => _min = value; }
    public float Max { get => _max; set => _max = value; }
    public float Value
    {
        get => _value;
        set
        {
            float tempValue = value;
            if (tempValue < _min) return;
            if (tempValue > _max) return;
            _value = tempValue;
        }
    }



}
