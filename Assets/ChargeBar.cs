using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public Activable activable;
    public Slider slider;
    public Image fillArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = activable.currentCharge / activable.maxCharge;
        fillArea.color = (slider.value == 1f) ? Color.yellow : Color.white;
    }
}
