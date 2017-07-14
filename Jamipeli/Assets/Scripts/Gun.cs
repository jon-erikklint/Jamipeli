using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject projectilePrefab;

    public float shootForceMultiplier;
    public float shootInterval;

    private float lastShoot;

    private Transform owner;
    private CircleCollider2D oc;
    
	void Start () {
        owner = transform;
        oc = GetComponent<CircleCollider2D>();

        lastShoot = long.MinValue;
	}

    public void Shoot()
    {
        if (!CanShoot()) return;

        GameObject projectile = CreateProjectile();

        SetProjectileSpeed(projectile);
    }

    public bool CanShoot()
    {
        if(Time.time >= lastShoot + shootInterval)
        {
            lastShoot = Time.time;
            return true;
        }

        return false;
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
        return (Quaternion.AngleAxis(owner.eulerAngles.z, Vector3.forward) * Vector3.right).normalized;
    }

    public GameObject Projectile()
    {
        return projectilePrefab;
    }

    private GameObject CreateProjectile()
    {
        return Instantiate(Projectile(), CreationPoint(), owner.rotation) as GameObject;
    }

    private void SetProjectileSpeed(GameObject proj)
    {
        Rigidbody2D projbody = proj.GetComponent<Rigidbody2D>();
        
        Vector2 projectileVelocity = ShootForce();

        projbody.AddForce(projectileVelocity, ForceMode2D.Impulse);
    }

    public Vector2 ShootForce()
    {
        return FacedDirection() * shootForceMultiplier;
    }
}
