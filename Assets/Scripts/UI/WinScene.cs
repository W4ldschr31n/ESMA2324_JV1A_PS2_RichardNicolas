using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    private void Update()
    {
        if (SingletonMaster.Instance.InputManager.AnyInput)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
