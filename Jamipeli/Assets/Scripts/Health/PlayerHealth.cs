using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : IntHealth{

    Health slowCharge;
    float slowtimeForHealth;

    public PlayerHealth(int startHealth, int maxHealth, float slowtimeForHealth, Health slowCharge, Dieable dieable = null) 
        : base(startHealth, maxHealth, dieable)
    {
        this.slowCharge = slowCharge;
        this.slowtimeForHealth = slowtimeForHealth;
    }

    public PlayerHealth(int maxHealth, float slowtimeForHealth, Health slowCharge, Dieable dieable = null) 
        : this(maxHealth, maxHealth, slowtimeForHealth, slowCharge, dieable) { }

    public override int IntHeal(int amount)
    {
        int healed = base.IntHeal(amount);
        slowCharge.IncreaseMaxHealth(-slowtimeForHealth * healed);
        slowCharge.Damage(-slowtimeForHealth * healed);
        return healed;
    }
    public override int IntDamage(int amount)
    {
        int damaged = base.IntDamage(amount);
        slowCharge.IncreaseMaxHealth(slowtimeForHealth * damaged);
        slowCharge.Heal(slowtimeForHealth * damaged);
        return damaged;
    }
}
