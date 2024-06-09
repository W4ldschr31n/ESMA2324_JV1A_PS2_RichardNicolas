using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        SingletonMaster.Instance.AudioManager.ChangeMusic(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
