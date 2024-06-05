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

    private void LateUpdate()
    {
        // Exit early if not replaying
        if (!isReplaying)
            return;

        foreach(Recording recording in recordingList)
        {
            recording.ReplayNextData();
        }
    }

    public void RecordReplayData(ReplayData data)
    {
        recordingQueue.Enqueue(data);
    }

    public void StartReplay()
    {
        // Create replay with current record
        isReplaying = true;
        AddCurrentRecording();
        // TODO only the last recording needs to be called
        foreach(Recording recording in recordingList)
        {
            recording.CreateReplayPlayer(replayPlayerPrefab);
        }
    }

    public void RestartReplay()
    {
        // Restart every replay from the beginning
        isReplaying = true;
        foreach (Recording recording in recordingList)
        {
            recording.ReplayFromBeginning();
        }
    }

    public void Clear()
    {
        // Delete every replay and stop replaying
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
        // Save current record and start a new one
        recordingList.Add(new Recording(recordingQueue));
        recordingQueue.Clear();
    }

    public void CancelCurrentRecording()
    {
        recordingQueue.Clear();
    }

    public void StartNewRecording()
    {
        AddCurrentRecording();
    }
}
