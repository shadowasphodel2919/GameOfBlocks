﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;

    // Start is called before the first frame update

    Shape getRandomShape()
    {
        int i = Random.Range(0, m_allShapes.Length);
        if (m_allShapes[i])
        {
            return m_allShapes[i];
        }
        else
        {
            Debug.Log("WARNING!! Invalid Shape");
            return null;
        }
    }

    public Shape spawnShape()
    {
        Shape shape = null;
        shape = Instantiate(getRandomShape(), transform.position, Quaternion.identity) as Shape;
        if (shape)
        {
            return shape; 
        }
        else
        {
            Debug.Log("WARNING!! Not a valid object");
            return null;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
