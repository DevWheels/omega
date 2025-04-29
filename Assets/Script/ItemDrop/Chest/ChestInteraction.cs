using Mirror;
using UnityEngine;

public class ChestInteraction : NetworkBehaviour {
    [SyncVar]
    private bool _isNearby = false;

    [SyncVar]
    private bool _isOpened = false;

    [SyncVar]
    private float _cooldownTimer = 0f;

    [SerializeField]
    private Sprite _closedChestSprite;

    [SerializeField]
    private Sprite _openedChestSprite;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private GameObject _interactionPrompt;

    [SerializeField]
    private float _cooldownDuration = 30f;

    private void Update() {
        // Обновляем таймер перезарядки
        if (_isOpened && _cooldownTimer > 0) {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0) {
                ResetChest();
            }
        }
    }

    [Server]
    public void TryOpenChest(int playerLvl) {
        if (!_isOpened) {
            OpenChest(playerLvl);
        }
    }

    private void OpenChest(int playerLvl) {
        _isOpened = true;
        _cooldownTimer = _cooldownDuration;

        GetComponent<EnemyLoot>().DropItem(playerLvl);
        if (isServer) {
            RpcUpdateChestState(true);
        }
    }

    private void ResetChest() {
        _isOpened = false;
        if(isServer) {
            RpcUpdateChestState(false);
        }
    }

    [ClientRpc]
    private void RpcUpdateChestState(bool opened) {
        if (_spriteRenderer != null) {
            _spriteRenderer.sprite = opened ? _openedChestSprite : _closedChestSprite;
        }

        // Скрываем подсказку если сундук открыт
        if (opened) {
            HidePrompt();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;

        _isNearby = true;

        if (!_isOpened) {
            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;

        _isNearby = false;
        HidePrompt();
    }

    private void ShowPrompt() {
        if (_interactionPrompt != null && !_isOpened) {
            _interactionPrompt.SetActive(true);
        }
    }

    private void HidePrompt() {
        if (_interactionPrompt != null) {
            _interactionPrompt.SetActive(false);
        }
    }
}