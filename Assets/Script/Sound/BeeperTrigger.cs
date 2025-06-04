using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaSoundTrigger2D : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip areaSound;
    [Range(0, 1)] public float volume = 0.3f;
    public bool loopSound = true;
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color activeColor = new Color(0, 1, 0, 0.25f);
    public Color inactiveColor = new Color(1, 0, 0, 0.25f);

    private AudioSource _playerAudioSource;
    private Transform _playerTransform;
    private bool _isActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _playerTransform = other.transform;
        InitializeAudioSource(other.gameObject);
        PlaySound();
        
        Debug.Log($"Player entered sound zone: {name}", this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        StopSound();
        _playerTransform = null;
        
        Debug.Log($"Player exited sound zone: {name}", this);
    }

    private void InitializeAudioSource(GameObject player)
    {
        if (_playerAudioSource != null) return;
        
        _playerAudioSource = player.GetComponent<AudioSource>();
        if (_playerAudioSource == null)
        {
            _playerAudioSource = player.AddComponent<AudioSource>();
            Debug.Log($"Added AudioSource to {player.name}", this);
        }
    }

    private void PlaySound()
    {
        if (_playerAudioSource == null || areaSound == null) return;
        
        _playerAudioSource.clip = areaSound;
        _playerAudioSource.loop = loopSound;
        _playerAudioSource.volume = volume;
        _playerAudioSource.Play();
        _isActive = true;
    }

    private void StopSound()
    {
        if (_playerAudioSource == null) return;
        
        _playerAudioSource.Stop();
        _playerAudioSource.clip = null; // Очищаем клип для предотвращения случайного воспроизведения
        _isActive = false;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        
        var collider = GetComponent<Collider2D>();
        if (collider == null) return;

        Gizmos.color = _isActive ? activeColor : inactiveColor;

        switch (collider)
        {
            case BoxCollider2D box:
                Gizmos.DrawCube((Vector2)transform.position + box.offset, box.size);
                break;
                
            case CircleCollider2D circle:
                Gizmos.DrawSphere((Vector2)transform.position + circle.offset, circle.radius);
                break;
        }
    }

    // Дополнительная защита на случай уничтожения объекта
    private void OnDestroy()
    {
        if (_isActive)
        {
            StopSound();
        }
    }
}