using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : AIState {
    public Follow(Enemy enemy) : base(enemy) {}

    public override void Activate() {}

    public override void Update()
    {
        enemy.MoveTowardsTarget();
    }
}
