using System.Collections.Generic;
using UnityEngine;
using System;


public class NeuralNetwork
{
    public List<Layer> layers;
    public readonly int baseNubmer;
    public readonly int bonus;
    public readonly int layercount;

    public NeuralNetwork(int n,int b=0)
    {
        baseNubmer = n;
        bonus = b;

        layers = new List<Layer>();
        layers.Add(new Layer(n+b));
        layers.Add(new Layer(n+b+n/2, Function.Sig,LastLayer()));        
        layers.Add(new Layer(n, Function.Sig, LastLayer()));

        layercount = layers.Count;
    }

    public Layer LastLayer()
    {
        return layers[layers.Count - 1];
    }
    
    public List<float> Calculate(List<float> v)
    {
        foreach (var l in layers)
        {
            if (l.active != null)
            {
                l.CalculateLayer();
            }
            else
            {
                l.Values = v;
            }
        }
        return new List<float>(LastLayer().Values);
    }

    public void Mutate(NeuralNetwork nn, int k)
    {
        for(int i = 1; i < layers.Count; i++)
        {
            for (int j = 0; j < layers[i].cells.Count; j++)
            {
                layers[i].cells[j].Copy(nn.layers[i].cells[j]);
                layers[i].cells[j].Mutate(k);
            }                
        }
    }        
}

public static class Function
{
    public static float Sig(float v)
    {
        return 1 / (1 + Mathf.Exp(-v));
    }
    public static float ReLu(float v)
    {
        if (v < 0)
            return 0f;
        else return v;
    }
    public static float Square(float v)
    {
        return v * v;
    }
}
public class Layer
{
    Layer LastLayer;
    public int lastLayerCount;
    List<float> layerValues;    
    public List<Cell> cells;
    public Func<float, float> active;
    public Layer(int n)
    {
        layerValues = new List<float>();
        initList(n);
        cells = new List<Cell>();
        InputLayer(n);
    }
    
    public Layer(int n, Func<float, float> a, Layer l)
    {        
        layerValues = new List<float>();
        initList(n);

        active = a;

        cells = new List<Cell>();

        
        LastLayer = l;
        lastLayerCount = l.cells.Count;

        OtherLayer(n);
    }
    void initList(int n)
    {
        for (int i = 0; i < n; i++)
            layerValues.Add(0f);
    }
    void InputLayer(int n)
    {        
        for(int i = 0; i < n; i++)
        {
            cells.Add(new Cell());
        }
    }
    void OtherLayer(int n)
    {
        for (int i = 0; i < n; i++)
        {
            cells.Add(new Cell(lastLayerCount));
        }
    }   
   
    public List<float> Values
    {
        get
        {
            for (int i = 0; i < cells.Count; i++)
            {
                layerValues[i]=cells[i].value;
            }
            return layerValues;
        }
        set
        {
            for (int i = 0; i < cells.Count; i++)
                cells[i].value = value[i];
        }
    }

    public void CalculateLayer()
    {        
        foreach(var i in cells)
        {
            i.Calculate(LastLayer.Values,active);
        }
    }
    
}
public class Cell
{
    public float value=0;
    private float[] weight;
    private float bias = 0f;
    
    public Cell()
    {        
    }
    public Cell(int number)
    {        
        weight = new float[number];
        for(int i = 0; i< number; i++)
        {
            weight[i] = UnityEngine.Random.value * 50f - 25f;            
        }
        bias = UnityEngine.Random.value*100f-50f;
    }
    public void Copy(Cell c)
    {        
        weight = (float[])c.weight.Clone();
        bias = c.bias;
    }
    public void Calculate(List<float>v, Func<float,float> Activation)
    {
        value = 0;

        for(int i=0; i<weight.Length; i++)
        {
            value += v[i] * weight[i];            
        }

        value += bias;
        value = Activation(value);
    }

    public void Mutate(int k)
    {
        var v = Function.Square(k / 4f);
        for (int i = 0; i < weight.Length; i++)
        {
            weight[i] += UnityEngine.Random.value * 30f/v - 15f/v; 
        }
        bias += UnityEngine.Random.value * 60f/v - 30f/v;
    }
}
