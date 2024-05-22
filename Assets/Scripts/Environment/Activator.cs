using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activator : MonoBehaviour
{
    public float chargeFactor = 1f;
    public List<Activable> listActivablesOn = new List<Activable>();
    public List<Activable> listActivablesOff = new List<Activable>();
    abstract protected bool CheckCanActivate();
    abstract protected void UpdateDisplay();

    private void FixedUpdate()
    {
        if (CheckCanActivate())
        {
            foreach (Activable activable in listActivablesOn)
            {
                activable.AddCharge(Time.fixedDeltaTime * chargeFactor);
            }
            foreach (Activable activable in listActivablesOff)
            {
                activable.DecreaseCharge(Time.fixedDeltaTime * chargeFactor);
            }
        }
        else
        {
            foreach (Activable activable in listActivablesOff)
            {
                activable.AddCharge(Time.fixedDeltaTime * chargeFactor);
            }
            foreach (Activable activable in listActivablesOn)
            {
                activable.DecreaseCharge(Time.fixedDeltaTime * chargeFactor);
            }
        }
    }

    private void Update()
    {
        UpdateDisplay();
    }
}
