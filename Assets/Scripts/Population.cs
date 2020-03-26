using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    public int size;
    private int generationNumber = 0;
    private float bestScore = 0f;
    public GameObject creature;    
    Dictionary<GameObject,Creature> Creatures;

    private void Awake()
    {
        Creatures = new Dictionary<GameObject, Creature>();        
        for (int i = 0; i < size;i++)
        {
            var obj = Instantiate(creature, new Vector3(i * 10, 10f, 0), Quaternion.identity);
            Creatures.Add(obj, obj.GetComponent<Creature>());            
        }
    }
    private void Start()
    {
        InvokeRepeating("ResetGen", 5f, 10f);
    }
    // Update is called once per frame
    void ResetGen()
    {
        generationNumber++;
        foreach(var i in Creatures.Values)
        {
            i.ResetPosition();
        }       
    }
    private void OnGUI()
    {        
        GUI.Label(new Rect(new Vector2(0, 0), new Vector2(300, 30)), String.Format("Generation number: {0}", generationNumber));
        GUI.Label(new Rect(new Vector2(0, 30), new Vector2(300, 30)), String.Format("Best score: {0}", bestScore));
    }
}
