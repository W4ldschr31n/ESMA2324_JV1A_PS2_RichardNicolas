using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    MainMenuManager mainMenuManager;
    void Update()
    {
        if (Input.anyKey)
        {
            mainMenuManager.HideTitleScreen();
        }
    }
}