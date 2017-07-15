using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegeneratingHealth : DependentHealth {
    
    public float waitTime;
    public float regenerationSpeed;

    float lastHit;

    public override void DoOnStart()
    {
        base.DoOnStart();
        lastHit = Time.time;
    }

    public override void DoOnUpdate()
    {
        if (Time.time - lastHit > waitTime) {
            dependentHealth.Heal(regenerationSpeed*Time.deltaTime);
        }
    }

    public override bool Damaging(float amount)
    {
        lastHit = Time.time;
        return dependentHealth.Damaging(amount);
    }

}
