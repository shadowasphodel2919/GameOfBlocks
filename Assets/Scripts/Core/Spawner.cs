using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;
    public Transform[] m_queuedXforms = new Transform[3];

    public float m_queueScale = 0.5f;

    Shape[] m_queuedShapes = new Shape[3];
    // Start is called before the first frame update

    //Effects
    public ParticlePlayer m_spawnFX;

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
        Shape shape = getQueuedShape();
        //shape = Instantiate(getRandomShape(), transform.position, Quaternion.identity) as Shape;
        shape.transform.position = transform.position;


        //shape.transform.localScale = Vector3.one;


        //Using growShape Coroutine rather than localScale
        StartCoroutine(growShape(shape, transform.position, 0.25f));


        if (m_spawnFX)
        {
            m_spawnFX.play();
        }
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

    void initQueue()
    {
        for(int i = 0; i < m_queuedShapes.Length; i++)
        {
            m_queuedShapes[i] = null;
        }
        fillQueue();
    }

    void fillQueue()
    {
        for(int i = 0; i < m_queuedShapes.Length; i++)
        {
            if (!m_queuedShapes[i])
            {
                m_queuedShapes[i] = Instantiate(getRandomShape(), transform.position, Quaternion.identity) as Shape;
                m_queuedShapes[i].transform.position = m_queuedXforms[i].position + m_queuedShapes[i].m_queueOffset;
                m_queuedShapes[i].transform.localScale = new Vector3(m_queueScale, m_queueScale, m_queueScale);
            }
        }
    }

    Shape getQueuedShape()
    {
        Shape fShape = null;
        if (m_queuedShapes[0])
        {
            fShape = m_queuedShapes[0];
        }
        for(int i = 1; i < m_queuedShapes.Length; i++)
        {
            m_queuedShapes[i - 1] = m_queuedShapes[i];
            m_queuedShapes[i - 1].transform.position = m_queuedXforms[i - 1].position + m_queuedShapes[i].m_queueOffset;
        }
        m_queuedShapes[m_queuedShapes.Length - 1] = null;
        fillQueue();
        return fShape;
    }

    private void Awake()
    {
        initQueue();
    }
   
    //Effect to grow shape at spawn
    IEnumerator growShape(Shape shape,Vector3 position,float growTime = 0.5f)
    {
        float size = 0f;
        growTime = Mathf.Clamp(growTime, 0.1f, 2f);
        float sizeDelta = Time.deltaTime / growTime;

        while (size < 1f)
        {
            shape.transform.localScale = new Vector3(size, size, size);
            size += sizeDelta;
            shape.transform.position = position;
            yield return null;
        }
        shape.transform.localScale = Vector3.one;
    }
}
