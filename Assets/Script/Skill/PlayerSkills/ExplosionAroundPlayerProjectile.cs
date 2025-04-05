using System;
using Mirror;
using UnityEngine;
using System.Collections;

public class ExplosionAroundPlayerProjectile : NetworkBehaviour{
    [SyncVar]
    public int speed = 10;
    [SyncVar]
    public int damage = 35;


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
