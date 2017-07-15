using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTimeSlow : TimeSlow {

    public float radius = 2;
    public float minRadius = 1;
    public float maxRadius = 2.5f;
    public float radiusChangeSpeed = 0.5f;

    public float maxAlpha = 0.5f;
    public float alphaIncreaseTime = 0.05f;
    public float alphaDecreaseTime = 0.01f;

    SpriteRenderer spriteRenderer;
    protected bool isActive = false;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(radius, radius, 1);
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
    }

    protected override void SlowDown()
    {
        base.SlowDown();
        ChangeTransparency(maxAlpha * Time.deltaTime / alphaIncreaseTime);
    }

    protected override void SpeedUp()
    {
        base.SpeedUp();
        ChangeTransparency(-maxAlpha * Time.deltaTime / alphaDecreaseTime);
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

    public void ChangeRadius(float amount)
    {
        radius += radiusChangeSpeed*amount;
        radius = radius < maxRadius ? radius : maxRadius;
        radius = radius > minRadius ? radius : minRadius;
        transform.localScale = new Vector3(radius, radius, 1);
    }

    public override bool Active() { return isActive; }

    public void Activate() { isActive = true; }
    public void Deactivate() { isActive = false; }
    public bool ChangeActive()
    {
        isActive = !isActive;
        return isActive;
    }
}