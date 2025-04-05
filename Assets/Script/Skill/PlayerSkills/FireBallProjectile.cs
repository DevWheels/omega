using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
public class FireBallProjectile : NetworkBehaviour{
    public int speed = 5;
    public int damage = 35;
    private Vector2 target_direction;

    private void Start() {
        StartCoroutine(nameof(DestroyProjectile));
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        // Рассчитываем направление
        target_direction = (mouseWorldPos - transform.position).normalized;
    }

    void Update() {
        MoveProjectile();
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void MoveProjectile()
    {
        // Движемся в направлении курсора
        transform.position += (Vector3)(target_direction * (speed * Time.deltaTime));

    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0; // Или другое значение для Z
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
