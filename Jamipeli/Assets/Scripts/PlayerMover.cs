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
    public Color damageColor;
    public float invincibilityTime = 0.5f;
    public float maxSlowTime = 150;
    public float slowtimeFromHealth;
    public float waitTime = 0.5f;
    public float slowFromEnemy = 5;

    private GameManager gameManager;
    private float slowLastActive;

    private float lastHit;
    private SpriteRenderer spriteRenderer;
    private Vector4 damageColorVector;
    private Vector4 colorVector;

    private CircleCollider2D collider;
    private CameraHandler cameraHandler;

    private void Awake()
    {
        this.slowCharge = new Health(maxSlowTime);
        this.health = new Health(playerHealth, this);
        slowLastActive = Mathf.NegativeInfinity;

        spriteRenderer = GetComponent<SpriteRenderer>();
        damageColorVector = VectorColor.ColorToVector(damageColor);
        colorVector = VectorColor.ColorToVector(spriteRenderer.color);

        collider = GetComponent<CircleCollider2D>();

        cameraHandler = FindObjectOfType<CameraHandler>();

        lastHit = Mathf.NegativeInfinity;
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
        this.CheckChargeSlow();
        this.DamageAnimation();
    }

    private void DamageAnimation()
    {
        spriteRenderer.color = VectorColor.VectorToColor(Vector4.Lerp(spriteRenderer.color, colorVector, 2 * Time.deltaTime / invincibilityTime));
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
            cameraHandler.smoothness = 3f;
        }
        else if (!globSlow.Active())
            cameraHandler.smoothness = 10f;
    }

    private void CheckLocalSlow()
    {
        if(globSlow.Active())
        {
            CheckTeleport();
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

    void CheckTeleport()
    {

        if (Time.time - lastTeleport > teleportCooldown && Input.GetMouseButtonDown(1))
        {
            if (Physics2D.OverlapCircle(mousePosition, collider.radius/2) == null)
            {
                Teleport(mousePosition);
                lastTeleport = Time.time;
            }
        }
    }

    private void CheckChargeSlow()
    {
        if (locSlow.Active())
        {
            slowCharge.Damage(Time.deltaTime);
            if (slowCharge.IsEmpty())
                locSlow.Deactivate();
            slowLastActive = Time.time;
        }
    }

    public void GetEnemyCharge()
    {
        this.slowCharge.Heal(slowFromEnemy);
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
        if (Time.time - lastHit < invincibilityTime)
            return 0;
        float damaged = health.Damage(amount);
        slowCharge.IncreaseMaxHealth(slowtimeFromHealth * damaged);
        slowCharge.Heal(slowtimeFromHealth * damaged);

        spriteRenderer.color = damageColor;
        lastHit = Time.time;
        return damaged;
    }

    public void Kill()
    {
        gameManager.EndGame();
    }
}
