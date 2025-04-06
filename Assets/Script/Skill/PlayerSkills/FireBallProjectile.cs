using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class FireBallProjectile : ProjectileBase {

    private Vector2 target_direction;
    private int projectileDamage;
    private int projectileSpeed;

    public override void Init(int damage, int speed) {
        projectileDamage =  damage;
        projectileSpeed = speed;
        
    }
    
    private void Start() {
        StartCoroutine(nameof(DestroyProjectile));
        InitDirection();
    }

    void Update() {
        MoveProjectile();
    }

    private void DestroyProjectile() {
        Destroy(gameObject, 3);
    }

    private void MoveProjectile() {
        transform.position += (Vector3)(target_direction * (projectileSpeed * Time.deltaTime));
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void InitDirection() {
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        target_direction = (mouseWorldPos - transform.position).normalized;
    }
}