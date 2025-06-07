using System.Collections;
using UnityEngine;
using Mirror;

public class BleedingDebuff : NetworkBehaviour
{
    [SyncVar] private float duration;
    [SyncVar] private float damagePerTick;
    [SyncVar] private float tickInterval;
    
    private PlayerStats playerStats;
    private Coroutine bleedingCoroutine;
    private float endTime;

    public void SetParameters(float newDuration, float newDamage, float newInterval)
    {
        duration = newDuration;
        damagePerTick = newDamage;
        tickInterval = newInterval;
    }

    public void ApplyDebuff()
    {
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
        
        endTime = Time.time + duration;
        
        if (bleedingCoroutine != null)
            StopCoroutine(bleedingCoroutine);
            
        bleedingCoroutine = StartCoroutine(BleedingEffect());
        RpcShowBleedingEffect(true);
    }

    private IEnumerator BleedingEffect()
    {
        while (Time.time < endTime && playerStats != null)
        {
            // Изменено: передаем только один аргумент (damage)
            playerStats.TakeHit((int)damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        
        RpcShowBleedingEffect(false);
        Destroy(this);
    }

    [ClientRpc]
    private void RpcShowBleedingEffect(bool show)
    {
        // Визуальный эффект кровотечения (например, частицы крови)
    }
}