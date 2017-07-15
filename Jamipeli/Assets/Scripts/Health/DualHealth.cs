using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualHealth : Health{

    new public bool kills { get { return health1.kills || health2.kills; } }
    public Health health1;
    public Health health2;

    public override bool Damaging(float amount)
    {
        bool success = health1.Damaging(amount);
        if(success)
            health2.Healing(amount);
        return success;
    }

    public override bool Healing(float amount)
    {
        bool success = health1.Healing(amount);
        if(success)
            health2.Damaging(amount);
        return success;
    }

    public override float Amount()
    {
        if (health1.Amount() + health2.Amount() == 0) return -1;
        return health1.Amount() / (health1.Amount() + health2.Amount());
    }

    public override bool IsEmpty()
    {
        return health1.IsEmpty() && health2.IsEmpty();
    }

    public override float Max()
    {
        throw new NotImplementedException();
    }
}
