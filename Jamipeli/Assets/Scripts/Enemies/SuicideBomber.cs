using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : Enemy {

    public float startMoveRadius = 2;
    public float continueMoveRadius = 4;

    public float speed = 1;

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

    public Transform target;

    bool isMoving = false;
    bool isTriggered = false;

    public Vector3 targetDisplacement { get { return target.transform.position - transform.position; } }
    public float   sqrDistFromTarget  { get { return targetDisplacement.sqrMagnitude; } }
    public float   distFromTarget     { get { return targetDisplacement.magnitude; } }

    Vector4 triggeredColorVector;
    Vector4 colorVector;
    SpriteRenderer sRenderer;
    Timer triggerTimer;

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        if(sRenderer == null)
            Debug.LogWarning("Object " + gameObject.name + ": SpriteRenderer not found, trigger animation won't work!");
        else
            colorVector = ColorToVector(GetComponent<SpriteRenderer>().color);
        
        triggeredColorVector = ColorToVector(triggeredColor);

        if (target == null)
            target = FindObjectOfType<PlayerMover>().transform;

        triggerTimer = new Timer();
        triggerTimer.AddAction(new DoOnTimeout(Kill));
        triggerTimer.purpose = "Trigger timer";
    }

    public override bool IsActing()
    {
        isTriggered = isTriggered || sqrDistFromTarget < triggerRadius * triggerRadius;
        return isTriggered;
    }

    public override void Act() {
        triggerTimer.StartTimer(triggerTime);
        if (sRenderer != null)
            Animate();
    }

    void Animate()
    {
        float timePassed = triggerTimer.timePassed;
        float cosArg;
        if (timePassed > speedUpAnimationStartTime)
            cosArg = 2 * Mathf.PI * (speedUpAnimationStartTime * triggeredAnimationSpeedStart + (speedUpAnimationStartTime - timePassed) * triggeredAnimationSpeedEnd);
        else
            cosArg = 2 * Mathf.PI * timePassed * triggeredAnimationSpeedStart;

        float fraction = 1 - Mathf.Cos(cosArg);
        Debug.Log(fraction);
        Vector4 newColorVector = colorVector + (triggeredColorVector - colorVector) * fraction;
        sRenderer.color = VectorToColor(newColorVector);
    }

    public override bool IsMoving()
    {
        isMoving = isTriggered || sqrDistFromTarget < startMoveRadius*startMoveRadius || (isMoving && sqrDistFromTarget < continueMoveRadius);
        return isMoving;
    }

    public override void Move()
    {
        transform.right = targetDisplacement;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = transform.right * speed;
    }

    public override void Kill()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        for (int i = 0; i < cols.Length; i++)
        {
            Rigidbody2D rb = cols[i].GetComponent<Rigidbody2D>();
            if (rb == null)
                continue;
            Vector3 dir = (rb.transform.position - transform.position).normalized;
            rb.AddForce(dir*knockBack, ForceMode2D.Impulse);
            Health health = rb.GetComponent<Health>();
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

    Color VectorToColor(Vector4 vec)
    {
        return new Color(vec.x, vec.y, vec.z, vec.w);
    }

    Vector4 ColorToVector(Color col)
    {
        return new Vector4(col.r, col.g, col.b, col.a);
    }
}
