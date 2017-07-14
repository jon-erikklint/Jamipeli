using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour {

    public float slowFraction = 0.5f;
    public float slowSpeed = 1f;
    public float speedUpSpeed = 0.1f;

    public float radius = 1;
    public float maxAlpha = 0.5f;
    public float alphaIncreaseTime = 0.05f;
    public float alphaDecreaseTime = 0.01f;

    SpriteRenderer spriteRenderer;
    Dictionary<Rigidbody2D, SlowData> slowFractions;
    bool isActive = false;
    // Use this for initialization
 
    void Awake () {
        slowFractions = new Dictionary<Rigidbody2D, SlowData>();
	}

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(radius, radius, 1);
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
    }

    private void Update()
    {
        if (isActive)
        {
            SlowDown();
            ChangeTransparency(maxAlpha*Time.deltaTime/alphaIncreaseTime);
        }
        else
        {
            SpeedUp();
            ChangeTransparency(-maxAlpha * Time.deltaTime / alphaDecreaseTime);
        }
    }

    void ChangeTransparency(float amount)
    {
        Color color = spriteRenderer.color;
        if ( (amount < maxAlpha && color.a > 0) || (amount > 0 && color.a < maxAlpha)) {
            color.a += amount;
            color.a = Mathf.Min(color.a, maxAlpha);
            color.a = Mathf.Max(color.a, 0);
            spriteRenderer.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && collision.tag != "Player") {
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
        {
            slowFractions[rb].isInside = false;
        }
    }

    void SlowDown()
    {
        float frac;
        if (slowSpeed > 0)
            frac = Time.deltaTime / slowSpeed;
        else frac = slowFraction;

        List<Rigidbody2D> notInside = new List<Rigidbody2D>();
        foreach (var rb in slowFractions.Keys)
        {
            if (!SlowDown(rb, frac))
            {
                SpeedUp(rb, frac*slowSpeed/speedUpSpeed);
                notInside.Add(rb);
            }
        }

        foreach (var rb in notInside)
        {
            if (!SpeedUp(rb, frac))
                slowFractions.Remove(rb);
        }
    }

    void SpeedUp()
    {
        float frac;
        if (slowSpeed > 0)
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
    bool SlowDown(Rigidbody2D rb, float frac) {
        SlowData slowData = slowFractions[rb];
        if (slowData.isInside && slowData.slowFrac > slowFraction)
        {
            float frac_ = Mathf.Min(frac, slowData.slowFrac - slowFraction);
            rb.velocity *= 1 - frac_/slowData.slowFrac;
            rb.angularVelocity *= 1 - frac_ / slowData.slowFrac;
            slowData.slowFrac -= frac_;
            return true;
        }
        return slowData.isInside;
    }

    // For higher performance assumes that slowFractions contains rb
    bool SpeedUp(Rigidbody2D rb, float frac) {
        SlowData slowData = slowFractions[rb];
        if (slowData.slowFrac < 1)
        {
            float frac_ = Mathf.Min(frac, 1-slowData.slowFrac);
            rb.velocity *= 1 + frac_/slowData.slowFrac;
            rb.angularVelocity *= 1 + frac_ / slowData.slowFrac;
            slowData.slowFrac += frac;
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
    public float slowFrac { get; set; }

    public SlowData(bool isInside) {
        this.isInside = isInside;
        this.slowFrac = 1f;
    }
}