using UnityEngine;

namespace UIManager
{

    [System.Serializable]
    public struct BoundedVector3
    {

        [field: SerializeField] public float X { get; set; }
        [field: SerializeField] public float Y { get; set; }
        [field: SerializeField] public float Z { get; set; }

        public Vector3 Vector
        {
            get => new(X, Y, Z); set
            {
                X = value.x;
                Y = value.y;
                Z = value.z;
            }
        }

    }


}

