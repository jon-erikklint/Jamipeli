using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DoOnTimeout();

public class Timer
{
    public string purpose;
    bool paused_ = false;
    bool running_ = false;
    bool hasEnded_ = false;

    float duration_ = -1;
    float timeLeft_ = 0;
    float speed_ = 1f;

    public bool paused { get { return paused_; } }
    public bool running { get { return running_; } }
    public bool hasEnded { get { return hasEnded_; } }

    public float duration { get { return duration_; } }
    public float timeLeft { get { return timeLeft_; } }
    public float speed { get { return speed_; } }

    public float timePassed { get { return duration - timeLeft; } }

    event DoOnTimeout actionList;

    private void Update()
    {
        if (running_ && !paused_)
        {
            timeLeft_ -= speed_ * Time.deltaTime;
            running_ = timeLeft_ > 0;
            if (!running_)
            {
                hasEnded_ = true;
                if (actionList != null)
                    actionList();
            }
        }
    }

    public bool StartTimer(float time, DoOnTimeout action = null, bool force = false)
    {
        if (running_ && !force)
            return false;

        if (action != null)
            actionList += action;

        running_ = true;
        hasEnded_ = false;
        duration_ = time;
        timeLeft_ = time;
        return true;
    }

    public void AddAction(DoOnTimeout action)
    {
        this.actionList += action;
    }

    public bool StopTimer()
    {
        if (!running_)
            return false;

        running_ = false;
        return true;
    }

    public bool Pause()
    {
        if (paused_)
            return false;

        paused_ = true;
        return true;
    }

    public bool Unpause()
    {
        if (!paused_)
            return false;

        paused_ = false;
        return true;
    }

}
