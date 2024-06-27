using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
        musicSlider.value = 0.5f;
        sfxSlider.value = 0.5f;
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
        float volume = Mathf.Log10(Mathf.Max(0.01f, sliderValue)) * 20;
        return volume;
    }
}
