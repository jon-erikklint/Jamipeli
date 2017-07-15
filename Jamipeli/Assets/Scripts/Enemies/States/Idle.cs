using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIState
{
    public Idle(Enemy enemy) : base(enemy) {}

    public override void Activate()
    {
        enemy.Stop();
    }

    public override void Update()
    {
        enemy.Stop();
    }
}
