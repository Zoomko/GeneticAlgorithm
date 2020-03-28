using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Convert
{
    public static float JointTo(float v)
    {
        return (v + 15000f) / 30000;
    }
    public static float JointOut(float v)
    {
        return v * 30000 - 15000;
    }
    public static float Rotation(float v)
    {
        return ((v + 180) % 360)/360;
    }
}
