using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Resetable : MonoBehaviour
{
    // Virtual protected is required so inheriting classes can call it before their own Start
    virtual protected void Start()
    {
        TimerManager.onTimerStarted.AddListener(RestoreData);
        StoreData();
    }

    private void OnDisable()
    {
        TimerManager.onTimerStarted.RemoveListener(RestoreData);
    }

    abstract protected void StoreData();
    abstract protected void RestoreData();
}
