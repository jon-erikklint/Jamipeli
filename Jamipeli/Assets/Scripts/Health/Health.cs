using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health {

    public float maxHealth;
    public float health;
    protected Dieable dieable;

    public Health(float maxHealth, Dieable dieable = null) : this(maxHealth, maxHealth, dieable) { }

    public Health(float startHealth, float maxHealth, Dieable dieable = null)
    {
        this.maxHealth = maxHealth;
        this.health = startHealth;
        this.dieable = dieable;
    }

    public virtual float Amount() { return health; }
    public virtual float Max() { return maxHealth; }

    public float Ratio()
    {
        return Amount() / Max();
    }

    public float IncreaseMaxHealth(float amount)
    {
        return SetMaxHealth(maxHealth + amount);
    }

    public virtual float SetMaxHealth(float amount)
    {
        maxHealth = amount;
        float prev = health;
        health = health < maxHealth ? health : amount;
        return prev - health;
    }

    public virtual float Heal(float amount)
    {
        float next = health + amount;
        health = next < maxHealth ? next : maxHealth;
        return amount - (next - health);
    }

    public virtual float Damage(float amount)
    {
        float next = health - amount;
        health = next > 0 ? next : 0;
        if (dieable != null && this.IsEmpty())
            dieable.Kill();
        return amount - (health - next);
    }
    public virtual bool IsEmpty() { return Amount() <= 0; }
    public virtual bool IsFull() { return Amount() >= Max(); }

}
