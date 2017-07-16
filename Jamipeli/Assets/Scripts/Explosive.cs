using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive: MonoBehaviour {

    public float triggerRadius = 2;
    public float triggerTime = 4;
    public Color triggeredColor = Color.red;
    public float triggeredAnimationSpeedStart = 2;
    public float triggeredAnimationSpeedEnd = 5;
    public float speedUpAnimationStartTime = 2;
    public float size = .2f;

    public bool isTriggered { get { return triggerTimer.running; } }

    public float damageRadius = 1;
    public float knockBack = 1;

    public int damagePlayer = 1;
    public int damageEnemy = 2;
    public Dieable owner;

    public Sprite explosionSprite;
    
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
        timerObject.transform.parent = transform.parent;
        triggerTimer = timerObject.AddComponent<Timer>();
        triggerTimer.AddAction(new DoOnTimeout(() => Explode(owner)));
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

    public void Explode(Dieable dieable)
    {
        Explosion exp = gameObject.AddComponent<Explosion>();
        exp.fadeoutTime = 0.5f;
        exp.damageRadius = this.damageRadius;
        exp.damageEnemy = this.damageEnemy;
        exp.damagePlayer = this.damagePlayer;
        exp.knockback = this.knockBack;
        exp.GetComponent<SpriteRenderer>().sprite = explosionSprite;
        exp.transform.localScale *= size;

        exp.Initialize();
        exp.Explode(dieable);
    }

}
