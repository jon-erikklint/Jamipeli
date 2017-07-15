using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float fatalSpeed = 2;
    public int damage = 1;

    public float safeTime = 0.1f;

    public Color fatalColor = Color.white;
    public Color safeColor = Color.blue;
    
    Rigidbody2D playerRb;

    Rigidbody2D bulletRb;
    SpriteRenderer bulletRenderer;

    float lastHit;
    float speed { get { return bulletRb.velocity.magnitude; } }

    void Start () {
        Transform player = FindObjectOfType<PlayerMover>().transform;
        playerRb = player.GetComponent<Rigidbody2D>();

        bulletRb = GetComponent<Rigidbody2D>();
        bulletRenderer = GetComponent<SpriteRenderer>();

        lastHit = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (speed < fatalSpeed)
            bulletRenderer.color = safeColor;
        else
            bulletRenderer.color = fatalColor;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Bullet")
            return;
        if (Time.time - lastHit > safeTime && bulletRenderer.color == fatalColor)
        {
            Health health = obj.GetComponent<Health>();
            if(health != null)
                health.Damage(damage);
            Destroy(gameObject);
        }

        lastHit = Time.time;
    }
}
