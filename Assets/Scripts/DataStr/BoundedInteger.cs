using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BoundedInteger
{

    private int _min;
    private int _max;
    private int _value;

    public int Min { get => _min; set => _min = value; }
    public int Max { get => _max; set => _max = value; }
    public int Value
    {
        get => _value;
        set
        {
            int tempValue = value;
            if (tempValue < _min) return;
            if (tempValue > _max) return;
            _value = tempValue;
        }
    }



}
