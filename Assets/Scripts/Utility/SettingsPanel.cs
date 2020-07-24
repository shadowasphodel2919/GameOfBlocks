using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    GameController m_gameController;
    TouchController m_touchController;

    public Slider m_dragDistanceSlider;
    public Slider m_swipeDistanceSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_gameController = GameObject.FindObjectOfType<GameController>().GetComponent<GameController>();
        m_touchController = GameObject.FindObjectOfType<TouchController>().GetComponent<TouchController>();
        setOriginalValues();
    }

    void setOriginalValues()
    {
        if (m_dragDistanceSlider != null)
        {
            m_dragDistanceSlider.value = m_touchController.m_minDragDistance;
            m_dragDistanceSlider.minValue = 50;
            m_dragDistanceSlider.maxValue = 150;
        }
        if (m_swipeDistanceSlider != null)
        {
            m_swipeDistanceSlider.value = m_touchController.m_minSwipeDistance;
            m_swipeDistanceSlider.minValue = 20;
            m_swipeDistanceSlider.maxValue = 250;
        }

    }
    public void UpdatePanel()
    {
        if (m_dragDistanceSlider != null)
        {
            if (m_touchController != null)
            {
                m_touchController.m_minDragDistance = (int)m_dragDistanceSlider.value;
            }
        }

        if (m_swipeDistanceSlider != null)
        {
            if (m_touchController != null)
            {
                m_touchController.m_minSwipeDistance = (int)m_swipeDistanceSlider.value;
            }
        }
    }
}
