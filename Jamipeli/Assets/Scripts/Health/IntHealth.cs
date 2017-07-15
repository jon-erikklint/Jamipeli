using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IntHealth : Health
{
    override public sealed bool Healing(float amount)
    {
        return IntHealing((int)Mathf.Round(amount));
    }

    public sealed override bool Damaging(float amount)
    {
        return IntDamaging((int)Mathf.Round(amount));
    }

    public abstract bool IntHealing(int amount);
    public abstract bool IntDamaging(int amount);

}
