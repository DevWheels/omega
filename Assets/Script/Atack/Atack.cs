//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class NewBehaviourScript : MonoBehaviour
//{
//    public KeyCode attackKey = KeyCode.T;
//    public int damage = 25;

//    void Update()
//    {
//        if (Input.GetKeyDown(attackKey))
//        {
//            RaycastHit hit;
//            if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
//            {
//                var enemyHealth = hit.collider.GetComponent<EnemyHealth>();
//                if (enemyHealth != null)
//                {
//                    enemyHealth.TakeDamage(damage);
//                    Debug.Log("Нанесен урон врагу!");
//                }
//            }
//        }
//    }
//}
