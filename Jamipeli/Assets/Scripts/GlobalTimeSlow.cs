using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimeSlow : TimeSlow {
    public float duration = 3;

    float startingTime = Mathf.NegativeInfinity;

    public override bool Active()
    {
        return Time.time - startingTime < duration;
    }

    public void Activate()
    {
        startingTime = Time.time;
    }
}
