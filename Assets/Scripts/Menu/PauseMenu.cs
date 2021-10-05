using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject startBlackScreen;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI gameOverTimer;

    private bool isGameOver = false;

    private float time,seconds, minutes;

    // Update is called once per frame

    private void Start()
    {
        StartCoroutine(nameof(BlackScreen));
    }

    IEnumerator BlackScreen()
    {
        startBlackScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        startBlackScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
        if(!isGameOver)
            CountTime();
    }

    public void GameOver()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isGameOver = true;
        gameOverUI.SetActive(true);
    }

    private void CountTime()
    {
        time = Time.timeSinceLevelLoad;
        minutes = (int)(time / 60f);
        seconds = (int)(time % 60);
        timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        gameOverTimer.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void Retart()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<AudioLevelManager>().ChangeVolume(8);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindObjectOfType<AudioLevelManager>().ChangeVolume(-8);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
