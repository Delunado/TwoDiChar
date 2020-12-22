using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Timer
{
    public enum TimerType
    {
        TIMER_UPDATE,
        TIMER_FIXED_UPDATE
    }

    private TimerType timerType;

    private float timeToWait;
    private float timer;

    private bool started;
    private bool finished;
    public bool Finished { get => finished; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeToWait">The time to wait for the timer</param>
    public Timer(float timeToWait, TimerType timerType = TimerType.TIMER_UPDATE)
    {
        this.timerType = timerType;
        this.timeToWait = timeToWait;
        timer = 0f;
        started = false;
        finished = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (started)
        {
            if (timer < timeToWait)
            {
                if (timerType == TimerType.TIMER_UPDATE)
                    timer += Time.deltaTime;
                else
                    timer += Time.fixedDeltaTime;
            }
            else
            {
                finished = true;
                started = false;
            }
        }
    }

    /// <summary>
    /// Start the timer
    /// </summary>
    public void Start()
    {
        started = true;
    }

    /// <summary>
    /// Stop the timer
    /// </summary>
    public void Stop()
    {
        started = false;
    }

    /// <summary>
    /// Reset the timer. You can use it again with Start.
    /// </summary>
    public void Reset()
    {
        timer = 0f;
        finished = false;
    }

    /// <summary>
    /// Restart the timer. Start again from the beginning.
    /// </summary>
    public void Restart()
    {
        timer = 0f;
        finished = false;
        started = true;
    }

    /// <summary>
    /// Reset the timer, setting a new Time to Wait. You can use it again with Start.
    /// </summary>
    /// <param name="newTimeToWait"></param>
    public void Reset(float newTimeToWait)
    {
        timeToWait = newTimeToWait;
        Reset();
    }

    public float Value()
    {
        return timeToWait - timer;
    }
}
