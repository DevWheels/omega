using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class FireBallProjectile : ProjectileBase {

    private Vector2 _targetDirection;
    private int _projectileDamage;
    private int _projectileSpeed;
    private int _projectileLifetime;
    private GameObject _owner;
    public override void Init(GameObject player,int damage, int speed, int lifetime) {
        _projectileDamage =  damage;
        _projectileSpeed = speed;
        _projectileLifetime = lifetime;
        _owner = player;
        
    }
    
    private void Start() {
        StartCoroutine(nameof(DestroyProjectile));
        InitDirection();
    }

    void Update() {
        MoveProjectile();

    }

    private void DestroyProjectile() {
        Destroy(gameObject,_projectileLifetime);
    }

    private void MoveProjectile() {
        transform.position += (Vector3)(_targetDirection * (_projectileSpeed * Time.deltaTime));
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void InitDirection() {
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        _targetDirection = (mouseWorldPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == gameObject || collision.gameObject == _owner) {return;}
        var enemyPlayer = collision.GetComponent<PlayerStats>();
        
        enemyPlayer.TakeHit(_projectileDamage);
    }
}