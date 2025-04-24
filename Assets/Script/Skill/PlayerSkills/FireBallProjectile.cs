using Mirror;
using UnityEngine;

public class FireBallProjectile : ProjectileBase {
    [SyncVar]
    private int _projectileDamage;

    [SyncVar]
    private int _projectileSpeed;

    [SyncVar]
    private int _projectileLifetime;

    [SyncVar]
    private GameObject _owner;

    public override void Init(GameObject player, int damage, int speed, int lifetime) {
        _projectileDamage = damage;
        _projectileSpeed = speed;
        _projectileLifetime = lifetime;
        _owner = player;
    }

    private void Start() {
        Destroy(gameObject, _projectileLifetime);
    }

    void Update() {
        MoveProjectile();
    }

    private void MoveProjectile() {
        transform.position += (Vector3)(TargetDirection * (_projectileSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_owner == null || collision.gameObject == _owner) {
            return; 
        }

        PlayerStats enemyPlayer = collision.GetComponent<PlayerStats>();
        if (enemyPlayer != null || !enemyPlayer.isLocalPlayer) {
            enemyPlayer.TakeHit(_projectileDamage);
        }
    }
}