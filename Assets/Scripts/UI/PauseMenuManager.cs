using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
