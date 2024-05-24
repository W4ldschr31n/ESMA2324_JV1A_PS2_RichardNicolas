using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    MainMenuManager mainMenuManager;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            mainMenuManager.HideTitleScreen();
        }
    }
}