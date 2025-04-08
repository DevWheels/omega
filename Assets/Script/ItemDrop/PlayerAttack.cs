using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 10;
    public float attackRange = 1f;
    public float attackCooldown = 0.5f;
    public Vector2 attackOffset = new Vector2(0.5f, 0);

    [Header("References")]
    public LayerMask enemyLayer;
    public Animator animator;
    public AudioClip attackSound;

    private float lastAttackTime;
    private bool isAttacking;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
  
        //if (animator != null)
        //    animator.SetTrigger("Attack");

  
        //if (attackSound != null)
        //    AudioSource.PlayClipAtPoint(attackSound, transform.position);


        Vector2 attackPos = (Vector2)transform.position +
                          attackOffset * (transform.localScale.x > 0 ? 1 : -1);


        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPos,
            attackRange,
            enemyLayer
        );


        foreach (Collider2D enemy in hitEnemies)
        {
            TestenemyHealth testenemyHealth = enemy.GetComponent<TestenemyHealth>();
            if (testenemyHealth != null)
            {
                testenemyHealth.TakeDamage(damage);
                Debug.Log($"Hit {enemy.name} for {damage} damage");
            }
        }

        lastAttackTime = Time.time;
    }


    void OnDrawGizmosSelected()
    {
        Vector2 attackPos = (Vector2)transform.position +
                          attackOffset * (transform.localScale.x > 0 ? 1 : -1);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}

