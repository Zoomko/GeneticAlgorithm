using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    public int size;
    public int generationNumber;    
    public GameObject creature;
    List<GameObject> creatures;

    private void Awake()
    {
        creatures = new List<GameObject>();
        for (int i = 0; i < size;i++)
        {
            creatures.Add(Instantiate(creature, new Vector3(i * 10, 10f, 0), Quaternion.identity));
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(var i in creatures)
            {
                i.GetComponent<Creature>().ResetPosition();
            }
        }
    }
}
