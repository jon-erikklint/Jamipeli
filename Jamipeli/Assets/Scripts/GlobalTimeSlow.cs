using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimeSlow : TimeSlow {
    public float duration = 3;

    float startingTime = Mathf.NegativeInfinity;

    public Color effectColor = Color.yellow;
    public Sprite effectSprite;
    SpriteRenderer effectRenderer;
    Vector4 colorVector;
    Vector4 whiteVector;
    void Start()
    {
        Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();
        foreach (var rb in rbs)
        {
            if (rb.GetComponent<PlayerMover>() == null)
                Add(rb);
        }
        Creator creator = FindObjectOfType<Creator>();
        creator.Event += Add;
        GameObject worldSpriteRenderer = new GameObject();
        effectRenderer = worldSpriteRenderer.AddComponent<SpriteRenderer>();
        effectRenderer.sprite = effectSprite;
        effectRenderer.color = VectorColor.VectorToColor(whiteVector);
        colorVector = VectorColor.ColorToVector(effectColor);
        whiteVector = VectorColor.ColorToVector(new Color(1,1,1,0));
    }

    protected override void DoOnUpdate()
    {
        ColorEffect();
    }

    private void ColorEffect()
    {
        if (Active())
        {
            ChangeWorldColor(colorVector);
        }
        else
        {
            ChangeWorldColor(whiteVector);
        }
    }

    private void ChangeWorldColor(Vector4 target)
    {
        effectRenderer.color = VectorColor.VectorToColor(Vector4.Lerp(effectRenderer.color, target, Time.deltaTime/base.speedUpSpeed));
    }

    public void Add(Rigidbody2D rb)
    {
        slowFractions.Add(rb, new SlowData(true));
    }

    public override bool Active()
    {
        return Time.time - startingTime < duration;
    }

    public void Activate()
    {
        startingTime = Time.time;
    }

    private void OnDestroy()
    {
        Creator creator = FindObjectOfType<Creator>();
        if (creator != null)
            creator.Event -= Add;
    }
}
