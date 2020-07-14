using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;

    public int m_header=8;

    Transform[,] m_grid;

    public int m_completeRows = 0;

    private void Awake()
    {
        m_grid = new Transform[m_width, m_height];
    }

    void Start()
    {
        drawEmptyCells();
    }

    void Update()
    {
        
    }

    bool isWithinBoard(int x,int y)
    {
        return (x >= 0 && x < m_width && y >= 0);
    }

    public bool isValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);

            if (!isWithinBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (isOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }
        }

        return true;
    }

    void drawEmptyCells()
    {
        if (m_emptySprite != null)
        {
            for (int y = 0; y < m_height-m_header; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Transform clone;
                    clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = "Board Space ( x = " + x.ToString() + y.ToString() + ")";
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("WARNING! BITCH ASSIGN A SPRITE TO IT");
        }
    }

    public void storeShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }
        foreach(Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            m_grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool isOccupied(int x,int y,Shape shape)
    {
        return (m_grid[x, y] != null && m_grid[x, y].parent != shape.transform);
    }


    bool isComplete(int x)//x is row
    {
        for (int y = 0; y < m_width; ++y)
        {
            if (m_grid[y, x] == null)
            {
                return false;
            }
        }
        return true;
    }

    void clearRow(int x)
    {
        for(int y = 0; y < m_width; ++y)
        {
            if (m_grid[y, x] != null)
            {
                Destroy(m_grid[y, x].gameObject);
            }
            m_grid[y,x] = null;
        }
    }

    void shiftOneRowDown(int y)
    {

        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    void shiftRowsDown(int y)
    {
        for (int i = y; i < m_height; ++i)
        {
            shiftOneRowDown(i);
        }
    }

    public void clearAllRows()
    {
        m_completeRows = 0;
        for(int i = 0; i < m_height; ++i)
        {
            if (isComplete(i))
            {
                m_completeRows++;
                clearRow(i);
                shiftRowsDown(i + 1);
                i--;
            }
        }
    }

    public bool isOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= m_height - m_header)
            {
                return true;
            }
        }
        return false;
    }
}
