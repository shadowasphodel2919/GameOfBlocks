﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public Transform m_holderXform;
    public Shape m_heldShape = null;
    float m_scale = 0.5f;

    public bool m_canRelease = false;


    public void catchShape(Shape shape)
    {
        if (m_heldShape)
        {
            Debug.LogWarning("HOLDER WARNING! Release a shape before trying to hold");
            return;
        }
        if (!shape)
        {
            Debug.LogWarning("HOLDER WARNIG! Invalid Shape");
            return;
        }
        if (m_holderXform)
        {
            shape.transform.position = m_holderXform.position + shape.m_queueOffset;
            shape.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            m_heldShape = shape;
            shape.transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("HOLDER WARNING! Holder has no transform assigned");
        }
    }

    public Shape releaseShape()
    {
        m_heldShape.transform.localScale = Vector3.one;
        Shape shape = m_heldShape;
        m_heldShape = null;

        m_canRelease = false;

        return shape;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
