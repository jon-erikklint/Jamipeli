using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour {

    public float slowFraction = 0.5f;
    public float slowSpeed = 1f;
    Dictionary<Rigidbody2D, SlowData> slowFractions;
    bool isActive = false;
    // Use this for initialization
 
    void Initialize () {
        slowFractions = new Dictionary<Rigidbody2D, SlowData>();
	}

    private void Update()
    {
        if (isActive)
            SlowDown();
        else
            SpeedUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && collision.tag != "player") {
            if (slowFractions.ContainsKey(rb))
                slowFractions[rb].isInside = true;
            else {
                slowFractions.Add(rb, new SlowData(true));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && slowFractions.ContainsKey(rb))
            slowFractions[rb].isInside = false;
    }

    void SlowDown()
    {
        float frac = Time.deltaTime / slowSpeed;
        List<Rigidbody2D> notInside = new List<Rigidbody2D>();
        foreach (var rb in slowFractions.Keys)
        {
            if (!SlowDown(rb, frac))
                SpeedUp(rb, frac);
            else
                notInside.Add(rb);
        }

        foreach (var rb in notInside)
        {
            if (!SpeedUp(rb, frac))
                slowFractions.Remove(rb);
        }
    }

    void SpeedUp() {
        float frac = Time.deltaTime / slowSpeed;
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
    bool SlowDown(Rigidbody2D rb, float frac) {
        SlowData slowData = slowFractions[rb];
        if (slowData.isInside && slowData.slowFraction > slowFraction)
        {
            rb.velocity *= 1 - slowFraction * (1 - frac) / frac;
            slowFraction -= frac;
            return true;
        }
        return slowData.isInside;
    }

    // For higher performance assumes that slowFractions contains rb
    bool SpeedUp(Rigidbody2D rb, float frac) {
        SlowData slowData = slowFractions[rb];
        if (slowData.slowFraction < 1)
        {
            rb.velocity *= 1 + slowFraction * (1 + frac) / frac;
            slowFraction += frac;
            return true;
        }
        return slowData.isInside;
    }

    public void Activate() { isActive = true; }
    public void Deactivate() { isActive = false; }
    public bool ChangeActive()
    {
        isActive = !isActive;
        return isActive;
    }
}

class SlowData
{
    public bool isInside { get; set; }
    public float slowFraction { get; set; }

    public SlowData(bool isInside) {
        this.isInside = isInside;
        this.slowFraction = 1f;
    }
}