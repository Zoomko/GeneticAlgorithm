using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransform
{
    public float x;
    public float y;
    public float z;
    Quaternion rotation;
    public MyTransform(Vector3 v, Quaternion q)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        rotation = q;
    }

    public Vector3 Position
    {
        get
        {
            return new Vector3(x, y, z);
        }
    }
    public Quaternion Rotaion
    {
        get
        {
            return rotation;
        }
    }
}
