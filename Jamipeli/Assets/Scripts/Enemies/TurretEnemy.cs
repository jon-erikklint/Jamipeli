using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Enemy
{
    public float shootDistance;

    private void Start()
    {
        SetStates(1, new Turret(this), new Follow(this));
        TargetPlayer();
    }

    public override void CheckStateChange() {
        if (currentIndex == 0 && !TargetInDistance(shootDistance))
        {
            ChangeState(1);
        }
        else if (currentIndex == 1 && TargetInDistance(shootDistance))
        {
            ChangeState(0);
        }
    }
}
