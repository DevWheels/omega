using UnityEngine;
using Mirror;

public class PoisonPoolController : NetworkBehaviour
{
    [SerializeField] private float _damageInterval = 1f;
    [SerializeField] private float _damage = 2f;
    [SerializeField] private float _slowFactor = 0.7f;
    [SerializeField] private float _effectRadius = 2f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isServer) return;

        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null && Time.time % _damageInterval < Time.deltaTime)
        {
            // Конвертируем урон в int
            int damage = Mathf.RoundToInt(_damage);
            player.TakeHit(damage);
            player.RpcApplyTemporarySlow(_slowFactor, _damageInterval + 0.1f);
        }
    }

    [ClientRpc]
    public void RpcPlaySpawnEffect()
    {
        // Визуальный эффект появления лужи
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.3f);
        Gizmos.DrawSphere(transform.position, _effectRadius);
    }
}