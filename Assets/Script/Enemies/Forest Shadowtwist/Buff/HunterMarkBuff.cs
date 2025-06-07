using UnityEngine;
using Mirror;

public class HunterMarkBuff : NetworkBehaviour
{
    [SyncVar] private float duration;
    [SyncVar] private float damageTakenMultiplier;
    
    private PlayerStats playerStats;
    private float endTime;

    public void SetParameters(float newDuration, float newMultiplier)
    {
        duration = newDuration;
        damageTakenMultiplier = newMultiplier;
    }

    public void ApplyBuff()
    {
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
        
        endTime = Time.time + duration;
        RpcShowHunterMarkEffect(true);
    }

    public float GetDamageMultiplier()
    {
        return Time.time < endTime ? damageTakenMultiplier : 1f;
    }

    [ClientRpc]
    private void RpcShowHunterMarkEffect(bool show)
    {
        // Визуальный эффект метки (например, красный ореол вокруг игрока)
    }

    private void OnDestroy()
    {
        RpcShowHunterMarkEffect(false);
    }
}