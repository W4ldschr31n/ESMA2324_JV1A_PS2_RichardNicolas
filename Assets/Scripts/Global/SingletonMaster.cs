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
    public SceneChangeManager SceneChangeManager { get; private set; }


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
        // The SceneChangeManager exists within another game object to be in the main menu
        SceneChangeManager = FindObjectOfType<SceneChangeManager>();
    }
}
