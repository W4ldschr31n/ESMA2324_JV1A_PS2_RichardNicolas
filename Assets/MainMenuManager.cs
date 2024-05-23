using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // External components
    [SerializeField]
    GameObject titleScreen;

    [SerializeField]
    private GameObject mainMenu, settingsMenu, controlsMenu;

    [SerializeField]
    private GameObject mainMenuFirstElement, settingsMenuFirstElement, controlsMenuFirstElement;

    private void Start()
    {
        HideAllMenus();
    }

    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
        // Show the menu after a few frames to avoid hitting the play button instantly
        Invoke(nameof(ShowMainMenu), 0.5f);
    }

    private void HideAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAllMenus();
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstElement);
    }

    public void ShowSettingsMenu()
    {
        HideAllMenus();
        settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstElement);
    }

    public void ShowControlsMenu()
    {
        HideAllMenus();
        controlsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(controlsMenuFirstElement);
    }

    public void QuitButton()
    {
        // Quitting method is different between play mode and build
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void PlayGame()
    {
        StartCoroutine(LoadYourAsyncScene());//SceneManager.LoadSceneAsync("Init");
    }

    private IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Init");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("WAIT");
            yield return null;
        }
        Debug.Log("ALLO");
    }
}
