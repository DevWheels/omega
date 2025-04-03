using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EnemyStats : MonoBehaviour
{
    private int max_hp = 300;
    private int currently_hp;
    public int xp_enemy = 10;
    public int attack_enemy = 45;
    public int speed = 1;
    public int armor = 100;

    private PlayerStats playerStats;
    //private Animator animator; 
    private Coroutine hurtCoroutine; 

    public Image healthBarFill;


    void Start()
    {
        currently_hp = max_hp;
        playerStats = FindObjectOfType<PlayerStats>();
        //animator = GetComponent<Animator>(); 
        UpdateHealthBar(); 
    }


    void Update()
    {
        if (currently_hp <= 0)
        {
            Die(); 
        }
        else
        {
            UpdateHealthBar(); 
        }
    }

    public int GetXp()
    {
        return xp_enemy;
    }

    public void TakeHit(int damage)
    {
        currently_hp -= damage;
        if (currently_hp < 0) currently_hp = 0;
        UpdateHealthBar(); 

        //animator.SetTrigger("isHurt"); 

    
        if (hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }


        hurtCoroutine = StartCoroutine(ResetHurtAnimation());
    }

    public bool IsAlive()
    {
        return currently_hp > 0; 
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerStats.TakeHit(attack_enemy);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float healthPercentage = (float)currently_hp / max_hp;
            healthBarFill.fillAmount = healthPercentage; 
        }
    }

    private void Die()
    {
        //animator.SetBool("isDead", true);

        Enemy enemy = GetComponent<Enemy>();

        enemy.Defeated();

        Destroy(gameObject, 1f); 
    }

    private IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        //animator.ResetTrigger("isHurt"); 
    }
}
