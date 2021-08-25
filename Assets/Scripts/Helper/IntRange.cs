using UnityEngine;

[System.Serializable]
public struct IntRange
{
    [SerializeField]
    int _min, _max;

    public int Min => _min;

    public int Max => _max;

    public IntRange(int value)
    {
        _min = _max = value;
    }

    public IntRange(int min, int max)
    {
        _min = min;
        _max = max < min ? min : max;
    }

    public static IntRange operator +(IntRange a, IntRange b)
    {
        return new IntRange(a._min + b._min, a._max + b._max);
    }

    public static IntRange operator -(IntRange a, IntRange b)
    {
        return new IntRange(a._min - b._min, a._max - b._max);
    }
}
