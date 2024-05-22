using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonMaster : MonoBehaviour
{
    public static SingletonMaster Instance { get; private set; }

    public CameraManager CameraManager { get; private set; }
    public TimerManager TimerManager { get; private set; }
    public GameManager GameManager { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        CameraManager = GetComponent<CameraManager>();
        TimerManager = GetComponent<TimerManager>();
        GameManager = GetComponent<GameManager>();
    }
}
