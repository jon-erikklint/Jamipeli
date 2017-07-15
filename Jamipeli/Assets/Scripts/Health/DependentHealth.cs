using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependentHealth : Health {

    public Health dependentHealth;

    public override void DoOnAwake()
    {
        if (dependentHealth == null)
        {
            Debug.LogWarning("Object " + gameObject.name + ": Health not specified!");
            this.enabled = false;
        }
        dependentHealth.DoOnAwake();
    }
    public override void DoOnStart()
    {
        if (dependentHealth == null)
        {
            Debug.LogWarning("Object " + gameObject.name + ": Health not specified!");
            this.enabled = false;
        }
        dependentHealth.DoOnStart();
    }
    public override void DoOnUpdate()
    {
        dependentHealth.DoOnUpdate();
    }

    public override float Amount()
    {
        return dependentHealth.Amount();
    }

    public override bool Damaging(float amount)
    {
        return dependentHealth.Damaging(amount);
    }

    public override bool Healing(float amount)
    {
        return dependentHealth.Healing(amount);
    }

    public override bool IsEmpty()
    {
        return dependentHealth.IsEmpty();
    }

    public override float Max()
    {
        throw new NotImplementedException();
    }
}
