using System.Collections;
using Mirror;
using UnityEngine;

public class ShadowStalkerDebuff : NetworkBehaviour
{
    [SyncVar] private float duration;
    [SyncVar] private float effectValue;
    [SyncVar] private int maxStacks;
    [SyncVar] private int currentStacks;
    [SyncVar] private float endTime;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private float originalSpeed;

    public int CurrentStacks => currentStacks;

    public void SetDebuffParameters(float newDuration, float newEffectValue, int newMaxStacks)
    {
        duration = newDuration;
        effectValue = newEffectValue;
        maxStacks = newMaxStacks;
    }

    public void AddStack()
    {
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
            playerMovement = GetComponent<PlayerMovement>();
            originalSpeed = playerMovement.moveSpeed;
        }

        currentStacks = Mathf.Min(currentStacks + 1, maxStacks);
        endTime = Time.time + duration;
        
        ApplyEffect();
        
        // Перезапускаем корутину, если это новый стак
        StopAllCoroutines();
        StartCoroutine(DebuffCountdown());
    }

    private void ApplyEffect()
    {
        if (playerMovement != null)
        {
            // Пример: уменьшаем скорость игрока на effectValue за каждый стак
            playerMovement.moveSpeed = originalSpeed * (1 - effectValue * currentStacks);
        }
    }

    private IEnumerator DebuffCountdown()
    {
        while (Time.time < endTime)
        {
            yield return null;
        }
        
        currentStacks--;
        
        if (currentStacks <= 0)
        {
            RemoveEffect();
            Destroy(this);
        }
        else
        {
            endTime = Time.time + duration;
            ApplyEffect();
            StartCoroutine(DebuffCountdown());
        }
    }

    private void RemoveEffect()
    {
        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalSpeed;
        }
    }

    private void OnDestroy()
    {
        RemoveEffect();
    }
}