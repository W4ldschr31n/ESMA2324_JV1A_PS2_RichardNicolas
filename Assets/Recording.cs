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
        replayQueue = new(originalQueue);
    }

    public bool ReplayNextData()
    {
        bool hasMoreData = false;
        if(replayQueue.Count > 0)
        {
            ReplayData data = replayQueue.Dequeue();
            player.SetReplayData(data);
            hasMoreData = true;
        }

        return hasMoreData;
    }

    public void CreateReplayPlayer(GameObject replayPlayerPrefab)
    {
        if(replayQueue.Count > 0)
        {
            ReplayData firstData = replayQueue.Peek();
            player = GameObject.Instantiate(replayPlayerPrefab, firstData.position, Quaternion.identity)
                .GetComponent<ReplayPlayer>();
        }
    }

    public void DestroyReplayPlayer()
    {
        if (player != null)
        {
            GameObject.Destroy(player.gameObject);
        }
    }
}
