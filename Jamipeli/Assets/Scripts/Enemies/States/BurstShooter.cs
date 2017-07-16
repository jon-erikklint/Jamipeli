using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShooter : AIState {

    float burstAngle;
    float burstInterval;
    float shootInterval;
    int numOfBullets;

    float shootAngle;
    int bulletsLeft;
    Timer burstTimer;
    Timer shootTimer;
    public BurstShooter(Enemy enemy, float burstInterval, float burstAngle, float shootInterval, int numOfBullets) : base(enemy)
    {
        this.burstAngle = burstAngle;
        this.burstInterval = burstInterval;
        this.numOfBullets = numOfBullets;
        this.shootInterval = shootInterval;
        GameObject timerObject = new GameObject();
        timerObject.transform.parent = enemy.transform;
        burstTimer = timerObject.AddComponent<Timer>();
        shootTimer = timerObject.AddComponent<Timer>();

        burstTimer.AddAction(new DoOnTimeout(BurstShoot));
        shootTimer.AddAction(new DoOnTimeout(Shoot));
    }

    public override void Activate()
    {
        burstTimer.StartTimer(burstInterval, true);
    }

    public override void Deactivate()
    {
        burstTimer.StopTimer();
        shootTimer.StopTimer();
    }

    private void BurstShoot()
    {
        shootTimer.StartTimer(shootInterval);
    }

    private void Shoot()
    {
        if (shootInterval == 0)
        {
            ShootAll();
            Reload();
            return;
        }

        if (bulletsLeft > 0)
        {
            LaunchBullet();
            shootTimer.StartTimer(shootInterval, true);
        }
        else
            Reload();
    }

    private void LaunchBullet()
    {
        enemy.Shoot(shootAngle);
        bulletsLeft--;
        shootAngle += burstAngle / numOfBullets;
    }

    private void Reload()
    {
        shootAngle = -burstAngle / 2;
        bulletsLeft = numOfBullets;
        burstTimer.StartTimer(burstInterval, true);
    }

    private void ShootAll()
    {
        for (int i = 0; i < numOfBullets; i++)
            LaunchBullet();
    }
}
