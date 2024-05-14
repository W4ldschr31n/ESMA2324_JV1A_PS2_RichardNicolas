using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public Queue<ReplayData> recordingQueue;
    public GameObject replayPlayerPrefab;
    public bool isReplaying;
    private Recording recording;

    private void Awake()
    {
        recordingQueue = new Queue<ReplayData>();
    }

    private void Update()
    {
        // Exit early if not replaying
        if (!isReplaying)
            return;

        bool hasMoreData = recording.ReplayNextData();
        if (!hasMoreData)
            RestartReplay();
    }

    public void RecordReplayData(ReplayData data)
    {
        recordingQueue.Enqueue(data);
    }

    public void StartReplay()
    {
        isReplaying = true;
        recording = new Recording(recordingQueue);
        recordingQueue.Clear();
        recording.CreateReplayPlayer(replayPlayerPrefab);
    }

    public void RestartReplay()
    {
        isReplaying = true;
        recording.ReplayFromBeginning();
    }


    private void Reset()
    {
        isReplaying = false;
        recordingQueue.Clear();
        recording.DestroyReplayPlayer();
        recording = null;
    }
}
