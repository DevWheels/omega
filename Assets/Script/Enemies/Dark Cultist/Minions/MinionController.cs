using System.Linq;
using UnityEngine;
using Mirror;
public abstract class MinionController : NetworkBehaviour
{
    [Header("Базовые настройки миньона")]
    [SerializeField] protected float _moveSpeed = 3f; 
    [SerializeField] protected float _attackRange = 1.5f; 
    [SerializeField] protected float _leashRadius = 8f; 
    [SerializeField] protected float _damageOnContact = 5f; 

    [Header("Damage Resistances")]
    [SerializeField] protected float fireResistance = 1f;
    [SerializeField] protected float iceResistance = 1f;
    [SerializeField] protected float lightningResistance = 1f;
    [SerializeField] protected float poisonResistance = 1f;
    [SerializeField] protected float holyResistance = 1f;
    
    
    protected Transform _target;
    protected TestenemyHealth _health; 
    protected Rigidbody2D _rb; 
    protected Transform _cultist; 

    public void SetCultist(Transform cultist)
    {
        _cultist = cultist;
    }

    protected virtual void Awake()
    {
        _health = GetComponent<TestenemyHealth>();
        _rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Minion";
        gameObject.layer = LayerMask.NameToLayer("Minions");
        
        _health.OnDeath += HandleDeath;
    }

    protected virtual void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDeath -= HandleDeath;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        FindTarget();
    }

    [Server]
    protected virtual void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            // Выбираем только живых игроков
            var alivePlayers = players.Where(p => 
                p.GetComponent<PlayerStats>()?.CurrentlyHp > 0).ToArray();
            if (alivePlayers.Length > 0)
            {
                _target = alivePlayers[Random.Range(0, alivePlayers.Length)].transform;
            }
        }
    }

    [ServerCallback]
    protected virtual void Update()
    {
        if (!isServer || _health.IsDead) return;

        if (_cultist != null && Vector2.Distance(transform.position, _cultist.position) > _leashRadius)
        {
            Vector2 direction = (_cultist.position - transform.position).normalized;
            _rb.linearVelocity = direction * _moveSpeed;
            return;
        }

        if (_target == null)
        {
            FindTarget();
            return;
        }

        HandleMovement();
    }

    [Server]
    protected virtual void HandleMovement()
    {
        float distance = Vector2.Distance(transform.position, _target.position);

        if (distance > _attackRange)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.linearVelocity = direction * _moveSpeed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
            Attack();
        }
    }

    [Server]
    protected virtual void Attack()
    {
        if (_health.CanAttack())
        {
            _health.ResetAttackCooldown();
            // Конвертируем урон из float в int перед передачей в TakeHit
            int damage = Mathf.RoundToInt(_health.CurrentAttack);
            _target.GetComponent<PlayerStats>().TakeHit(damage);
        }
    }

    protected virtual void HandleDeath()
    {
        Debug.Log($"{gameObject.name} умер");
    }
    [Server]
    public virtual void TakeDamage(int damage, DamageType damageType = DamageType.Physical)
    {
        float resistance = GetResistanceForType(damageType);
        int finalDamage = Mathf.RoundToInt(damage * resistance);
    
        Debug.Log($"Damage received: {damage} ({damageType}), resistance: {resistance}, final: {finalDamage}");
    
        _health.TakeDamage(finalDamage, null);
    }
    
    protected float GetResistanceForType(DamageType damageType)
    {
        return damageType switch
        {
            DamageType.Fire => fireResistance,
            DamageType.Ice => iceResistance,
            DamageType.Lightning => lightningResistance,
            DamageType.Poison => poisonResistance,
            DamageType.Holy => holyResistance,
            _ => 1f // Физический урон и другие по умолчанию
        };
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer) return;
    
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            // Конвертируем урон из float в int перед передачей в TakeHit
            int damage = Mathf.RoundToInt(_damageOnContact);
            player.TakeHit(damage);
        }
    }
}