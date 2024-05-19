using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activable : MonoBehaviour
{
    public float currentCharge { get; private set; }
    public float maxCharge;
    public void AddCharge(float charge)
    {
        currentCharge = Mathf.Min(currentCharge + charge, maxCharge);
        if (currentCharge >= maxCharge)
        {
            Activate();
        }
    }
    public void DecreaseCharge(float charge)
    {
        if(currentCharge >= maxCharge)
        {
            Deactivate();
        }
        currentCharge = Mathf.Max(currentCharge - charge, 0f);
    }

    abstract public void Activate();
    abstract public void Deactivate();
}
