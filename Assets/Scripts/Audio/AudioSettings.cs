using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        float musicValue, sfxValue;
        audioMixer.GetFloat("music", out musicValue);
        audioMixer.GetFloat("sfx", out sfxValue);
        musicSlider.value = ComputeSliderValue(musicValue);
        sfxSlider.value = ComputeSliderValue(sfxValue);
    }

    public void ChangeMusicVolume()
    {
        float volume = ComputeVolume(musicSlider.value);
        audioMixer.SetFloat("music", volume);
    }

    public void ChangeSfxVolume()
    {
        float volume = ComputeVolume(sfxSlider.value);
        audioMixer.SetFloat("sfx", volume);
    }

    private float ComputeVolume(float sliderValue)
    {
        if(sliderValue == 0)
        {
            return -80f; // Mute
        }
        else
        {
            return Mathf.Log10(Mathf.Max(0.001f, sliderValue)) * 20f;
        }
    }
    private float ComputeSliderValue(float volume)
    {
        if (volume <= -80f)
        {
            return 0f;
        }
        else
        {
            return Mathf.Pow(10, volume / 20f);
        }
    }
}
