using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeSlow : MonoBehaviour {
    public float slowFraction = 0.5f;
    public float slowDownSpeed = 1f;
    public float speedUpSpeed = 0.1f;

    protected Dictionary<Rigidbody2D, SlowData> slowFractions;

    void Awake()
    {
        slowFractions = new Dictionary<Rigidbody2D, SlowData>();
    }
    
    private void Update()
    {
        if (Active())
        {
            SlowDown();
        }
        else
        {
            SpeedUp();
        }

        DoOnUpdate();
    }

    protected virtual void DoOnUpdate() { }

    protected virtual void SlowDown()
    {
        float frac;
        if (slowDownSpeed > 0)
            frac = Time.deltaTime / slowDownSpeed;
        else frac = slowFraction;

        List<Rigidbody2D> notInside = new List<Rigidbody2D>();
        foreach (var rb in slowFractions.Keys)
        {
            if (!SlowDown(rb, frac))
            {
                SpeedUp(rb, frac * slowDownSpeed / speedUpSpeed);
                notInside.Add(rb);
            }
        }

        foreach (var rb in notInside)
        {
            if (!SpeedUp(rb, frac))
                slowFractions.Remove(rb);
        }
    }

    protected virtual void SpeedUp()
    {
        float frac;
        if (slowDownSpeed > 0)
            frac = Time.deltaTime / speedUpSpeed;
        else frac = slowFraction;

        List<Rigidbody2D> toBeRemoved = new List<Rigidbody2D>();
        foreach (var rb in slowFractions.Keys)
        {
            if (!SpeedUp(rb, frac))
                toBeRemoved.Add(rb);
        }

        foreach (var rb in toBeRemoved)
        {
            slowFractions.Remove(rb);
        }
    }

    // For higher performance assumes that slowFractions contains rb
    protected bool SlowDown(Rigidbody2D rb, float frac)
    {
        if (rb == null)
            return false;
        SlowData slowData = slowFractions[rb];
        if (slowData.isInside && slowData.slowFrac > slowFraction)
        {
            float frac_ = Mathf.Min(frac, slowData.slowFrac - slowFraction);
            rb.velocity *= 1 - frac_ / slowData.slowFrac;
            rb.angularVelocity *= 1 - frac_ / slowData.slowFrac;

            slowData.slowFrac -= frac_;

            foreach (var keeper in rb.GetComponents<SlowKeeper>())
                keeper.slowFactor = slowData.slowFrac;
            foreach (var keeper in rb.GetComponentsInChildren<SlowKeeper>())
                keeper.slowFactor = slowData.slowFrac;

            return true;
        }
        return slowData.isInside;
    }

    // For higher performance assumes that slowFractions contains rb
    protected bool SpeedUp(Rigidbody2D rb, float frac)
    {
        if (rb == null)
            return false;
        SlowData slowData = slowFractions[rb];
        if (slowData.slowFrac < 1)
        {
            float frac_ = Mathf.Min(frac, 1 - slowData.slowFrac);
            rb.velocity *= 1 + frac_ / slowData.slowFrac;
            rb.angularVelocity *= 1 + frac_ / slowData.slowFrac;

            slowData.slowFrac += frac;

            foreach (var keeper in rb.GetComponents<SlowKeeper>())
                keeper.slowFactor = slowData.slowFrac;
            foreach (var keeper in rb.GetComponentsInChildren<SlowKeeper>())
                keeper.slowFactor = slowData.slowFrac;

            return true;
        }
        return slowData.isInside;
    }

    public abstract bool Active();
}

public class SlowData
{
    public bool isInside { get; set; }
    public float slowFrac { get; set; }

    public SlowData(bool isInside)
    {
        this.isInside = isInside;
        this.slowFrac = 1f;
    }
}