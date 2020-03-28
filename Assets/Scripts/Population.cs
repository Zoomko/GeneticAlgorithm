using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Population : MonoBehaviour
{
    public float timeBetween;
    public int size;
    private int generationNumber = 0;
    private float bestScore = 0;
    public GameObject creature;

    NeuralNetwork x;
    NeuralNetwork y;
    NeuralNetwork xy;
    NeuralNetwork yx;

    Dictionary<GameObject,Creature> Creatures;

    private void Awake()
    {
        Creatures = new Dictionary<GameObject, Creature>();        
        for (int i = 0; i < size;i++)
        {
            var obj = Instantiate(creature, new Vector3(i * 10, 11f, 0), Quaternion.identity);
            Creatures.Add(obj, obj.GetComponent<Creature>());            
        }
    }
    private void Start()
    {
        InvokeRepeating("ResetGen", timeBetween, timeBetween);
    }
    
    void SetBestScore()
    {
        bestScore = -1000f;
        foreach (var i in Creatures.Values)
        {
            if (i.score > bestScore)
                bestScore = i.score;
        }        
        
    }
    public void Selection()
    {        
        int c = 0;
        foreach(var i in Creatures.OrderByDescending(pair => pair.Value.score))
        {
            if (c == 0)
                x = i.Value.net;
            else if (c == 1)
                y = i.Value.net;
            else break;
            c++;
        }        
    }
    public void Cross()
    {
        xy = new NeuralNetwork(x.baseNubmer, x.bonus);
        yx = new NeuralNetwork(x.baseNubmer, x.bonus);
        for (int i = 1; i < x.layercount; i++)
            for (int j = 0; j < x.layers[i].cells.Count; j++)
            {
                if(UnityEngine.Random.value>0.5)
                {
                    xy.layers[i].cells[j].Copy(x.layers[i].cells[j]);
                    yx.layers[i].cells[j].Copy(y.layers[i].cells[j]);
                }
                else
                {
                    yx.layers[i].cells[j].Copy(x.layers[i].cells[j]);
                    xy.layers[i].cells[j].Copy(y.layers[i].cells[j]);
                }
            }
    }
    public void Mutate()
    {
        int c = 0;        
        int n = Creatures.Count / 4;
        foreach (var i in Creatures.Values)
        {
            if (c < n)
            {
                i.net.Mutate(x,c+1);
            }
            else if(c<n*2)
            {
                i.net.Mutate(y,(c%n)+1);
            }
            else if (c<n*3)
            {
                i.net.Mutate(xy, (c%(n*2))+1);
            }
            else if (c< n*4)
            {
                i.net.Mutate(yx, (c%(n*3))+1);
            }
            c++;
        }
    }

    // Update is called once per frame
    void ResetGen()
    {       
        generationNumber++;        
        foreach(var i in Creatures.Values)
        {
            i.ResetPosition();
        }
        SetBestScore();

        Selection();
        Cross();
        Mutate();
    }
    private void OnGUI()
    {        
        GUI.Label(new Rect(new Vector2(0, 0), new Vector2(300, 30)), String.Format("Generation number: {0}", generationNumber));
        GUI.Label(new Rect(new Vector2(0, 30), new Vector2(300, 30)), String.Format("Best score: {0}", bestScore));
    }
}
