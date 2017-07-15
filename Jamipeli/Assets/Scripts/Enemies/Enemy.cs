using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Dieable {

    public float speed;

    public Vector3 targetDisplacement { get { if (target == null) return Vector2.zero; return target.transform.position - transform.position; } }
    public float targetDistance { get { if (target == null) return int.MaxValue; return targetDisplacement.magnitude; } }

    private Rigidbody2D rb;
    private GunInterface gun;
    private GameObject player;

    private List<AIState> states;
    private AIState currentState;
    private int _currentIndex;
    public int currentIndex { get { return _currentIndex; } }

    public GameObject target;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.gun = (GunInterface) GetComponent(typeof(GunInterface));
        this.player = FindObjectOfType<PlayerMover>().gameObject;
    }

    void Update()
    {
        CheckStateChange();
        currentState.Update();
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

    public virtual void Move(Vector2 movement)
    {
        transform.right = movement;
        if (rb != null) rb.velocity = transform.right * speed;
    }

    public void Stop()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }

    public virtual void Turn(Vector2 direction)
    {
        transform.right = direction;
    }
    
    public virtual bool Shoot()
    {
        return gun.Shoot();
    }

    public virtual void Kill()
    {
        Destroy(this.gameObject);
    }
}
