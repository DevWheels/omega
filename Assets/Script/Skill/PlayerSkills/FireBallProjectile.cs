using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireBallProjectile : MonoBehaviour{
    public int speed = 5;
    public int damage = 35;

    private void Start() {
        StartCoroutine("DestroyProjectile");
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
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
}
