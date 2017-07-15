using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour, Dieable, HasHealth {

    private Rigidbody2D rb;
    private Camera c;
    private LocalTimeSlow locSlow;
    private GlobalTimeSlow globSlow;
    private GunInterface gun;

    public float playerSpeed;
    public int globalSlows = 1;
    public float teleportCooldown = 0.5f;

    private float lastTeleport = Mathf.NegativeInfinity;
    private Vector3 mousePosition { get { return c.ScreenToWorldPoint(Input.mousePosition); } }

    public Health health;
    public Health slowCharge;
    public int playerHealth = 3;
    public float maxSlowTime = 150;
    public float slowtimeFromHealth;

    private GameManager gameManager;

    private void Awake()
    {
        this.slowCharge = new Health(maxSlowTime);
        this.health = new Health(playerHealth, this);
    }

    void Start () {
        gameManager = FindObjectOfType<GameManager>();
        this.rb = GetComponent<Rigidbody2D>();
        this.c = Camera.main;
        this.locSlow = GetComponentInChildren<LocalTimeSlow>();
        this.globSlow = GetComponentInChildren<GlobalTimeSlow>();
        this.gun = GetComponent<GunInterface>();
	}
	
	// Update is called once per frame
	void Update () {
        this.SetSpeed();
        this.SetRotation();
        this.CheckShoot();
        this.CheckLocalSlow();
        this.CheckGlobalSlow();
        this.CheckSlowRadius();
    }

    private void CheckShoot()
    {
        if (Input.GetMouseButton(0))
        {
            this.gun.Shoot();
        }
    }

    private void CheckGlobalSlow()
    {
        if (globalSlows > 0 && Input.GetKeyDown(KeyCode.Space) && !globSlow.Active())
        {
            globSlow.Activate();
            locSlow.Deactivate();
            globalSlows--;
        }
    }

    private void CheckLocalSlow()
    {
        if(globSlow.Active())
        {
            if (Time.time - lastTeleport > teleportCooldown && Input.GetMouseButtonDown(1))
            {
                Teleport(mousePosition);
                lastTeleport = Time.time;
            }
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            locSlow.Activate();
        }

        if (Input.GetMouseButtonUp(1))
        {
            locSlow.Deactivate();
        }
    }

    private void CheckSlowRadius()
    {
        locSlow.ChangeRadius(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void SetSpeed()
    {
        Vector2 movement = Vector2.zero;

        movement.x = playerSpeed * Input.GetAxis("Horizontal");
        movement.y = playerSpeed * Input.GetAxis("Vertical");

        rb.velocity = movement;
        rb.angularVelocity = 0;
    }

    private void SetRotation()
    {
        Vector2 worldLocation = mousePosition;
        transform.eulerAngles = new Vector3(0, 0, AngleInDeg(transform.position, worldLocation));
    }

    private void Teleport(Vector2 position)
    {
        transform.position = position;
    }

    private float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    private float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }

    public float Heal(float amount)
    {
        float healed = health.Heal(amount);
        slowCharge.IncreaseMaxHealth(-slowtimeFromHealth * healed);
        slowCharge.Damage(slowtimeFromHealth * healed);
        return healed;
    }

    public float Damage(float amount)
    {
        float damaged = health.Damage(amount);
        slowCharge.IncreaseMaxHealth(slowtimeFromHealth * damaged);
        slowCharge.Heal(slowtimeFromHealth * damaged);
        return damaged;
    }

    public void Kill()
    {
        gameManager.EndGame();
    }
}
