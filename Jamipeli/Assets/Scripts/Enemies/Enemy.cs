﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DoOnDestroy(Enemy died);

public abstract class Enemy : MonoBehaviour, Dieable, HasHealth
{

    public int points;
    public float maxHealth;
    public Color damageColor = Color.red;
    public float invincibilityTime = 0.2f;

    public float speed;
    public float acceleration;
    public float angularAcceleration;

    public Vector3 targetDisplacement { get { if (target == null) return Vector2.zero; return target.transform.position - transform.position; } }
    public float targetDistance { get { if (target == null) return int.MaxValue; return targetDisplacement.magnitude; } }

    private Rigidbody2D rb;
    private GunInterface gun;
    private GameObject player;

    private List<AIState> states;
    private AIState currentState;
    private int _currentIndex;
    public int currentIndex { get { return _currentIndex; } }

    public float speed_ { get { return speed * slowKeeper.slowFactor; } }
    public float acceleration_ { get { return acceleration * slowKeeper.slowFactor; } }
    public float angularAcceleration_ { get { return angularAcceleration * slowKeeper.slowFactor; } }
    public GameObject target;

    SlowKeeper slowKeeper;
    Health health;

    SpriteRenderer spriteRenderer;
    Vector4 damageColorVector;
    Vector4 colorVector;

    float lastHit;
    public bool isDead = false;

    public event DoOnDestroy Destroyed;
    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.gun = GetComponent<GunInterface>();
        this.player = FindObjectOfType<PlayerMover>().gameObject;
        health = new Health(maxHealth, this);
        this.slowKeeper = FindObjectOfType<SlowKeeper>();
        if (slowKeeper == null)
            slowKeeper = gameObject.AddComponent<SlowKeeper>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        damageColorVector = VectorColor.ColorToVector(damageColor);
        colorVector = VectorColor.ColorToVector(spriteRenderer.color);

        lastHit = Mathf.NegativeInfinity;
        DoOnAwake();
    }

    void Start()
    {
        DoOnStart();
    }

    void Update()
    {
        CheckStateChange();
        currentState.Update();
        DamageAnimation();
        DoOnUpdate();
    }

    protected virtual void DamageAnimation()
    {
        spriteRenderer.color = VectorColor.VectorToColor(Vector4.Lerp(spriteRenderer.color, colorVector, 2*Time.deltaTime/invincibilityTime));
    }

    public abstract void CheckStateChange();

    public void SetStates(int startIndex, params AIState[] states)
    {
        this.states = new List<AIState>();

        foreach (AIState state in states)
        {
            this.states.Add(state);
        }

        ChangeState(startIndex);
    }

    public void SetStates(int startIndex, List<AIState> states)
    {
        this.states = states;
        ChangeState(startIndex);
    }

    public void ChangeState(int state)
    {
        this.currentState = states[state];
        this._currentIndex = state;
        currentState.Activate();
    }

    public void TargetPlayer()
    {
        this.target = player;
    }

    public bool SeesTarget()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, targetDisplacement, targetDisplacement.magnitude);

        int acceptedHits = 2;
        if (!hits[0].collider.gameObject.Equals(this.gameObject))
        {
            acceptedHits--;
        }
        if(!hits[hits.Length - 1].collider.gameObject.Equals(this.target))
        {
            acceptedHits--;
        }

        return hits.Length <= acceptedHits;
    }

    public bool TargetInDistance(float distance)
    {
        return this.targetDistance <= distance;
    }

    public bool TargetVisibleAndClose(float distance)
    {
        return TargetInDistance(distance) && SeesTarget();
    }

    public void MoveTowardsTarget()
    {
        Move(targetDisplacement);
    }

    public void TurnToTarget()
    {
        Turn(targetDisplacement);
    }

    public virtual void Move(Vector2 movement)
    {
        transform.right = Vector2.Lerp(transform.right, movement, angularAcceleration_*Time.deltaTime);
        rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0, angularAcceleration_*Time.deltaTime);
        if (rb != null) rb.velocity = Vector2.Lerp(rb.velocity, transform.right * speed_, acceleration_*Time.deltaTime);
    }

    public void Stop()
    {

        if (rb != null)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, acceleration_*Time.deltaTime);
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0, angularAcceleration_*Time.deltaTime);
        }
    }

    public virtual void Turn(Vector2 direction)
    {
        transform.right = Vector2.Lerp(transform.right, direction.normalized, angularAcceleration_*Time.deltaTime);
    }
    
    public virtual bool Shoot(float angle = 0)
    {
        return gun.Shoot(angle);
    }

    public float Heal(float amount)
    {
        return health.Heal(amount);
    }
    public float Damage(float amount)
    {
        if (Time.time - lastHit < invincibilityTime)
            return 0;
        spriteRenderer.color = damageColor;
        lastHit = Time.deltaTime;
        return health.Damage(amount);
    }

    public virtual void Kill()
    {
        isDead = true;
        Destroy(this.gameObject);
    }

    public void AddDestroyListener(DoOnDestroy doIt)
    {
        Destroyed += doIt;
    }

    private void OnDestroy()
    {
        if(Destroyed != null)
            Destroyed(this);
        Destroyed = null;
    }

    public virtual void DoOnAwake() { }
    public virtual void DoOnStart() { }
    public virtual void DoOnUpdate() { }

}
