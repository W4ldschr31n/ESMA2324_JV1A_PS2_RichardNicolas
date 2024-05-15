using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public Queue<ReplayData> recordingQueue;
    public GameObject replayPlayerPrefab;
    public bool isReplaying;
    private List<Recording> recordingList;

    private void Awake()
    {
        recordingQueue = new Queue<ReplayData>();
        recordingList = new List<Recording>();
    }

    private void Update()
    {
        // Exit early if not replaying
        if (!isReplaying)
            return;

        bool hasMoreData = false;
        foreach(Recording recording in recordingList)
        {
            hasMoreData |= recording.ReplayNextData();
        }
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
        AddCurrentRecording();
        foreach(Recording recording in recordingList)
        {
            recording.CreateReplayPlayer(replayPlayerPrefab);
        }
        // TODO camera follow
    }

    public void RestartReplay()
    {
        isReplaying = true;
        foreach (Recording recording in recordingList)
        {
            recording.ReplayFromBeginning();
        }
    }

    public void Clear()
    {
        isReplaying = false;
        recordingQueue.Clear();
        foreach (Recording recording in recordingList)
        {
            recording.DestroyReplayPlayer();
        }
        recordingList.Clear();
    }

    public void AddCurrentRecording()
    {
        recordingList.Add(new Recording(recordingQueue));
        recordingQueue.Clear();
    }

    public void StartNewRecording()
    {
        AddCurrentRecording();
    }
}
