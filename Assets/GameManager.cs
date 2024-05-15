using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerSpawn;
    public GameObject playerPrefab;
    private PlayerMovement playerInstance;
    private void Update()
    {
        if (playerInstance==null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                playerInstance = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity)
                    .GetComponent<PlayerMovement>();
                playerInstance.StartRecording();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                playerInstance.StopRecording();
                playerInstance.transform.position = playerSpawn.position;
                playerInstance.StartRecording();
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                playerInstance.ClearAllRecordings();
            }
        }
    }
}
