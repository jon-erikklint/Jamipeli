using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstEnemy : Enemy
{
    public float burstAngle    = 90;
    public int   numOfBullets  = 9;
    public float burstInterval = 2;
    public float shootInterval = 0.1f;
    private void Start()
    {
        SetStates(0, new BurstShooter(this, burstInterval, burstAngle, shootInterval, numOfBullets));
        TargetPlayer();
    }

    public override void CheckStateChange()
    {
    }
}
