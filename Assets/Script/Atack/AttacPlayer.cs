//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttacPlayer : MonoBehaviour
//{

//    [System.Serializable]
//    public class AttackOption
//    {
//        public KeyCode attackKey;
//        public int attackPower;
//        public int manaCost; 
//        public GameObject attackEffectPrefab; 
//        public bool effectTriggered = false; 
//    }

//    public AttackOption[] attackOptions; 
//    public string[] enemyTags; 
//    private PlayerStats playerStats; 

//    void Start()
//    {
//        playerStats = GetComponent<PlayerStats>(); 
//    }

//    void Update()
//    {
//        foreach (var attackOption in attackOptions)
//        {
//            if (Input.GetKeyDown(attackOption.attackKey) && !attackOption.effectTriggered)
//            {
//                Attack(attackOption); 
//                attackOption.effectTriggered = true;
//            }

         
//            if (Input.GetKeyUp(attackOption.attackKey))
//            {
//                attackOption.effectTriggered = false;
//            }
//        }
//    }

//    void Attack(AttackOption attackOption)
//    {
    
//        if (!playerStats.HasEnoughMana(attackOption.manaCost))
//        {
//            Debug.Log("Not enough mana!");
//            return;
//        }

 
//        playerStats.ConsumeMana(attackOption.manaCost);

 
//        GameObject effectInstance = CreateAttackEffect(attackOption.attackEffectPrefab);

//        if (effectInstance != null)
//        {
   
//            StartCoroutine(DestroyEffectAfterTime(effectInstance, 1.0f)); 
//        }

  
//        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

//        if (hit.collider != null)
//        {
      
//            foreach (var tag in enemyTags)
//            {
//                if (hit.collider.CompareTag(tag)) 
//                {
//                    EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();

//                    //if (enemy != null && enemy.IsAlive())
//                    {

//                        int damage = 10;
//                        enemy.TakeDamage(damage);
//                    }
//                    break; 
//                }
//            }
//        }
//    }

//    GameObject CreateAttackEffect(GameObject effectPrefab)
//    {
//        if (effectPrefab != null)
//        {
           
//            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            mousePosition.z = 0; 

//            return Instantiate(effectPrefab, mousePosition, Quaternion.identity);
//        }
//        return null; 
//    }

//    IEnumerator DestroyEffectAfterTime(GameObject effectInstance, float duration)
//    {
       
//        yield return new WaitForSeconds(duration);


//        Destroy(effectInstance);
//    }

//    int CalculateDamage(int attackPower, int enemyArmor)
//    {
//        int damage = attackPower - enemyArmor;
//        return damage > 0 ? damage : 0; 
//    }
//}
