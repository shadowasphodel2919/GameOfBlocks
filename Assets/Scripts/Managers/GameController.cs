using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Board m_gameBoard;
    Spawner m_spawner;

    Shape m_activeShape;

    public GameObject m_gameOverPanel;

    bool m_gameOver = false;

    SoundManager m_soundManager;

    public float m_dropInterval = 0.9f;

    float m_timeToDrop;

    //variables if you use GetButton to slow
    /*float m_timeToNextKey;
    [Range(0.02f,1f)]//It introduces a slider for m_keyRepeatRate
    public float m_keyRepeatRate = 0.25f;*/

    float m_timeToNextKeyLR;
    [Range(0.02f, 1f)]//It introduces a slider for m_keyRepeatRate
    public float m_keyRepeatRateLR = 0.113f;

    float m_timeToNextKeyD;
    [Range(0.01f, 1f)]//It introduces a slider for m_keyRepeatRate
    public float m_keyRepeatRateD = 0.05f;

    float m_timeToNextKeyRotate;
    [Range(0.02f, 1f)]//It introduces a slider for m_keyRepeatRate
    public float m_keyRepeatRateRotate = 0.205f;

    void Start()
    {
        //m_timeToNextKey = Time.time;//redundant
        m_timeToNextKeyLR = Time.time+ m_timeToNextKeyLR;
        m_timeToNextKeyD = Time.time+ m_timeToNextKeyD;
        m_timeToNextKeyRotate = Time.time+ m_timeToNextKeyRotate;


        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (!m_soundManager)
        {
            Debug.Log("WARNING!! No sound manager detected");
        }
        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING!! There is no gameboard defined");
        }
        if (!m_spawner)
        {
            Debug.LogWarning("WARNING!! There is no spawner defined");
        }
        else
        {
            if (m_activeShape == null)
            {
                m_activeShape = m_spawner.spawnShape();
            }
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }
        
    }

    void playerInput()
    {
        // if (Input.GetButtonDown("MoveRight"))
        if ((Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLR))
        {
            m_activeShape.moveRight();
            //withGetButton
            m_timeToNextKeyLR = Time.time + m_keyRepeatRateLR;
            if (m_gameBoard.isValidPosition(m_activeShape))
            {
                //Debug.Log("Move Right");
                playSound(m_soundManager.m_moveSound,1f);
            }
            else
            {
                m_activeShape.moveLeft();
                playSound(m_soundManager.m_errorSound,1f);
                //Debug.Log("Hit the right boundary");
            }
        }

        else if ((Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLR) )
        {
            m_activeShape.moveLeft();
            //withGetButton
            m_timeToNextKeyLR = Time.time + m_keyRepeatRateLR;
            if (m_gameBoard.isValidPosition(m_activeShape))
            {
                //Debug.Log("Move Left");
                playSound(m_soundManager.m_moveSound, 1f);
            }
            else
            {
                m_activeShape.moveRight();
                playSound(m_soundManager.m_errorSound, 1f);
                //Debug.Log("Hit the left boundary");
            }
        }

        else if (Input.GetButtonDown("Rotate") && Time.time > m_timeToNextKeyRotate)//onlyGetButtonDown
        {
            m_activeShape.rotateRight();
            //withGetButton
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
            if (m_gameBoard.isValidPosition(m_activeShape))
            {
                //Debug.Log("rotateRight");
                playSound(m_soundManager.m_moveSound, 1f);
            }
            else
            {
                m_activeShape.rotateRight();
                playSound(m_soundManager.m_errorSound, 1f);
                //Debug.Log("Hit the rotateRight boundary");
            }
        }

        else if ((Input.GetButton("MoveDown") && Time.time > m_timeToNextKeyD) ||Time.time > m_timeToDrop)
        {
            m_timeToDrop = Time.time + m_dropInterval;
            //withGetButton
            m_timeToNextKeyD = Time.time + m_keyRepeatRateD;
            if (m_activeShape)
            {
                m_activeShape.moveDown();

                if (!m_gameBoard.isValidPosition(m_activeShape))
                {

                    if (m_gameBoard.isOverLimit(m_activeShape))
                    {
                        gameOver();
                    }
                    else
                    {
                        landShape();
                    }
                }
            }
        }
    }

    void playSound(AudioClip clip,float volumeMutiplier)
    {
        if (m_soundManager.m_fxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume*volumeMutiplier,0.05f,1f));
            //clamp -->Returns the min value if the given float value is less than the min. Returns the max value if the given value is greater than the max value. Use Clamp to restrict a value to a range that is defined by the min and max values.
        }
    }

    void gameOver()
    {
        m_activeShape.moveUp();
        m_gameOver = true;
        Debug.LogWarning(m_activeShape + "is near limit");
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
        playSound(m_soundManager.m_gameOverSound, 1f);
        playSound(m_soundManager.m_gameOverVocal, 1f);
    }

    void landShape()
    {
        // m_timeToNextKey = Time.time;
        m_timeToNextKeyLR = Time.time;
        m_timeToNextKeyD = Time.time;
        m_timeToNextKeyRotate = Time.time;
        m_activeShape.moveUp();
        m_gameBoard.storeShapeInGrid(m_activeShape);
        if (m_spawner)
        {
            m_activeShape = m_spawner.spawnShape();
        }

        //InvokingBoard class members
        m_gameBoard.clearAllRows();

        playSound(m_soundManager.m_dropSound, 1f);

        if (m_gameBoard.m_completeRows > 0)
        {
            if (m_gameBoard.m_completeRows > 1)
            {
                AudioClip randomSound = m_soundManager.returnRandomClip(m_soundManager.m_exclaimationSounds);
                playSound(randomSound,1f);
            }
            playSound(m_soundManager.m_clearRowSound,1f);
        }
    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver||!m_soundManager)
        {
            return;
        }
        playerInput();
    }

    public void restart()
    {
        Debug.Log("Restarted");
        Application.LoadLevel(Application.loadedLevel);
    }
}
