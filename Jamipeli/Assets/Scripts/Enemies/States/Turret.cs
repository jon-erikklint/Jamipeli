using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AIState
{
    public Turret(Enemy enemy) : base(enemy)
    {
    }

    public override void Activate()
    {
        enemy.TargetPlayer();
    }

    public override void Update()
    {
        enemy.Shoot();
        enemy.TurnToTarget();
        enemy.MoveTowardsTarget();
    }
}
