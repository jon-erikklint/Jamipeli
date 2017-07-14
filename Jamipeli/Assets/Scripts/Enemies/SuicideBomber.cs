using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : Enemy {

    public float startMoveRadius = 5;
    public float continueMoveRadius = 7;
    public float speed = 5;
    public float triggerRadius = 1;
    public float triggerTime = 1;
    public float damageRadius = 1;
    public int damagePlayer = 2;
    public int damageEnemy = 3;

    public Transform follow;

    bool isMoving = false;
    bool isTriggered = false;

    public override bool IsActing()
    {
        return isTriggered;
    }

    public override void Act()
    {
        triggerTime -= Time.deltaTime;
        if (triggerTime < 0)
            Kill();
    }

    public override bool IsMoving()
    {
        return false;
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
