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

    private void OnEnable()
    {
        TimerManager.onTimerEnded.AddListener(OnTimerEnded);
    }

    private void OnDisable()
    {
        TimerManager.onTimerEnded.RemoveListener(OnTimerEnded);
    }

    private void Update()
    {
        if (playerInstance==null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
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
        playerInstance.StartRecording();
        timerManager.StartTimer(timer);
    }

    private void OnTimerEnded()
    {
        RespawnPlayer();
    }
}
