using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Components;

public class GameManager : MonoBehaviour
{
    public Transform playerSpawn;
    public GameObject playerPrefab;
    private PlayerMovement playerInstance;
    public float timer;
    public int maxLives;
    private int currentLives;
    public GameObject promptText;
    public LocalizeStringEvent localizedPromptText;
    private bool isPlaying, isInLoadingScreen, isGameOver;
    public string firstScene;
    private string nextScene;
    public string pauseScene;
    public string endScene;
    private bool isLoreTransition;
    private bool finishedLevel;
    public bool isGamePaused;
    public GameObject mainUI;
    public GameObject gameOverScreen;
    public TextMeshProUGUI livesText;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TimerManager.onTimerEnded.AddListener(OnTimerEnded);
        SingletonMaster.Instance.SceneChangeManager.LoadScene(firstScene, true);
        HideMainUI();
        isInLoadingScreen = true;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        TimerManager.onTimerEnded.RemoveListener(OnTimerEnded);

    }

    private void Update()
    {
        CheatCodes();
        if (SingletonMaster.Instance.InputManager.PauseInput)
        {
            SwitchPauseMenu();
        }
        // Don't listen for further inputs if game is paused
        if (isGamePaused)
        {
            return;
        }
        //GameOver
        if(isGameOver && SingletonMaster.Instance.InputManager.JumpInputPressed) {
            ResetLevel();
        }
        // Level Start
        else if (!isInLoadingScreen && !isPlaying && SingletonMaster.Instance.InputManager.JumpInputPressed)
        {
            if (!finishedLevel)
            {
                RespawnPlayer();
            }
            else
            {
                GoNextLevel();
            }
        }
        else // During gameplay
        {
            if (SingletonMaster.Instance.InputManager.ResetInput)
            {
                ResetLevel();
            }
        }
    }

    private void CheatCodes()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
                SingletonMaster.Instance.SceneChangeManager.LoadScene("Level1-1", true);
            else if (Input.GetKeyDown(KeyCode.Keypad2))
                SingletonMaster.Instance.SceneChangeManager.LoadScene("Level2-1", true);
            else if (Input.GetKeyDown(KeyCode.Keypad3))
                SingletonMaster.Instance.SceneChangeManager.LoadScene("Level3-1", true);
            else if (Input.GetKeyDown(KeyCode.KeypadPlus))
                GoNextLevel();
        }
    }

    private void ResetLevel()
    {
        DestroyPlayer();
        SingletonMaster.Instance.TimerManager.EndTimer();
        SingletonMaster.Instance.TimerManager.isPlaying = false;
        isPlaying = false;
        ShowStartPrompt();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoNextLevel()
    {
        SingletonMaster.Instance.SceneChangeManager.LoadScene(nextScene, isLoreTransition);
    }

    private void ShowStartPrompt()
    {
        localizedPromptText.SetEntry("pak_start");
        promptText.SetActive(true);
    }

    private void ShowRestartPrompt()
    {
        localizedPromptText.SetEntry("pak_restart");
        promptText.SetActive(true);
    }

    private void ShowContinuePrompt()
    {
        localizedPromptText.SetEntry("press_any_key");
        promptText.SetActive(true);
    }

    private void DestroyPlayer()
    {
        if(playerInstance != null)
        {
            playerInstance.ClearAllRecordings();
            Destroy(playerInstance.gameObject);
            playerInstance = null;
        }
    }

    private void RespawnPlayer()
    {
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity)
                .GetComponent<PlayerMovement>();
            playerInstance.EnableAndShow();
            playerInstance.onPlayerDeath.AddListener(OnPlayerDeath);
        }
        else
        {
            playerInstance.StopRecording();
            playerInstance.transform.position = playerSpawn.position;
            playerInstance.Resurrect();
            playerInstance.RestartReplay();
        }
        playerInstance.StartRecording();
        SingletonMaster.Instance.CameraManager.SetCameraTarget(playerInstance.transform);
        SingletonMaster.Instance.CameraManager.ResetZoom();
        SingletonMaster.Instance.TimerManager.StartTimer(timer);
        isPlaying = true;
        promptText.SetActive(false);
    }

    private void OnTimerEnded()
    {
        if (!finishedLevel)
        {
            playerInstance.Die();
        }
        else
        {
            playerInstance.RestartReplay();
            SingletonMaster.Instance.TimerManager.StartTimer(timer);
        }
    }

    private void OnPlayerDeath()
    {
        isPlaying = false;
        ShowRestartPrompt();
        SingletonMaster.Instance.CameraManager.ZoomOut();
        currentLives--;
        UpdateDisplayLives();
        if (currentLives == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        HideMainUI();
        ShowGameOverScreen();
    }

    public void FinishGame()
    {
        isPlaying = false;
        finishedLevel = true;
        playerInstance.DisableAndHide();
        playerInstance.StopRecording();
        SingletonMaster.Instance.TimerManager.StartTimer(timer);
        playerInstance.RestartReplay();
        ShowContinuePrompt();
        SingletonMaster.Instance.CameraManager.SetCameraTarget(playerInstance.GetWinningReplay().transform);
        SingletonMaster.Instance.CameraManager.ZoomOut();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == SingletonMaster.Instance.SceneChangeManager.loadingScreenScene || scene.name == endScene)
        {
            HideMainUI();
            isInLoadingScreen = true;
        }
        // If we are loading a new level
        else if(loadSceneMode == LoadSceneMode.Single)
        {
            // Setup game data
            playerSpawn = GameObject.FindGameObjectWithTag("Respawn").transform;
            isInLoadingScreen = false;
            isPlaying = false;
            finishedLevel = false;
            isGameOver=false;

            // Load data from the level
            LevelData levelData = FindObjectOfType<LevelData>();
            if (levelData)
            {
                timer = levelData.timer;
                maxLives = levelData.lives;
                nextScene = levelData.nextLevel;
                isLoreTransition = levelData.isLoreTransition;
            }
            // Setup the singletons
            SingletonMaster.Instance.CameraManager.ZoomOut();
            SingletonMaster.Instance.CameraManager.SetCameraTarget(playerSpawn);
            SingletonMaster.Instance.TimerManager.isPlaying = false;
            SingletonMaster.Instance.TimerManager.currentTimer = timer;

            // Display UI
            currentLives = maxLives;
            UpdateDisplayLives();
            ShowMainUI();
            HideGameOverScreen();
            ShowStartPrompt();
        }
    }

    private void UpdateDisplayLives()
    {
        livesText.text = $"{currentLives}";
    }

    private void HideMainUI()
    {
        mainUI.SetActive(false);
    }

    private void ShowMainUI()
    {
        mainUI.SetActive(true);
    }

    private void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    private void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }

    public void SwitchPauseMenu()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            SceneManager.UnloadSceneAsync(pauseScene);
            Time.timeScale = 1f;
        }
        else
        {
            isGamePaused = true;
            SceneManager.LoadScene(pauseScene, LoadSceneMode.Additive);
            Time.timeScale = 0f;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(endScene);
    }
}
