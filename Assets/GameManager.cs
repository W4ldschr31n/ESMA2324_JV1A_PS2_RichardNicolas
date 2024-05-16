using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerSpawn;
    public GameObject playerPrefab;
    private PlayerMovement playerInstance;
    public TimerManager timerManager;
    public float timer;
    public GameObject promptText;
    private bool isPlaying;

    void Start()
    {
        // Do this in Start to let the event be initialized in an Awake
        TimerManager.onTimerEnded.AddListener(OnTimerEnded);
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
                RespawnPlayer();
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                playerInstance.ClearAllRecordings();
                RespawnPlayer();
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
        }
        playerInstance.EnableAndShow();
        playerInstance.StartRecording();
        timerManager.StartTimer(timer);
    }

    private void OnTimerEnded()
    {
        if(isPlaying)
            RespawnPlayer();
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
    }
}
