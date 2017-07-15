using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimeSlow : TimeSlow {
    public float duration = 3;

    float startingTime = Mathf.NegativeInfinity;

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
        Debug.Log("Ny hidastuu");
        startingTime = Time.time;
    }

    private void OnDestroy()
    {
        Creator creator = FindObjectOfType<Creator>();
        if (creator != null)
            creator.Event -= Add;
    }
}
