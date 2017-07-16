using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : Enemy {

    public float triggerRadius = 2;
    public float triggerTime = 4;
    public Color triggeredColor = Color.red;
    public float triggeredAnimationSpeedStart = 2;
    public float triggeredAnimationSpeedEnd = 5;
    public float speedUpAnimationStartTime = 2;

    public float damageRadius = 1;
    public float knockBack = 1;

    public int damagePlayer = 1;
    public int damageEnemy = 2;

    bool isMoving = false;
    bool isTriggered = false;

    void Start()
    {
        SetStates(0, new Exploder(this, triggerRadius, triggerTime, triggeredColor, triggeredAnimationSpeedStart, triggeredAnimationSpeedEnd, speedUpAnimationStartTime));
        TargetPlayer();
    }

    public override void CheckStateChange(){}

    public override void Kill()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject == this.gameObject)
                continue;
            Rigidbody2D rb = cols[i].GetComponent<Rigidbody2D>();
            if (rb == null)
                continue;
            Vector3 dir = (rb.transform.position - transform.position).normalized;
            rb.AddForce(dir*knockBack, ForceMode2D.Impulse);
            HasHealth health = rb.GetComponent<HasHealth>();
            if (health != null)
            {
                if (cols[i].tag == "Player")
                    health.Damage(damagePlayer);
                else
                    health.Damage(damageEnemy);
            }
        }

        Destroy(gameObject);
    }
}
