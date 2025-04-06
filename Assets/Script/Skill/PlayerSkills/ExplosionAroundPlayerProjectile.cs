using System;
using Mirror;
using UnityEngine;
using System.Collections;

public class ExplosionAroundPlayerProjectile : ProjectileBase {
    private int projectileDamage;


    public override void Init(int damage, int speed) {
        projectileDamage = damage;
    }
    private void Start() {
        StartCoroutine(nameof(DestroyProjectile));
    }

    private void DestroyProjectile() {
        Destroy(gameObject,3);
    }


}