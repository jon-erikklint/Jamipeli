using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBoss : Enemy{

    public float burstInterval = 2;
    public float burstAngle = 90;
    public float shootInterval = 0.02f;
    public int numOfBullets = 10;

    int count;
    float lastChange;

    public override void CheckStateChange()
    {
        if(Time.time - lastChange > burstInterval+shootInterval + Time.deltaTime)
        {
            ChangeState(RandomInt(count));
        }
    }

    // Use this for initialization
    public override void DoOnAwake()
    {
        Gun[] guns = gameObject.GetComponents<Gun>();
        List<AIState> states = new List<AIState>();
        foreach(Gun gun in guns)
        {
            states.Add(new BurstShooter(this, burstInterval, burstAngle, shootInterval, numOfBullets));
        }
        count = states.Count;
        SetStates(RandomInt(count), states);
        lastChange = Time.time;
        TargetPlayer();
    }

    private int RandomInt(int max)
    {
        return Mathf.FloorToInt(UnityEngine.Random.value * max);
    }
}
