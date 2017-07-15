using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicHealth : Health {

    public float startHealth;
    public float maxHealth;
    protected float health;

    public override void DoOnAwake()
    {
        health = startHealth;
    }

    public override bool Damaging(float amount)
    {
        return Healing(-amount);
    }

    public override bool Healing(float amount)
    {
        if ((health >= maxHealth && amount > 0) || (health <= 0 && amount < 0))
            return false;
        health = Mathf.Min(health + amount, maxHealth);
        health = Math.Max(health, 0);
        return true;
    }

    override public float Amount()
    {
        return health;
    }

    override public bool IsEmpty()
    {
        return Amount() <= 0;
    }

    public override float Max()
    {
        return maxHealth;
    }
}
