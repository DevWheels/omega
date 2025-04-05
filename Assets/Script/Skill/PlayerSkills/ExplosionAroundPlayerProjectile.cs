using System;
using Mirror;
using UnityEngine;
using System.Collections;

public class ExplosionAroundPlayerProjectile : NetworkBehaviour{
    [SyncVar]
    public int speed;
    [SyncVar]
    public int damage;


    private void Start()
    {
        StartCoroutine(nameof(DestroyProjectile));
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
