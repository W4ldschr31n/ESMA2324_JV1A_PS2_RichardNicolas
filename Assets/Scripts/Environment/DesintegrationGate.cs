using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesintegrationGate : MonoBehaviour
{
    public int playersNeeded;
    public DesintegrationObjects[] desintegrationsObjectsArray;
    public List<Activable> activablesList;

    private void Start()
    {
        TimerManager.onTimerStarted.AddListener(OnTimerStarted);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playersNeeded>0)
        {
            collision.GetComponent<PlayerMovement>().Die();
            playersNeeded--;
        }
    }

    private void OnTimerStarted()
    {
        // If we reached the target amount of players, activate the activables
        if(playersNeeded == 0)
        {
            foreach(Activable activable in activablesList)
            {
                activable.Activate();
            }
        }
        // Enable and disable objects according to remaining players (extra index for after the gate is solved)
        DesintegrationObjects desintegrationObjects = desintegrationsObjectsArray[^(playersNeeded+1)];
        foreach(GameObject go in desintegrationObjects.objectsToEnable)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in desintegrationObjects.objectsToDisable)
        {
            go.SetActive(false);
        }
    }

    private void OnValidate()
    {
        // Resize with an extra slot to account for what happens after the gate is solved
        Array.Resize(ref desintegrationsObjectsArray, playersNeeded + 1);
    }

    [System.Serializable]
    public class DesintegrationObjects
    {
        public List<GameObject> objectsToEnable = new List<GameObject>();
        public List<GameObject> objectsToDisable = new List<GameObject>();
    }
}
