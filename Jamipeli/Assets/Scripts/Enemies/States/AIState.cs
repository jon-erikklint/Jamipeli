using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState {

    protected Enemy enemy;

	public AIState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Activate() { }
    public virtual void Deactivate() { }
    public virtual void Update() { }
}
