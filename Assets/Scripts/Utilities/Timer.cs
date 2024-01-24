using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    private float time;
    private float timer;
    public bool timerDone;

    public event Action OnTimerDone;

    public Timer(float timeToComplete)
    {
        time = timeToComplete;
    }
    public Timer(float timeToComplete,  Action onTimerDone)
    {
        time = timeToComplete;
        OnTimerDone = onTimerDone;
    }

    private void Update()
    {
        if (timer < Time.time && !timerDone)
        {
            timerDone = true;
            OnTimerDone?.Invoke();
        }
    }

    public void StartTimer()
    {
        timer = Time.time + time;
        timerDone = false;
    }
}
