using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float fatalSpeed = 2;
    public int damage = 1;

    public float safeTime = 0.1f;

    public Color fatalColor = Color.white;
    public Color fatalForEnemyColor = Color.blue;
    public Color safeColor = Color.white;
    
    Rigidbody2D playerRb;

    Rigidbody2D bulletRb;
    SpriteRenderer bulletRenderer;
    float lastHit;
    float speed { get { return bulletRb.velocity.magnitude; } }
    bool fatalForEnemy = false;

    void Awake() {
        Transform player = FindObjectOfType<PlayerMover>().transform;
        playerRb = player.GetComponent<Rigidbody2D>();

        bulletRb = GetComponent<Rigidbody2D>();
        bulletRenderer = GetComponent<SpriteRenderer>();

        lastHit = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsFatal())
            bulletRenderer.color = safeColor;
        else
            bulletRenderer.color = fatalForEnemy ? fatalForEnemyColor : fatalColor; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Bullet" && obj.tag != "Wall")
            return;

        if (obj.tag == "Wall" || (Time.time - lastHit > safeTime && IsFatal()))
        {
            HasHealth health = obj.GetComponent<HasHealth>();
            if(health != null && Damages(obj.tag))
                health.Damage(damage);
            Destroy(gameObject);
        }
        Debug.Log((Time.time - lastHit > safeTime) + ", " + IsFatal());
        lastHit = Time.deltaTime;
    }

    public void MakeFatalForEnemies()
    {
        fatalForEnemy = true;
    }

    private bool IsFatal()
    {
        return speed > fatalSpeed;
    }

    private bool Damages(string tag)
    {
        return (tag == "Enemy" && fatalForEnemy) || (tag == "Player");
    }
}
