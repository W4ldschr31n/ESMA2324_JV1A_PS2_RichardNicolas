using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public static SceneChangeManager Instance;
    public string loadingScreenScene;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void LoadInitScene()
    {
        fadeScreen.FadeIn();
        SceneManager.LoadScene("Init");
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // Fade to black and load the loading screen
        fadeScreen.FadeIn();
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadingScreenScene);

        // Wait until the loading screen scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Load the real scene we want to get to
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        // Get the loading screen manager in the scene and tell him what to display
        LoadingScreenManager loadingScreenManager = FindObjectOfType<LoadingScreenManager>();
        loadingScreenManager.SetLocalizationKey(sceneName);

        while (!asyncLoad.isDone)
        {
            // Loading in progress
            if (asyncLoad.progress < 0.9f)
            {
                loadingScreenManager.UpdateLoadingProgress(asyncLoad.progress / 0.9f);
            }
            // Loading finished
            else
            {
                // Show the player he can load the scene
                loadingScreenManager.FinishLoading();

                // Wait for the player to confirm the loading after displaying the full text
                if (SingletonMaster.Instance.InputManager.AnyInput && loadingScreenManager.isTextFullyDisplayed)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            // Player can fast forward the text
            if (SingletonMaster.Instance.InputManager.AnyInput && !loadingScreenManager.isTextFullyDisplayed)
            {
                loadingScreenManager.ForceTextDisplay();
            }

            yield return null;
        }

        // Fade out as the new scene is loaded
        fadeScreen.FadeOut();
    }
}
