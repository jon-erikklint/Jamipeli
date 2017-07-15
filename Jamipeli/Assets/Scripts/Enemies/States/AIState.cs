using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState {

    protected Enemy enemy;

	public AIState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Activate();

    public abstract void Update();
}
