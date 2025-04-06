using System;
using Mirror;
using UnityEngine;
using System.Collections;

public class ExplosionAroundPlayerProjectile : ProjectileBase {
    private int projectileDamage;
    private int projectileSpeed;
    private int projectileLifetime;

    public override void Init(int damage, int speed, int lifetime) {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileLifetime = lifetime;
    }
    private void Start() {
        StartCoroutine(nameof(DestroyProjectile));
    }

    private void DestroyProjectile() {
        Destroy(gameObject,projectileLifetime);
    }


}