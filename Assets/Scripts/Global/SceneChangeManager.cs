using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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
        Slider loadingBar = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<Slider>();
        TextMeshProUGUI textPrompt = GameObject.FindGameObjectWithTag("LoadingPrompt").GetComponent<TextMeshProUGUI>();
        textPrompt.enabled = false;

        while (!asyncLoad.isDone)
        {
            // Update the loading bar
            loadingBar.value = asyncLoad.progress / 0.9f;
            if(asyncLoad.progress >= 0.9f)
            {
                // Show the player he can load the scene
                textPrompt.enabled = true;

                // Wait for the player to confirm the loading
                if (SingletonMaster.Instance.InputManager.AnyInput)
                {
                    asyncLoad.allowSceneActivation = true;
                }

            }
            yield return null;
        }

        // Fade out as the new scene is loaded
        fadeScreen.FadeOut();
    }
}
