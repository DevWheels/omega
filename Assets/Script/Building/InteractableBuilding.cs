using Mirror;
using UnityEngine;

public class InteractableBuilding : NetworkBehaviour
{
    public string BuildingId;
    public float InteractionRadius = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var player = other.GetComponent<NetworkIdentity>();
        if (player.isLocalPlayer)
        {
            PlayersUI.Instance.ShowInteractionPrompt("Press E to interact");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var player = other.GetComponent<NetworkIdentity>();
        if (player.isLocalPlayer && Input.GetKeyDown(KeyCode.E))
        {
            var buildingConfig = BuildingSystem.Instance.GetBuildingConfig(BuildingId);
            if (buildingConfig != null)
            {
                PlayersUI.Instance.ShowBuildingUI(buildingConfig, player.GetComponent<PlayerBuildings>().GetBuildingLevel(BuildingId));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var player = other.GetComponent<NetworkIdentity>();
        if (player.isLocalPlayer)
        {
            PlayersUI.Instance.HideInteractionPrompt();
        }
    }
}