using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;


public class LoadingScreenManager : MonoBehaviour
{
    public Slider loadingBar;
    public GameObject prompt;
    public TextMeshProUGUI loreTextMeshPro;
    private LocalizeStringEvent localizeStringEvent;
    public bool isTextFullyDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        localizeStringEvent = GetComponent<LocalizeStringEvent>();
        prompt.SetActive(false);
        loadingBar.value = 0;
        loreTextMeshPro.text = "";
    }

    public void ForceTextDisplay()
    {
        StopAllCoroutines();
        loreTextMeshPro.maxVisibleCharacters = loreTextMeshPro.text.Length;
        isTextFullyDisplayed = true;
    }

    public void UpdateLoadingProgress(float value)
    {
        loadingBar.value = value;
    }

    public void SetLocalizationKey(string key)
    {
        localizeStringEvent.SetEntry(key);
    }

    public void FinishLoading()
    {
        prompt.SetActive(true);
    }

    /*
     * Called from the LocalizeStringEvent
     */
    public void DisplayText(string text)
    {
        // Reset display
        loreTextMeshPro.text = "";
        StopAllCoroutines();
        // Call the writing
        StartCoroutine(WriteText(text));
    }

    private IEnumerator WriteText(string text)
    {
        loreTextMeshPro.text = text;
        loreTextMeshPro.maxVisibleCharacters = 0;
        isTextFullyDisplayed = false;
        foreach (char c in text)
        {
            loreTextMeshPro.maxVisibleCharacters ++;
            switch(c)
            {
                // Pause time depending on the character revealed
                case '.':
                    yield return new WaitForSeconds(0.5f);
                    break;
                case ',':
                    yield return new WaitForSeconds(0.2f);
                    break;
                case ' ':
                    break;
                default:
                    yield return new WaitForSeconds(0.04f);
                    break;
            }
        }
        isTextFullyDisplayed = true;
        yield return null;
    }
}
