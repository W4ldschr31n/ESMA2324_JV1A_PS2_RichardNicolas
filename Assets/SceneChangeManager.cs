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
    

    public void LoadSceneWithFade(string sceneName)
    {
        
        StartCoroutine(LoadYourAsyncScene(sceneName));
        

    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        fadeScreen.FadeIn();
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadingScreenScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        Slider loadingBar = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<Slider>();
        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress / 0.9f;
            yield return null;
        }
        // afficher % chargement
        fadeScreen.FadeOut();
    }
}
