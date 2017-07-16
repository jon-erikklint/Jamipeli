using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour, GunInterface {

    public float hitRadius;
    public float hitFov;
    public float hitStrength;
    public float cooldown;

    public float drawTime;

    private float lastShot;
    private SpriteRenderer slashSprite;
    private Vector2 startRight;
    private bool fading;
    private float cosfov;

    void Start () {
        lastShot = float.MinValue;
        cosfov = Mathf.Cos(hitFov * Mathf.PI / 360);

        slashSprite = transform.Find("Slash").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (fading)
        {
            Color current = slashSprite.color;
            Color color = new Color(current.r, current.g, current.b, Math.Max(0, 1 - (Time.time - lastShot)/drawTime));
            slashSprite.color = color;
            if (color.a == 0) fading = false;
        }
        
    }

    public bool Shoot(float angle)
    {
        if (Time.time < lastShot + cooldown) return false;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, hitRadius);

        List<GameObject> toHit = new List<GameObject>();

        foreach(Collider2D hit in hits)
        {
            if (Vector2.Dot(VectorToObject(hit.gameObject), transform.right.normalized) >= cosfov && !hit.gameObject.Equals(gameObject) && !hit.gameObject.tag.Equals("Wall"))
            {
                toHit.Add(hit.gameObject);
            }
        }

        HitObjects(toHit);

        lastShot = Time.time;
        fading = true;

        return true;
    }

    private void HitObjects(List<GameObject> toHit)
    {
        foreach(GameObject o in toHit)
        {
            Hit(o);
        }
    }

    private void Hit(GameObject toHit)
    {
        Rigidbody2D rb = toHit.GetComponent<Rigidbody2D>();

        Vector2 hitForce = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - toHit.transform.position);
        hitForce = hitForce.normalized;
        hitForce *= Math.Max(rb.velocity.magnitude, hitStrength);
        
        rb.velocity = hitForce;

        Bullet bullet = toHit.GetComponent<Bullet>();
        if (bullet != null)
            bullet.MakeFatalForEnemies();
    }

    private Vector2 VectorToObject(GameObject o)
    {
        return (o.transform.position - transform.position).normalized;
    }
}
