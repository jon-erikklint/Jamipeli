using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, GunInterface {

    public GameObject projectilePrefab;

    public float shootForceMultiplier;
    public float shootInterval;

    private Transform owner;
    private CircleCollider2D oc;
    private SlowKeeper slow;

    private Creator creator;

    Timer timer;

	void Start () {
        owner = transform;
        oc = GetComponent<CircleCollider2D>();
        slow = GetComponent<SlowKeeper>();

        creator = FindObjectOfType<Creator>();
        GameObject timerHolder = new GameObject("TimerHolder");
        timerHolder.transform.parent = this.transform;
        timer = timerHolder.AddComponent<Timer>();
        timer.StartTimer(0);
	}

    public bool Shoot(float angle)
    {
        if (!CanShoot()) return false;

        GameObject projectile = CreateProjectile();

        SetProjectileSpeed(projectile, angle);

        timer.StartTimer(shootInterval, true);

        return true;
    }

    private bool CanShoot()
    {
        return timer.ready;
    }

    private Vector3 CreationPoint()
    {
        Vector2 offset = FacedDirection();
        float width = oc.radius;
        Vector3 realOffset = new Vector3(width * offset.x, width * offset.y, 0);

        return owner.position + realOffset;
    }

    protected Vector2 FacedDirection()
    {
        return transform.right;// (Quaternion.AngleAxis(owner.eulerAngles.z, Vector3.forward) * Vector3.right).normalized;
    }

    public GameObject Projectile()
    {
        return projectilePrefab;
    }

    private GameObject CreateProjectile()
    {
        return creator.Instantiate(Projectile(), CreationPoint(), owner.rotation) as GameObject;
    }

    private void SetProjectileSpeed(GameObject proj, float angle)
    {
        Rigidbody2D projbody = proj.GetComponent<Rigidbody2D>();
        
        Vector2 projectileVelocity = ShootForce(angle);
        projectileVelocity *= slow.slowFactor;

        projbody.AddForce(projectileVelocity, ForceMode2D.Impulse);
    }

    public Vector2 ShootForce(float angle)
    {
        return Rotate(FacedDirection(), angle) * shootForceMultiplier;
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
