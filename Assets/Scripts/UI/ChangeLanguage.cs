using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
public class ChangeLanguage : MonoBehaviour
{
    private bool isEnglish;
    public Image flagImage;
    public Sprite englishFlag;
    public Sprite frenchFlag;

    private void OnEnable()
    {
        // Retrieve what locale is used when the screen appears
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            SetEnglishLocale();
        }
        else
        {
            SetFrenchLocale();
        }
    }

    public void SwitchLanguage()
    {
        if (isEnglish)
        {
            SetFrenchLocale();
        } else {
            SetEnglishLocale();
        }
    }

    public void SetEnglishLocale()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        flagImage.sprite = englishFlag;
        isEnglish = true;
    }

    public void SetFrenchLocale()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        flagImage.sprite = frenchFlag;
        isEnglish = false;
    }

}
