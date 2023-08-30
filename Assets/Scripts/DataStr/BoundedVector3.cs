using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BoundedVector3
{


    [SerializeField] private BoundedFloat _x;
    [SerializeField] private BoundedFloat _y;
    [SerializeField] private BoundedFloat _z;

    public float X { get => _x.Value; set => _x.Value = value; }
    public float Y { get => _y.Value; set => _y.Value = value; }
    public float Z { get => _z.Value; set => _z.Value = value; }

    public Vector3 Vector { get=> new(X, Y, Z); set
        {
            X = value.x;
            Y = value.y;
            Z = value.z;
        } 
    }

}
