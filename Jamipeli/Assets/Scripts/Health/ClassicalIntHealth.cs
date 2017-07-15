using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicalIntHealth : IntHealth {

    public int startHealth;
    public int maxHealth;
    private int health_;
    public int health { get { return health_; } }

    public override void DoOnAwake()
    {
        health_ = startHealth;
    }

    public override bool IntDamaging(int amount)
    {
        return IntHealing(-amount);
    }

    public override bool IntHealing(int amount)
    {
        if ((health_ >= maxHealth && amount > 0) || (health_ <= 0 && amount < 0))
            return false;
        health_ = Math.Min(health_ + amount, maxHealth);
        health_ = Math.Max(health_, 0);
        return true;
    }

    override public float Amount()
    {
        return health_;
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
