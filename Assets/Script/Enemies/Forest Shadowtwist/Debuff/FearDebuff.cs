using UnityEngine;
using Mirror;
using System.Collections;

public class FearDebuff : NetworkBehaviour
{
    [SyncVar] private float duration;
    private PlayerMovement playerMovement;

    public void SetParameters(float newDuration)
    {
        duration = newDuration;
    }

    public void ApplyDebuff()
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
        
        playerMovement.ApplyFearEffect(duration);
        RpcShowFearEffect(true);
        
        StartCoroutine(RemoveAfterDuration());
    }

    private IEnumerator RemoveAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        RpcShowFearEffect(false);
        Destroy(this);
    }

    [ClientRpc]
    private void RpcShowFearEffect(bool show)
    {
        // Визуальный эффект страха (например, затемнение краев экрана)
    }
}