using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float damageRadius;
    public float knockback;
    public float damagePlayer;
    public float damageEnemy;
    public float fadeoutTime;

    bool exploded = false;
    Timer timer;
    SpriteRenderer sRenderer;
    private void Start()
    {
    }

    public void Initialize()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        if (sRenderer == null)
            sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.color = new Color(0, 0, 0, 0);
        GameObject timerObject = new GameObject("timer");
        timerObject.transform.parent = transform.parent;
        timer = timerObject.AddComponent<Timer>();
    }

    private void Update()
    {
        if (exploded)
            Animate();
    }

    private void Animate()
    {
        Vector4 whiteVector = VectorColor.ColorToVector(Color.white);
        sRenderer.color = VectorColor.VectorToColor(whiteVector*(1-timer.timePassed/fadeoutTime));
        if (timer.ready)
            Destroy(gameObject);
    }

    public void Explode(Dieable dieable)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        foreach (var col in cols)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                if (rb == null) continue;
            Dieable lel = rb.GetComponent<Dieable>();
            if (lel == dieable)
                continue;
            Vector3 displacement = rb.transform.position - transform.position;
            rb.AddForce(displacement.normalized * knockback, ForceMode2D.Impulse);
            HasHealth health = rb.GetComponent<HasHealth>();
            if (health != null)
            {
                if (col.tag == "Player")
                    health.Damage(damagePlayer);
                else
                    health.Damage(damageEnemy);
            }
        }
        sRenderer.color = Color.white;
        timer.StartTimer(fadeoutTime);
        exploded = true;
    }
}
