using UnityEngine;
using Mirror;

public class ProjectileDarkCultist : NetworkBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private float _speed = 5f;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // Игнорируем столкновения с врагами, миньонами и другими снарядами
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Minions"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Projectiles"), true);
    }

    public void Initialize(int damage, Vector2 direction)
    {
        _damage = damage;
        _rb.linearVelocity = direction * _speed;
        Destroy(gameObject, _lifetime);
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer) return;

        // Проверяем по компонентам, а не по тегам
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            player.TakeHit(_damage);
            NetworkServer.Destroy(gameObject);
            return;
        }

        // Проверяем стену по компоненту или слою
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}