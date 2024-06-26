using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording
{
    public ReplayPlayer player;
    private Queue<ReplayData> originalQueue;
    private Queue<ReplayData> replayQueue;

    public Recording(Queue<ReplayData> _originalQueue)
    {
        // Create new queues from the given queue
        originalQueue = new(_originalQueue);
        replayQueue= new(_originalQueue);
    }

    public void ReplayFromBeginning()
    {
        // Replace current replay queue with original queue
        replayQueue = new(originalQueue);
    }

    public void ReplayNextData()
    {
        // Check if there is more data to play
        if(replayQueue.Count > 0)
        {
            ReplayData data = replayQueue.Dequeue();
            player.SetReplayData(data);
        } else if (player != null)
        {
            player.Hide();
        }
    }

    public void CreateReplayPlayer(GameObject replayPlayerPrefab)
    {
        // Create a replay player if needed
        if(replayQueue.Count > 0 && player == null)
        {
            ReplayData firstData = replayQueue.Peek();
            player = GameObject.Instantiate(replayPlayerPrefab, firstData.position, Quaternion.identity)
                .GetComponent<ReplayPlayer>();
        }
    }

    public void DestroyReplayPlayer()
    {
        // Destroy the replay player if needed
        if (player != null)
        {
            player.DestroySelf();
        }
    }
}
