using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, Explosive {

    public float triggerRadius = 2;
    public float triggerTime = 4;
    public Color triggeredColor = Color.red;
    public float triggeredAnimationSpeedStart = 2;
    public float triggeredAnimationSpeedEnd = 5;
    public float speedUpAnimationStartTime = 2;

    public bool isTriggered { get { return triggerTimer.running; } }

    public float damageRadius = 1;
    public float knockBack = 1;

    public int damagePlayer = 1;
    public int damageEnemy = 2;
    Vector4 triggeredColorVector;
    Vector4 colorVector;
    SpriteRenderer sRenderer;
    Timer triggerTimer;
    Transform target;
    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        if (sRenderer == null)
            Debug.LogWarning("Object " + gameObject.name + ": SpriteRenderer not found, trigger animation won't work!");
        else
            colorVector = VectorColor.ColorToVector(GetComponent<SpriteRenderer>().color);

        triggeredColorVector = VectorColor.ColorToVector(triggeredColor);

        GameObject timerObject = new GameObject();
        triggerTimer = timerObject.AddComponent<Timer>();
        triggerTimer.AddAction(new DoOnTimeout(Explode));
        triggerTimer.purpose = "Trigger timer";
        timerObject.transform.parent = transform;

        target = FindObjectOfType<PlayerMover>().transform;
    }

    private void Update()
    {
        if (isTriggered && sRenderer != null)
            Animate();
        else
            CheckTarget();
    }

    void CheckTarget()
    {
        if ((target.position - transform.position).sqrMagnitude < triggerRadius * triggerRadius)
            triggerTimer.StartTimer(triggerTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            triggerTimer.StartTimer(triggerTime);
    }

    void Animate()
    {
        float timePassed = triggerTimer.timePassed;
        float cosArg;
        if (timePassed > speedUpAnimationStartTime)
            cosArg = 2 * Mathf.PI * (speedUpAnimationStartTime * triggeredAnimationSpeedStart + (speedUpAnimationStartTime - timePassed) * triggeredAnimationSpeedEnd);
        else
            cosArg = 2 * Mathf.PI * timePassed * triggeredAnimationSpeedStart;

        float fraction = 1 - Mathf.Cos(cosArg);
        Vector4 newColorVector = colorVector + (triggeredColorVector - colorVector) * fraction;
        sRenderer.color = VectorColor.VectorToColor(newColorVector);
    }

    public void Explode()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        foreach(var col in cols)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            Vector3 displacement = rb.transform.position - transform.position;
            rb.AddForce(displacement.normalized*knockBack, ForceMode2D.Impulse);
            Health health = rb.GetComponent<Health>();
            if (health != null)
            {
                if (col.tag == "Player")
                    health.Damage(damagePlayer);
                else
                    health.Damage(damageEnemy);
            }
        }

        Destroy(gameObject);
    }

}
