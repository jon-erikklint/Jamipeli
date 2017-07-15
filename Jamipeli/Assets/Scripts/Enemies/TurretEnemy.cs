using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Enemy
{
    public override void CheckStateChange() { }

    private void Start()
    {
        SetStates(0, new Turret(this));
        TargetPlayer();
    }
}
