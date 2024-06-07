using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DesintegrationGate : MonoBehaviour
{
    public int nbPlayersNeeded;
    public TextMeshProUGUI textCounter;
    public DesintegrationObjects[] desintegrationsObjectsArray;

    private void Start()
    {
        UpdateDisplay();
        UpdateGameObjects();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the current player touches the gate and we need to destroy more players
        if (collision.CompareTag("Player") && nbPlayersNeeded>0)
        {
            collision.GetComponent<PlayerMovement>().Die();
            nbPlayersNeeded--;
            UpdateDisplay();
            UpdateGameObjects();
        }
    }

    private void UpdateGameObjects()
    {
        // If we reached the target amount of players, destroy itself
        if(nbPlayersNeeded == 0)
        {
            Destroy(gameObject);
        }
        // Enable and disable objects according to remaining players (extra index for after the gate is solved)
        DesintegrationObjects desintegrationObjects = desintegrationsObjectsArray[^(nbPlayersNeeded+1)];
        foreach(GameObject go in desintegrationObjects.objectsToEnable)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in desintegrationObjects.objectsToDisable)
        {
            go.SetActive(false);
        }
    }

    private void UpdateDisplay()
    {
        textCounter.text = nbPlayersNeeded.ToString();
    }

    private void OnValidate()
    {
        // Resize with an extra slot to account for what happens after the gate is solved
        Array.Resize(ref desintegrationsObjectsArray, nbPlayersNeeded + 1);
    }

    [System.Serializable]
    public class DesintegrationObjects
    {
        public List<GameObject> objectsToEnable = new List<GameObject>();
        public List<GameObject> objectsToDisable = new List<GameObject>();
    }
}
