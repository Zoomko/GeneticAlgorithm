using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    List<Layer> layers;
    public NeuralNetwork()
    {
        layers.Add(new Layer(2, 'i'));
        layers.Add(new Layer(4, 'o', layers[layers.Count - 1]));
    }
}
public class Layer
{
    Layer LastLayer;
    List<Cell> cells;
    public Layer(int n, char s)
    {
        cells = new List<Cell>();
        if (s == 'i')
            InputLayer(n);
        else if (s == 'h')
            HidenLayer(n);
        else if (s == 'o')
            OutPutLayer(n);
    }
    public Layer(int n, char s, Layer l):this(n,s)
    {
        LastLayer = l;
    }

    void InputLayer(int n)
    {        
        for(int i = 0; i< n; i++)
        {
            cells.Add(new Cell());
        }
    }
    void HidenLayer(int n)
    {
        for (int i = 0; i < n; i++)
        {
            cells.Add(new Cell());
        }
    }
    void OutPutLayer(int n)
    {
        for (int i = 0; i < n; i++)
        {
            cells.Add(new Cell());
        }
    }
}
public class Cell
{
    float value;
    float[] weight;
    float bias;
    public Cell()
    {
        value = 0;
    }
    public Cell(int number)
    {
        value = 0;
        weight = new float[number];
    }
    public Cell(float[]w,float b)
    {
        weight = w;
        bias = b;
    }
}
