using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activator : MonoBehaviour
{
    public float chargeFactor = 1f;
    public Activable activable;
    abstract protected bool CheckCanActivate();
    abstract protected void UpdateDisplay();

    private void FixedUpdate()
    {
        if (CheckCanActivate())
        {
            activable.AddCharge(Time.fixedDeltaTime * chargeFactor);
        }
        else
        {
            activable.DecreaseCharge(Time.fixedDeltaTime * chargeFactor);
        }
    }

    private void Update()
    {
        UpdateDisplay();
    }
}
