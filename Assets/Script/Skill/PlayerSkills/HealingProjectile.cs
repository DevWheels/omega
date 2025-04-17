using System;
using Mirror;
using UnityEngine;

public class HealingProjectile : ProjectileBase {
    [SyncVar] private int _projectileDamage;
    [SyncVar] private int _projectileSpeed;
    [SyncVar] private int _projectileLifetime;
    [SyncVar] private GameObject _owner;
    public override void Init(GameObject player, int damage, int speed, int lifetime) {
        _owner = player;
        _projectileDamage = damage;
        _projectileSpeed = speed;
        _projectileLifetime = lifetime;
    }

    private void Start() {
        StartCoroutine(nameof(DestroyProjectile));
    }
    
    private void DestroyProjectile() {
        Destroy(gameObject,_projectileLifetime);
    }
}
