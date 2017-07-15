using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicalIntHealth : IntHealth {

    public int startHealth;
    public int maxHealth;
    int health;

    public override bool IntDamaging(int amount)
    {
        return IntHealing(-amount);
    }

    public override bool IntHealing(int amount)
    {
        if ((health >= maxHealth && amount > 0) || (health <= 0 && amount < 0))
            return false;
        health = Math.Min(health + amount, maxHealth);
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
}
