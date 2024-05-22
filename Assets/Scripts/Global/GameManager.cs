using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform playerSpawn;
    public GameObject playerPrefab;
    private PlayerMovement playerInstance;
    public TimerManager timerManager;
    public CameraManager cameraManager;
    public float timer;
    public GameObject promptText;
    private bool isPlaying;
    public string sceneToPlay;

    void Start()
    {
        // Do this in Start to let the event be initialized in an Awake
        TimerManager.onTimerEnded.AddListener(OnTimerEnded);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneToPlay);
    }

    private void OnDisable()
    {
        TimerManager.onTimerEnded.RemoveListener(OnTimerEnded);
    }

    private void Update()
    {
        // Level Start
        if (!isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isPlaying = true;
                promptText.SetActive(false);
                DestroyPlayer();
                RespawnPlayer();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                timerManager.currentTimer = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                playerInstance.ClearAllRecordings();
                timerManager.currentTimer = 0f;
            }
        }
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
        }
        else
        {
            playerInstance.StopRecording();
            playerInstance.transform.position = playerSpawn.position;
            playerInstance.RestartReplay();
        }
        playerInstance.EnableAndShow();
        playerInstance.StartRecording();
        cameraManager.SetCameraTarget(playerInstance.transform);
        cameraManager.ResetZoom();
        timerManager.StartTimer(timer);

    }

    private void OnTimerEnded()
    {
        if (isPlaying)
        {
            RespawnPlayer();
        }
        else
            timerManager.StartTimer(timer);
    }

    public void FinishGame()
    {
        isPlaying = false;
        playerInstance.DisableAndHide();
        playerInstance.StopRecording();
        timerManager.StartTimer(timer);
        playerInstance.RestartReplay();
        promptText.SetActive(true);
        cameraManager.SetCameraTarget(playerSpawn);
        cameraManager.ZoomOut();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(loadSceneMode == LoadSceneMode.Single)
        {
            playerSpawn = GameObject.FindGameObjectWithTag("Respawn").transform;
            cameraManager.SetCameraTarget(playerSpawn);
            cameraManager.ZoomOut();
        }
    }
}
