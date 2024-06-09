using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    EventSystem eventSystem;
    public GameObject firstSelectedItem;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(firstSelectedItem);
    }

    public void OnResumeButton()
    {
        SingletonMaster.Instance.GameManager.SwitchPauseMenu();
    }

    public void OnExitButton()
    {
        SingletonMaster.Instance.GameManager.BackToMainMenu();
    }
}
