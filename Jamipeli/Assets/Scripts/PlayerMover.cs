using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

    private Rigidbody2D rb;
    private Camera c;
    private Gun gun;

    public float playerSpeed;

	void Start () {
        this.rb = GetComponent<Rigidbody2D>();
        this.c = Camera.main;
        this.gun = GetComponent<Gun>();
	}
	
	// Update is called once per frame
	void Update () {
        this.SetSpeed();
        this.SetRotation();
        this.CheckShoot();
    }

    private void CheckShoot()
    {
        if (Input.GetMouseButton(0))
        {
            this.gun.Shoot();
        }
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
}
