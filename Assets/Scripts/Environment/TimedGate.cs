using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimedGate : MonoBehaviour
{
    public float timer;
    private float currentTimer;
    public Door door;
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        TimerManager.onTimerStarted.AddListener(OnTimerStarted);
    }

    private void Update()
    {
        if (currentTimer > 0f)
        {
            currentTimer = Mathf.Max(0f, currentTimer - Time.deltaTime);
            if (currentTimer <= 0f)
            {
                door.Deactivate();
            }
        }
        timerText.text = $"{currentTimer:F2}";
    }

    private void OnTimerStarted()
    {
        door.Activate();
        currentTimer = timer;
    }
}
