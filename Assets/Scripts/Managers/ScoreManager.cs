using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text m_linesText;
    public Text m_levelText;
    public Text m_scoreText;
    int m_score;
    int m_lines;
    public int m_level = 1;
    public int m_linesPerLevel = 3;

    public bool m_didLevelUp = false;

    const int m_minLines = 1;
    const int m_maxLines = 4;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        m_level = 1;
        m_lines = m_linesPerLevel * m_level;
        updateUIText();
    }

    public void scoreLines(int n)
    {
        m_didLevelUp = false;
        n = Mathf.Clamp(n, m_minLines, m_maxLines);
        switch (n)
        {
            case 1:
                m_score += 40 * m_level;
                break;
            case 2:
                m_score += 100 * m_level;
                break;
            case 3:
                m_score += 300 * m_level;
                break;
            case 4:
                m_score += 1200 * m_level;
                break;
        }
        m_lines -= n;
        if (m_lines <= 0)
        {
            levelUp();
        }
        updateUIText();
    }

    void updateUIText()
    {
        if (m_linesText)
        {
            m_linesText.text = m_lines.ToString();
        }
        if (m_levelText)
        {
            m_levelText.text = m_level.ToString();
        }
        if (m_scoreText)
        {
            m_scoreText.text = addZero(m_score,5);
        }
    }

    string addZero(int n,int addDigits)
    {
        string nStr = n.ToString();

        while (nStr.Length < addDigits)
        {
            nStr = "0" + nStr;
        }
        return nStr;
    }

    public void levelUp()
    {
        m_level++;
        m_lines = m_linesPerLevel * m_level;
        m_didLevelUp = true;
        if (m_levelUpFX)//EFFECT
        {
            m_levelUpFX.play();
        }
    }

    //Effect
    public ParticlePlayer m_levelUpFX;

}
