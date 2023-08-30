/// <summary>
/// Min ve Max degeri tutan basit bir veri yapisi.
/// </summary>
[System.Serializable]
public struct FloatMinMax
{

    public float min; 
    public float max;

    public FloatMinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

}
