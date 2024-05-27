using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform playerSpawn;
    public GameObject playerPrefab;
    private PlayerMovement playerInstance;
    public float timer;
    public GameObject promptText;
    private bool isPlaying;
    public string firstScene;
    private string nextScene;
    private bool finishedLevel;

    void Start()
    {
        // Do this in Start to let the event be initialized in an Awake
        TimerManager.onTimerEnded.AddListener(OnTimerEnded);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SingletonMaster.Instance.SceneChangeManager.LoadSceneWithFade(firstScene);
    }

    private void OnDisable()
    {
        TimerManager.onTimerEnded.RemoveListener(OnTimerEnded);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // Level Start
        if (!isPlaying && SingletonMaster.Instance.InputManager.JumpInputPressed)
        {
            if (!finishedLevel)
            {
                StartLevel();
            }
            else
            {
                GoNextLevel();
            }
        }
        else
        {
            if (SingletonMaster.Instance.InputManager.ResetInput)
            {
                DestroyPlayer();
                SingletonMaster.Instance.TimerManager.EndTimer();
                SingletonMaster.Instance.TimerManager.isPlaying = false;
                isPlaying = false;
                promptText.SetActive(true);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void StartLevel()
    {
        isPlaying = true;
        promptText.SetActive(false);
        DestroyPlayer();
        RespawnPlayer();
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

    }

    private void OnPlayerDeath()
    {
        isPlaying = false;
        promptText.SetActive(true);
        SingletonMaster.Instance.CameraManager.ZoomOut();
    }

    private void OnTimerEnded()
    {
        if (isPlaying)
        {
            RespawnPlayer();
        }
        else
            SingletonMaster.Instance.TimerManager.StartTimer(timer);
    }

    public void FinishGame(string _nextScene)
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
        nextScene = _nextScene;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // If we are loading a new level that is not the loading screen
        if(loadSceneMode == LoadSceneMode.Single && scene.name != SingletonMaster.Instance.SceneChangeManager.loadingScreenScene)
        {
            playerSpawn = GameObject.FindGameObjectWithTag("Respawn").transform;
            SingletonMaster.Instance.CameraManager.SetCameraTarget(playerSpawn);
            SingletonMaster.Instance.CameraManager.ZoomOut();
            isPlaying = false;
            finishedLevel = false;
            SingletonMaster.Instance.TimerManager.EndTimer();
            SingletonMaster.Instance.TimerManager.isPlaying = false;
            SingletonMaster.Instance.TimerManager.currentTimer = timer;
        }
    }
}
