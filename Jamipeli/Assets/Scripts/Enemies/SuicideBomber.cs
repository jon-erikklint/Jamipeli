using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : Enemy {

    public float moveRadius = 5;
    public float triggerRadius = 1;
    public float triggerTime = 1;
    public float damageRadius = 1;
    public int damagePlayer = 2;
    public int damageEnemy = 3;

    bool isTriggered = false;

    public override bool IsActing()
    {
        return isTriggered;
    }

    public override void Act()
    {
        
    }

    public override bool IsMoving()
    {
        throw new NotImplementedException();
    }

    public override void Move()
    {
        throw new NotImplementedException();
    }

    public override void Kill()
    {
        throw new NotImplementedException();
    }
}
