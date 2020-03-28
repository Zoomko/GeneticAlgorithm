using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [HideInInspector] public float score = 0f;
    public float motorForce;
    [HideInInspector]public float time = 0f;
    List<float> values;
    public NeuralNetwork net;
    (Transform, MyTransform) body;
    HingeJoint[] hinges;
    List<(Transform, MyTransform)> PartsOfBody;

    private void Awake()
    {
        hinges = GetComponentsInChildren<HingeJoint>();        
        PartsOfBody = new List<(Transform, MyTransform)>();
        foreach (var i in GetComponentsInChildren<Transform>())
        {
            Transform t = i as Transform;
            if (t != null)
            {
                PartsOfBody.Add((t, new MyTransform(t.position, t.rotation)));
                if (i.name == "body")
                    body = (t, new MyTransform(t.position, t.rotation));
            }
        }        
        net = new NeuralNetwork(hinges.Length, 4);
        InitMoving(hinges.Length);
    }

    private void Update()
    {
        Moving();
    }
    public void ChangeScore()
    {
        score = Vector2.SqrMagnitude(new Vector2(0, 0) - new Vector2(0, 1000)) - Vector2.SqrMagnitude(new Vector2(body.Item1.position.x, body.Item1.position.z) - new Vector2(body.Item2.x, body.Item2.z+1000));
    }
    public void ResetPosition()
    {
        ChangeScore();
        time = 0;
        foreach (var it in PartsOfBody)
        {
            it.Item1.position = it.Item2.Position;
            it.Item1.rotation = it.Item2.Rotaion;
        }
    }
    void InitMoving(int n)
    {
        values = new List<float>();
        for (int i = 0; i < n; i++)
        {
            values.Add(Convert.JointTo(hinges[i].motor.targetVelocity));
        }
    }
    void AssingMovingValues()
    {
        for (int i = 0; i<values.Count; i++)
            {
                JointMotor motor = hinges[i].motor;
                motor.force = motorForce;
                motor.targetVelocity = Convert.JointOut(values[i]);                
                hinges[i].motor = motor;
            }
    }
    void Moving()
    {
        time += Time.deltaTime;        
        values.Add((Mathf.Sin(time*4) + 1) / 2);
        Vector3 v = body.Item1.rotation.eulerAngles;
        
        values.Add(Convert.Rotation(v.x));
        values.Add(Convert.Rotation(v.y));
        values.Add(Convert.Rotation(v.z));
        
        values = net.Calculate(values);
        
        AssingMovingValues();
    }   
}
