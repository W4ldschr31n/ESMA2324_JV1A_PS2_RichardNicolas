using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTimer;
    public bool isPlaying;

    public static UnityEvent onTimerStarted;
    public static UnityEvent onTimerEnded;

    private void Awake()
    {
        if (onTimerStarted == null)
            onTimerStarted = new UnityEvent();
        if (onTimerEnded == null)
            onTimerEnded = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            currentTimer = Mathf.Max(currentTimer - Time.deltaTime, 0f);
            if(currentTimer == 0f)
            {
                isPlaying = false;
                onTimerEnded?.Invoke();
            }
        }
        // Display time left with 2 decimals
        timerText.text = $"{currentTimer:F2}";
    }

    public void StartTimer(float time)
    {
        // Inititalize time variables
        currentTimer = time;
        isPlaying = true;
        onTimerStarted?.Invoke();
    }

    public void EndTimer()
    {
        currentTimer = 0f;
    }
}
