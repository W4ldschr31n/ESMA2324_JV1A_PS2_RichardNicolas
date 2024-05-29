using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform playerSpawn;
    public GameObject playerPrefab;
    private PlayerMovement playerInstance;
    public float timer;
    public int maxLives;
    private int currentLives;
    public GameObject promptText;
    private bool isPlaying, isInLoadingScreen, isGameOver;
    public string firstScene;
    private string nextScene;
    private bool finishedLevel;
    public GameObject mainUI;
    public GameObject gameOverScreen;
    public TextMeshProUGUI livesText;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SingletonMaster.Instance.SceneChangeManager.LoadSceneWithFade(firstScene);
        HideMainUI();
        isInLoadingScreen = true;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
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

    private void ResetLevel()
    {
        DestroyPlayer();
        SingletonMaster.Instance.TimerManager.EndTimer();
        SingletonMaster.Instance.TimerManager.isPlaying = false;
        isPlaying = false;
        promptText.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoNextLevel()
    {
        SingletonMaster.Instance.SceneChangeManager.LoadSceneWithFade(nextScene);
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

    private void OnPlayerDeath()
    {
        isPlaying = false;
        promptText.SetActive(true);
        SingletonMaster.Instance.CameraManager.ZoomOut();
        currentLives--;
        UpdateDisplayLives();
        if(currentLives == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void FinishGame()
    {
        isPlaying = false;
        finishedLevel = true;
        playerInstance.DisableAndHide();
        playerInstance.StopRecording();
        SingletonMaster.Instance.TimerManager.StartTimer(timer);
        playerInstance.RestartReplay();
        promptText.SetActive(true);
        SingletonMaster.Instance.CameraManager.SetCameraTarget(playerSpawn);
        SingletonMaster.Instance.CameraManager.ZoomOut();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == SingletonMaster.Instance.SceneChangeManager.loadingScreenScene)
        {
            HideMainUI();
            isInLoadingScreen = true;
        }
        // If we are loading a new level that is not the loading screen
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
        }
    }

    private void UpdateDisplayLives()
    {
        livesText.text = $"Lives left\n{currentLives}";
    }

    private void HideMainUI()
    {
        mainUI.SetActive(false);
    }

    private void ShowMainUI()
    {
        mainUI.SetActive(true);
    }
}
