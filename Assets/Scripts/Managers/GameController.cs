using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Board m_gameBoard;
    Spawner m_spawner;

    Shape m_activeShape;

    public GameObject m_gameOverPanel;

    bool m_gameOver = false;

    public IconToggle m_rotIconToggle;
    bool m_clockwise = true;

    SoundManager m_soundManager;
    ScoreManager m_scoreManager;

    // GhostShapeHandle m_ghost;
    // ghost for visualization
    Ghost m_ghost;

    Holder m_holder;

    public float m_dropInterval = 0.9f;
    float m_dropIntervalChanged;

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

    public bool m_isPaused = false;
    public GameObject m_pausePanel;

    void Start()
    {
        //m_timeToNextKey = Time.time;//redundant
        m_timeToNextKeyLR = Time.time+ m_timeToNextKeyLR;
        m_timeToNextKeyD = Time.time+ m_timeToNextKeyD;
        m_timeToNextKeyRotate = Time.time+ m_timeToNextKeyRotate;


        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        m_ghost = GameObject.FindObjectOfType<Ghost>();

        //   m_ghost = GameObject.FindObjectOfType<GhostShapeHandle>();
        m_holder = GameObject.FindObjectOfType<Holder>();
        if (!m_soundManager)
        {
            Debug.Log("WARNING!! No sound manager detected");
        }
        if (!m_scoreManager)
        {
            Debug.Log("WARNING!! No score manager detected");
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
        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }
        m_dropIntervalChanged = Mathf.Clamp(m_dropInterval-((float)m_scoreManager.m_level*0.1f),0.05f,1f);

        /*if (diagText)
        {
            diagText.text = "";
        }*/
    }

    void moveRight()
    {
        m_activeShape.moveRight();
        //withGetButton
        m_timeToNextKeyLR = Time.time + m_keyRepeatRateLR;
        if (m_gameBoard.isValidPosition(m_activeShape))
        {
            //Debug.Log("Move Right");
            playSound(m_soundManager.m_moveSound, 1f);
        }
        else
        {
            m_activeShape.moveLeft();
            playSound(m_soundManager.m_errorSound, 1f);
            //Debug.Log("Hit the right boundary");
        }
    }

    void moveLeft()
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

    void rotate()
    {
        //m_activeShape.rotateRight();
        //withGetButton
        m_activeShape.rotateClockwise(m_clockwise);
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
        if (m_gameBoard.isValidPosition(m_activeShape))
        {
            //Debug.Log("rotateRight");
            playSound(m_soundManager.m_moveSound, 1f);
        }
        else
        {
            //m_activeShape.rotateRight();
            m_activeShape.rotateClockwise(!m_clockwise);
            playSound(m_soundManager.m_errorSound, 1f);
            //Debug.Log("Hit the rotateRight boundary");
        }
    }

    void moveDown()
    {
        m_timeToDrop = Time.time + m_dropIntervalChanged;
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

    void playerInput()
    {
        // if (Input.GetButtonDown("MoveRight"))
        if ((Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLR))
        {
            moveRight();
        }

        else if ((Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLR) )
        {
            moveLeft();
        }

        else if (Input.GetButtonDown("Rotate") && Time.time > m_timeToNextKeyRotate)//onlyGetButtonDown
        {
            rotate();
        }

        else if ((Input.GetButton("MoveDown") && Time.time > m_timeToNextKeyD) ||Time.time > m_timeToDrop)
        {
            moveDown();
        }

        //touch
        else if ((m_dragDirection == Direction.right && Time.time > m_timeToNextDrag) || (m_swipeDirection == Direction.right && Time.time > m_timeToNextSwipe))
        {
            moveRight();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
           // m_dragDirection = Direction.none;
           // m_swipeDirection = Direction.none;
        }

        else if ((m_dragDirection == Direction.left && Time.time > m_timeToNextDrag) || (m_swipeDirection == Direction.left && Time.time > m_timeToNextSwipe))
        {
            moveLeft();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
            // m_dragDirection = Direction.none;
            // m_swipeDirection = Direction.none;
        }

        else if ((m_swipeDirection == Direction.up && Time.time > m_timeToNextSwipe)||(m_didTap))
        {
            rotate();
            m_didTap = false;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
            //m_swipeDirection = Direction.none;
        }

        else if (m_dragDirection == Direction.down && Time.time > m_timeToNextDrag)
        {
            moveDown();//you can add time to next darg here too but we didnt because we like it this way
           // m_dragDirection = Direction.none;
        }
        //touch
        else if (Input.GetButtonDown("ToggleRot"))
        {
            toggleRotDirection();
        }

        else if (Input.GetButtonDown("Pause"))
        {
            togglePause();
        }

        else if (Input.GetButtonDown("Hold"))
        {
            hold();
        }

        m_dragDirection = Direction.none;
        m_swipeDirection = Direction.none;
        m_didTap = false;
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

        //Using coroutine to delay the display of gameover panel after effect
        StartCoroutine(gameOverRoutine());

        playSound(m_soundManager.m_gameOverSound, 1f);
        playSound(m_soundManager.m_gameOverVocal, 1f);
    }

    void landShape()
    {
        if (m_activeShape)
        {
            // move the shape up, store it in the Board's grid array
            m_activeShape.moveUp();
            m_gameBoard.storeShapeInGrid(m_activeShape);

            m_activeShape.landShapeFX();

            if (m_ghost)
            {
                m_ghost.Reset();
            }

            if (m_holder)
            {
                m_holder.m_canRelease = true;
            }
            // spawn a new shape
            m_activeShape = m_spawner.spawnShape();

            // set all of the timeToNextKey variables to current time, so no input delay for the next spawned shape
            m_timeToNextKeyLR = Time.time;
            m_timeToNextKeyD = Time.time;
            m_timeToNextKeyRotate = Time.time;

            // remove completed rows from the board if we have any 
            m_gameBoard.StartCoroutine("clearAllRows");


            playSound(m_soundManager.m_dropSound,1f);

            if (m_gameBoard.m_completeRows > 0)
            {
                m_scoreManager.scoreLines(m_gameBoard.m_completeRows);

                if (m_scoreManager.m_didLevelUp)
                {
                    m_dropIntervalChanged = Mathf.Clamp(m_dropInterval - ((float)m_scoreManager.m_level * 0.05f), 0.05f, 1f);
                    playSound(m_soundManager.m_levelUpVocal,1f);
                }
                else
                {
                    if (m_gameBoard.m_completeRows > 1)
                    {
                        AudioClip randomVocal = m_soundManager.returnRandomClip(m_soundManager.m_exclaimationSounds);
                        playSound(randomVocal,1f);
                    }
                }

                playSound(m_soundManager.m_clearRowSound,1f);
            }
        }

    }

    public void restart()
    {
        //Debug.Log("Restarted");
        Time.timeScale = 1f;
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void toggleRotDirection()
    {
        m_clockwise = !m_clockwise;
        if (m_rotIconToggle)
        {
            m_rotIconToggle.iconToggle(m_clockwise);
        }
    }

    public void togglePause()
    {
        if (m_gameOver)
        {
            return;
        }
        m_isPaused = !m_isPaused;
        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);
            if (m_soundManager)
            {
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume*0.25f : m_soundManager.m_musicVolume;
            }
            Time.timeScale = (m_isPaused) ? 0 : 1;
        }
    }

    public void hold()
    {
        if (!m_holder)
        {
            return;
        }
        if (!m_holder.m_heldShape)
        {
            m_holder.catchShape(m_activeShape);
            m_activeShape = m_spawner.spawnShape();
            playSound(m_soundManager.m_holdSound, 1f);
        }
        else if (m_holder.m_canRelease)
        {
            Shape shape = m_activeShape;
            m_activeShape = m_holder.releaseShape();
            m_activeShape.transform.position = m_spawner.transform.position;
            m_holder.catchShape(shape);
            playSound(m_soundManager.m_holdSound, 1f);
        }
        else
        {
            Debug.LogWarning("Holder WARNING! Wait for cool down!");
            playSound(m_soundManager.m_errorSound, 1f);
        }

        // reset the Ghost every time we tap the Hold button
        if (m_ghost)
        {
            m_ghost.Reset();
        }
    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver || !m_soundManager||!m_scoreManager)
        {
            return;
        }
        playerInput();
    }

    void LateUpdate()
    {
        if (m_ghost)
        {
            m_ghost.DrawGhost(m_activeShape, m_gameBoard);
        }
    }

    //Effects of gameOver
    public ParticlePlayer m_gameOverFX;

    IEnumerator gameOverRoutine()
    {
        //Effects of gameOver
        if (m_gameOverFX)
        {
            m_gameOverFX.play();
        }

        yield return new WaitForSeconds(0.5f);

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }

    enum Direction { none, left, right, up, down }

    Direction m_dragDirection = Direction.none;
    Direction m_swipeDirection = Direction.none;

    float m_timeToNextDrag;
    float m_timeToNextSwipe;

    [Range(0.05f, 1f)]
    public float m_minTimeToDrag = 0.15f;

    [Range(0.05f, 1f)]
    public float m_minTimeToSwipe = 0.3f;

    bool m_didTap = false;

    private void OnEnable()
    {
        TouchController.dragEvent += dragHandler;
        TouchController.swipeEvent += swipeHandler;
        TouchController.tapEvent += tapHandler;
    }

    private void OnDisable()
    {
        TouchController.dragEvent -= dragHandler;
        TouchController.swipeEvent -= swipeHandler;
        TouchController.tapEvent -= tapHandler;
    }

    //public Text diagText;

    void dragHandler(Vector2 dragMovement)
    {
        /*if (diagText)
        {
            diagText.text = "SwipeEvent Detected";
        }*/
        m_dragDirection = getDirection(dragMovement);
    }

    void swipeHandler(Vector2 swipeMovement)
    {
        /*if (diagText)
        {
            diagText.text = "";
        }*/
        m_swipeDirection = getDirection(swipeMovement);
    }

    void tapHandler(Vector2 swipeMovement)
    {
        m_didTap = true;
    }

    Direction getDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.none;

        //hor
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }

        //ver
        else
        {
            swipeDir = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
        }
        return swipeDir;
    }
}

