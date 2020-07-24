using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public float m_startAlpha = 1f;
    public float m_targetAlpha = 0f;
    public float m_delay = 0f;
    public float m_timeToFade = 1f;

    //cache
    float m_inc;
    float m_currentAlpha;
    MaskableGraphic m_graphic;
    Color m_originalColor;

    // Start is called before the first frame update
    void Start()
    {
        m_graphic = GetComponent<MaskableGraphic>();
        m_originalColor = m_graphic.color;
        m_currentAlpha = m_startAlpha;

        Color temp = new Color(m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);
        m_graphic.color = temp;

        m_inc = ((m_targetAlpha - m_startAlpha) / m_timeToFade) * Time.deltaTime;

        StartCoroutine("FadeRoutine");        
    }
    /// <summary>
    /// Coroutines are special routines which wait for a yield function to complete its work
    /// </summary>
    /// <returns>
    /// Coroutines return a special IEnumerator object
    /// </returns>
    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(m_delay);
        while (Mathf.Abs(m_targetAlpha - m_currentAlpha) > 0.01f)
        {
            yield return new WaitForEndOfFrame();
            m_currentAlpha = m_currentAlpha + m_inc;
            Color temp = new Color(m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);
            m_graphic.color = temp;
        }
        Debug.Log("SCREEN FADER");
    }
}
