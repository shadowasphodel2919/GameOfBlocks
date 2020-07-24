using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconToggle : MonoBehaviour
{
    public Sprite m_trueState;
    public Sprite m_falseState;

    public bool m_defaultState = true;

    Image m_image;
    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        m_image.sprite = (m_defaultState) ? m_trueState : m_falseState;
    }

    public void iconToggle(bool state)
    {
        if (!m_image || !m_trueState || !m_falseState)
        {
            Debug.LogWarning("WARNING!!Toggle Icon");
            return;
        }
        m_image.sprite = (state) ? m_trueState : m_falseState;
    }
}
