using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSpin : MonoBehaviour
{
    public float spinSpeed;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(new Vector3(0f, 0f, -spinSpeed*Time.deltaTime));
    }
}
