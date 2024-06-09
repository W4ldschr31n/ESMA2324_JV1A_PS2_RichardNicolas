using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public string mainMenuScene;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad[] objectsToDestroy =  GameObject.FindObjectsOfType<DontDestroyOnLoad>();
        foreach(DontDestroyOnLoad ddol in objectsToDestroy)
        {
            Destroy(ddol.gameObject);
        }
        StartCoroutine(LoadAfterDestroy());
    }


    private IEnumerator LoadAfterDestroy()
    {
        while (GameObject.FindObjectsOfType<DontDestroyOnLoad>().Length > 0)
        {
            yield return null;
        }
        SceneManager.LoadSceneAsync(mainMenuScene);
    }
}
