using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntHealth : Health
{
    new int maxHealth;
    new int health;

    public IntHealth(int maxHealth, Dieable dieable = null) : base(maxHealth, maxHealth, dieable) { }
    public IntHealth(int startHealth, int maxHealth, Dieable dieable = null) : base(startHealth, maxHealth, dieable) { }

    public sealed override float SetMaxHealth(float amount)
    {
        return (float)IntSetMaxHealth((int)Mathf.Round(amount));
    }

    sealed override public float Heal(float amount)
    {
        return IntHeal((int)Mathf.Round(amount));
    }

    sealed override public float Damage(float amount)
    {
        return IntDamage((int)Mathf.Round(amount));
    }

    public virtual int IntSetMaxHealth(int amount)
    {
        maxHealth = amount;
        int prev = health;
        health = health < maxHealth ? health : amount;
        return prev - health;
    }

    public virtual int IntHeal(int amount)
    {
        int next = health + amount;
        health = next < maxHealth ? next : maxHealth;
        return amount - (next - health);
    }
    public virtual int IntDamage(int amount)
    {
        int next = health - amount;
        Debug.Log(health + ", " + next);
        health = next > 0 ? next : 0;
        if (dieable != null && this.IsEmpty())
            dieable.Kill();
        return amount - (health - next);
    }
}
