using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : AIState
{
    private float triggerTime = 4;
    private Color triggeredColor = Color.red;
    private float triggeredAnimationSpeedStart = 2;
    private float triggeredAnimationSpeedEnd = 5;
    private float speedUpAnimationStartTime = 2;

    private Vector4 triggeredColorVector;
    private Vector4 colorVector;
    private SpriteRenderer sRenderer;

    private float triggerRadius;

    private Timer timer;
    private bool exploding;

    public Exploder(Enemy enemy, float triggerRadius, float triggerTime, Color triggeredColor, float triggeredAnimationSpeedStart, float triggeredAnimationSpeedEnd, float speedUpAnimationStartTime) : base(enemy)
    {
        this.triggeredAnimationSpeedEnd = triggeredAnimationSpeedEnd;
        this.triggeredAnimationSpeedStart = triggeredAnimationSpeedStart;
        this.triggeredColor = triggeredColor;
        this.triggerTime = triggerTime;
        this.speedUpAnimationStartTime = speedUpAnimationStartTime;
        this.triggerRadius = triggerRadius;

        sRenderer = enemy.GetComponent<SpriteRenderer>();
        if (sRenderer == null)
            Debug.LogWarning("Object " + enemy.gameObject.name + ": SpriteRenderer not found, trigger animation won't work!");
        else
            colorVector = ColorToVector(sRenderer.color);

        triggeredColorVector = ColorToVector(triggeredColor);
    }

    public override void Activate()
    {
        GameObject timerObject = new GameObject();
        timer = timerObject.AddComponent<Timer>();
        timer.AddAction(new DoOnTimeout(Explode));
        timer.purpose = "Trigger timer";
        timerObject.transform.parent = enemy.transform;
    }

    public override void Update()
    {
        if (!exploding && enemy.TargetInDistance(triggerRadius))
        {
            timer.StartTimer(triggerTime);
            exploding = true;
        }

        if (exploding)
        {
            Animate();
        }

        enemy.MoveTowardsTarget();
    }

    private void Explode()
    {
        enemy.Kill();
    }

    private void Animate()
    {
        if (sRenderer == null) return;

        float timePassed = timer.timePassed;
        float cosArg;
        if (timePassed > speedUpAnimationStartTime)
            cosArg = 2 * Mathf.PI * (speedUpAnimationStartTime * triggeredAnimationSpeedStart + (speedUpAnimationStartTime - timePassed) * triggeredAnimationSpeedEnd);
        else
            cosArg = 2 * Mathf.PI * timePassed * triggeredAnimationSpeedStart;

        float fraction = 1 - Mathf.Cos(cosArg);
        Vector4 newColorVector = colorVector + (triggeredColorVector - colorVector) * fraction;
        sRenderer.color = VectorToColor(newColorVector);
    }

    Color VectorToColor(Vector4 vec)
    {
        return new Color(vec.x, vec.y, vec.z, vec.w);
    }

    Vector4 ColorToVector(Color col)
    {
        return new Vector4(col.r, col.g, col.b, col.a);
    }
}
