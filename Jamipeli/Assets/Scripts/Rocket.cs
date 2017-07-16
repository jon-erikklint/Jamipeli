using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Explosive
{
    new public float triggerRadius { get { return 0; } }
    new public float triggerTime { get { return 0; } }

    new public Color triggeredColor { get { return Color.red; } }
    new public float triggeredAnimationSpeedStart { get { return 0; } }
    new public float triggeredAnimationSpeedEnd { get { return 0; } }
    new public float speedUpAnimationStartTime { get { return 0; } }

    public float speed = 1;
    public float acceleration = 1;
    public float angularAcceleration = 1;

    public float safeTime = 0.5f;

    float creationTime;
    Vector3 targetDisplacement { get { if (target == null) return Vector2.zero; return target.transform.position - transform.position; } }
    float targetDistance { get { if (target == null) return int.MaxValue; return targetDisplacement.magnitude; } }

    Rigidbody2D rb;
    Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<PlayerMover>().transform;
        creationTime = Time.time;
    }

    private void Update()
    {
        Move();
    }

    public bool SeesTarget()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, targetDisplacement, targetDisplacement.magnitude);

        int acceptedHits = 2;
        if (!hits[0].collider.gameObject.Equals(this.gameObject))
        {
            acceptedHits--;
        }
        if (!hits[hits.Length - 1].collider.gameObject.Equals(this.target))
        {
            acceptedHits--;
        }

        return hits.Length <= acceptedHits;
    }

    private void Move()
    {
        if (SeesTarget())
            Move(transform.right);
        else
            Move(targetDisplacement);
    }

    public virtual void Move(Vector2 movement)
    {
        transform.right = Vector2.Lerp(transform.right, movement, angularAcceleration * Time.deltaTime);

        if (rb != null) {
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0, angularAcceleration*Time.deltaTime); ;
            rb.velocity = Vector2.Lerp(rb.velocity, transform.right * speed, acceleration * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsSafe(collision.gameObject))
            return;
        Explode(null);
    }

    bool IsSafe(GameObject obj)
    {
        if (Time.time - creationTime > safeTime)
        {
            Bullet bullet = obj.GetComponent<Bullet>();
            return bullet != null && !bullet.IsFatalForEnemies();
        }
        return true;
    }

}
