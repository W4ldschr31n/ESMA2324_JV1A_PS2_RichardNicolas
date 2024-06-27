using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    private float startMusicVolume;
    public float fadeOutTime;
    public float fadeInTime;

    public AudioClip clickSound;
    public AudioClip deathSound;
    // Start is called before the first frame update
    void Start()
    {
        startMusicVolume = musicSource.volume;
    }

    public void ChangeMusic(AudioClip clip)
    {
        StartCoroutine(FadeOutIn(clip));
    }


    private IEnumerator FadeOutIn(AudioClip clip)
    {
        while(musicSource.volume > 0)
        {
            musicSource.volume -= startMusicVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }
        musicSource.clip = clip;
        musicSource.Play();
        while (musicSource.volume < startMusicVolume)
        {
            musicSource.volume += startMusicVolume * Time.deltaTime / fadeInTime;
            yield return null;
        }
    }

    public void PlayClickSound()
    {
        PlaySoundEffect(clickSound);
    }

    public void PlayDeathSound()
    {
        PlaySoundEffect(deathSound);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }


}
