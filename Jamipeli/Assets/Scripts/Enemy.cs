using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Dieable {

    bool wasMoving = false;
    bool wasActing = false;
	// Update is called once per frame
	void Update () {
        bool moves = IsMoving();
        bool acts = IsActing();

        if (wasMoving != moves) {
            if (moves)
                StartMoving();
            else
                StopMoving();
            wasMoving = moves;
        }

        if (wasActing != acts)
        {
            if (acts)
                StartActing();
            else
                StopActing();
            wasActing = acts;
        }


        if (moves)
            Move();
        else
            Idle();

        if (acts)
            Act();
	}

    public abstract bool IsMoving();
    public virtual void StartMoving() { }
    public virtual void StopMoving() { }
    public abstract void Move();
    public virtual void Idle()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector3.zero;
    }

    public abstract bool IsActing();
    public virtual void StartActing() { }
    public virtual void StopActing() { }
    public abstract void Act();

    public abstract void Kill();
}
