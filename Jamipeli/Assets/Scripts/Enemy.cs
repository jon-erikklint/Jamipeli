using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Dieable {
	
	// Update is called once per frame
	void Update () {
        if (IsMoving())
            Move();

        if (IsActing())
            Act();
	}

    public abstract bool IsMoving();
    public abstract void Move();

    public abstract bool IsActing();
    public abstract void Act();

    public abstract void Kill();
}
