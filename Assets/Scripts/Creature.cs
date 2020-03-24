using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature:MonoBehaviour
{
    float score = 0f;
    bool isAlive = true;
    HingeJoint[] hinges;
    List<(Transform,MyTransform)> PartsOfBody;    
    private void Awake()
    {                
        hinges = GetComponentsInChildren<HingeJoint>();
        PartsOfBody = new List<(Transform, MyTransform)>();
        foreach (var i in GetComponentsInChildren<Transform>())
        {
            Transform t = i as Transform;
            if (i != null)
            {
                PartsOfBody.Add((t, new MyTransform(t.position, t.rotation)));                
            }
        }
        print(PartsOfBody.Count);
    }
   
    private void Update()
    {
        foreach(var i in hinges)
        {
            i.motor = ChangetargetVelocity(i.motor);
        }
    }
    public void ResetPosition()
    {
        foreach (var it in PartsOfBody)
        {
            it.Item1.position = it.Item2.Position;
            it.Item1.rotation = it.Item2.Rotaion;
        }
    }
    JointMotor ChangetargetVelocity(JointMotor m)
    {
        var motor = m;
        if (Random.value > 0.8)
        {
            motor.targetVelocity = 100;
        }
        else
        {
            motor.targetVelocity = -100;
        }
        return motor;
    }    
}
class MyTransform
{
    float x;
    float y;
    float z;
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