using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour, Dieable {

    private Rigidbody2D rb;
    private Camera c;
    private LocalTimeSlow slow;
    private GunInterface gun;

    public float playerSpeed;
    public float acceleration;
    public float angularAcceleration;

    public Health health;
    public Health slowTime;
    public float slowtimeFromHealth;

	void Start () {
        this.rb = GetComponent<Rigidbody2D>();
        this.c = Camera.main;
        this.slow = GetComponentInChildren<LocalTimeSlow>();
        this.gun = GetComponent<GunInterface>();
	}
	
	// Update is called once per frame
	void Update () {
        this.SetSpeed();
        this.SetRotation();
        this.CheckShoot();
        this.CheckSlow();
        this.CheckSlowRadius();
    }

    private void CheckShoot()
    {
        if (Input.GetMouseButton(0))
        {
            this.gun.Shoot();
        }
    }

    private void CheckSlow()
    {
        if (Input.GetMouseButtonDown(1))
        {
            slow.Activate();
        }

        if (Input.GetMouseButtonUp(1))
        {
            slow.Deactivate();
        }
    }

    private void CheckSlowRadius()
    {
        slow.ChangeRadius(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void SetSpeed()
    {
        Vector2 movement = Vector2.zero;

        movement.x = Mathf.Lerp(movement.x, playerSpeed * Input.GetAxis("Horizontal"), acceleration*Time.deltaTime);
        movement.y = Mathf.Lerp(movement.y, playerSpeed * Input.GetAxis("Vertical"), acceleration*Time.deltaTime);

        rb.velocity = movement;
        rb.angularVelocity = 0;
    }

    private void SetRotation()
    {
        Vector2 worldLocation = c.ScreenToWorldPoint(Input.mousePosition);
        transform.eulerAngles = new Vector3(0, 0, AngleInDeg(transform.position, worldLocation));
    }

    private float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    private float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }

    public void Kill()
    {
        Debug.Log("Auts! Pelaaja kuoli :(");
    }
}
