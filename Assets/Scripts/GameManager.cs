using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Score Settings")]
    private float score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Timer Settings")]
    public float levelTime = 300f;
    private float currTime;
    public TextMeshProUGUI timerText;
    public bool timerActive = true;

    [Header("Checkpoint Settings")]
    public Vector3 lastCheckpointPos;
    public bool hasCheckpoint = false;
    public TextMeshProUGUI messageText;

    // Game Manager Singleton
    public static GameManager Gary;
    public string currentLevel;

    public GameObject objectToMove;
    public Vector3 targetPosition;
    public Vector3 resetPosition;

    void Awake()
    {
        // check for an existing singleton
        if (Gary)
        {
            // a singleton already exists
            Destroy(this.gameObject);
        }
        else
        {
            // no singleton exists
            Gary = this; // singleton definition
            DontDestroyOnLoad(this.gameObject); // keep this alive in a pocket scene
        }
    }

    private void Start()
    {
        StartANewGame();
    }

    private void StartANewGame()
    {
        // stop the music, if still playing
        SoundManager.Steve.StopTheMusic();

        currTime = levelTime;
        timerActive = true;
        UpdateTimer();
        UpdateScoreUI();
    }

    public void LevelStarted()
    {
        // go to the get ready state
        StartCoroutine(GetReady());
    }

    public void FactoryStarted()
    {
        // go to the get ready state
        StartCoroutine(GetFactoryReady());
    }

    public void BossStarted()
    {
        // go to the get ready state
        StartCoroutine(GetBossReady());
    }

    void Update()
    {
        // Press R to restart the current scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (timerActive)
        {
            if (currTime > 0)
            {
                currTime -= Time.deltaTime; // decrease time
                UpdateTimer();
            }
            else
            {
                currTime = 0;
                timerActive = false;
                TimerEnd();
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void LoseScore()
    {
        score = Mathf.Ceil(score/2);
        UpdateScoreUI();
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE\n" + score;
    }

    public void RestartGame()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return null;

        StartANewGame();
    }

    void UpdateTimer()
    {
        if (timerText != null)
        {
            if (currTime > 0)
            {
                int timeDisplay = Mathf.FloorToInt(currTime); //Apparenty this exists
                timerText.text = string.Format("TIME\n" + timeDisplay);
            } else
            {
                timerText.text = string.Format("TIME\n0");
            }
        }
    }

    void TimerEnd()
    {
        Debug.Log("Timer finished! TOO BAD! BWAHAHAHAHAHAHAHAAHHHAAAAhhhaa....heh...");
        DestroyAllEnemies();
        objectToMove.transform.position = targetPosition;
    }

    public IEnumerator GetReady()
    {

        // is this the first time we are in this level? 
        if (currentLevel != LevelManager.Larry.levelName)
        {
            messageText.enabled = true;
            messageText.text = LevelManager.Larry.levelName;

            yield return new WaitForSeconds(1f);

            // update the current level
            currentLevel = LevelManager.Larry.levelName;
        }

        // set the message
        messageText.enabled = true;
        messageText.text = "ONWARDS!!!";

        // start the music
        SoundManager.Steve.StartTheMusic();

        // wait for 3 seconds
        yield return new WaitForSeconds(1.0f);

        // turn off the messages
        messageText.enabled = false;

    }

    public IEnumerator GetFactoryReady()
    {

        // is this the first time we are in this level? 
        if (currentLevel != LevelManager.Larry.levelName)
        {
            messageText.enabled = true;
            messageText.text = LevelManager.Larry.levelName;

            yield return new WaitForSeconds(1f);

            // update the current level
            currentLevel = LevelManager.Larry.levelName;
        }

        // set the message
        messageText.enabled = true;
        messageText.text = "UPWARDS!!!";

        // start the music
        SoundManager.Steve.StartTheFactory();

        // wait for 3 seconds
        yield return new WaitForSeconds(1.0f);

        // turn off the messages
        messageText.enabled = false;

    }

    public IEnumerator GetBossReady()
    {

        // is this the first time we are in this level? 
        if (currentLevel != LevelManager.Larry.levelName)
        {
            messageText.enabled = true;
            messageText.text = LevelManager.Larry.levelName;

            yield return new WaitForSeconds(1f);

            // update the current level
            currentLevel = LevelManager.Larry.levelName;
        }

        // set the message
        messageText.enabled = true;
        messageText.text = "SHOWDOWN TIME!!!";

        // start the music
        SoundManager.Steve.StartTheBossMusic();

        // wait for 3 seconds
        yield return new WaitForSeconds(1.0f);

        // turn off the messages
        messageText.enabled = false;

    }

    public void OverText()
    {
        messageText.enabled = true;
        messageText.text = "Too BAD!";

    }

    public void WinText()
    {
        messageText.enabled = true;
        messageText.text = "CONGRATS!!!";

    }

    public void LevelWin()
    {
        StartCoroutine(Vitory());
    }

    public IEnumerator Vitory()
    {
        SoundManager.Steve.StopTheMusic();

        yield return new WaitForSeconds(2f);

        // time to go to the next level
        LevelManager.Larry.GoToNextLevel();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetLevel();
    }
    private void ResetLevel()
    {
        currTime = levelTime;
        timerActive = true;
        UpdateTimer();

        objectToMove.transform.position = resetPosition;
    }
    public void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] threats = GameObject.FindGameObjectsWithTag("Threat");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach (GameObject threat in threats)
        {
            Destroy(threat);
        }
    }

}