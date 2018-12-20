using UnityEngine;
using UnityEditor;

public class Utility
{
    public static float Map(float value, float r1, float r2, float m1, float m2)//mapping function used in sin wave terrain generation -- taken from examples in class
    {
        float dist = value - r1;
        float range1 = r2 - r1;
        float range2 = m2 - m1;
        return m1 + ((dist / range1) * range2);
    }
}

