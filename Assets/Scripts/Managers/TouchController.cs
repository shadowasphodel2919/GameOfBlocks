using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler dragEvent;
    public static event TouchEventHandler swipeEvent;
    public static event TouchEventHandler tapEvent;

    Vector2 m_touchMovement;

    [Range(50, 150)]
    public int m_minDragDistance = 100;

    [Range(20,250)]
    public int m_minSwipeDistance = 50;

    float m_tapTimeMax = 0;
    public float m_tapTimeWindow = 0.2f;

    void onTap()
    {
        if (tapEvent != null)
        {
            tapEvent(m_touchMovement);
        }
    }

    void onDrag()
    {
        if (dragEvent != null)
        {
            dragEvent(m_touchMovement);
        }
    }

    void onSwipeEnd()
    {
        if (swipeEvent != null)
        {
            swipeEvent(m_touchMovement);
        }
    }

    public Text m_diagnosticText1;
    public Text m_diagnosticText2;

    public bool m_useDiagnostic = false;

    void diagnostic(string text1,string text2)
    {
        m_diagnosticText1.gameObject.SetActive(m_useDiagnostic);
        m_diagnosticText2.gameObject.SetActive(m_useDiagnostic);

        if(m_diagnosticText1 && m_diagnosticText2)
        {
            m_diagnosticText1.text = text1;
            m_diagnosticText2.text = text2;
        }
    }

    string swipeDiagnostic (Vector2 swipeMovement)
    {
        string direction = "";

        //hori
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            direction = (swipeMovement.x >= 0) ? "right" : "left";
        }
        //ver
        else
        {
            direction = (swipeMovement.y >= 0) ? "up" : "down";
        }
        return direction;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        diagnostic("", "");  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                m_touchMovement = Vector2.zero;
                m_tapTimeMax = Time.time + m_tapTimeWindow;
                diagnostic("", "");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                m_touchMovement += touch.deltaPosition;
                if (m_touchMovement.magnitude > m_minDragDistance)
                {
                    onDrag();
                    diagnostic("Drag Detected", m_touchMovement.ToString() + " " + swipeDiagnostic(m_touchMovement));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (m_touchMovement.magnitude > m_minSwipeDistance)
                {
                    onSwipeEnd();
                    diagnostic("Swipe Detected", m_touchMovement.ToString() + " " + swipeDiagnostic(m_touchMovement));
                }
                else if (Time.time < m_tapTimeMax)
                {
                    onTap();
                    diagnostic("Tap Detected", m_touchMovement.ToString() + " " + swipeDiagnostic(m_touchMovement));
                }
            }
        }
    }
}
