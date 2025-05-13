// using UnityEngine;
// using Mirror;
//
//
// public class DialogStart : NetworkBehaviour {
//     public string title;
//     public GameObject Button;
//
//     private PlayerStats player;
//     private bool isPlayerNearby = false;
//     private Quest quest;
//     private QuestManager questManager;
//
//     private void Start() {
//         questManager = FindAnyObjectByType<QuestManager>();
//         quest = questManager.quests.Find(q => q.title == title);
//     }
//
//     void Update() {
//         if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !quest.isCompleted) {
//             if (!quest.isStarted) {
//                 gameObject.GetComponent<NPCIconController>().ChangeIcon();
//                 questManager.start.transform.localScale = Vector3.one;
//                 questManager.dialogueStart.text = quest.description;
//             }
//             else if (quest.isStarted) {
//                 questManager.middle.transform.localScale = Vector3.one;
//                 questManager.dialogueMiddle.text = quest.question;
//             }
//         }
//     }
//
//     [Client]
//     void OnTriggerEnter2D(Collider2D collider) {
//         questManager.playerStats = collider.GetComponent<PlayerStats>();
//         player = collider.GetComponent<PlayerStats>();
//         questManager.LoadAllQuests();
//         QuestManager questmanager = FindAnyObjectByType<QuestManager>();
//         Quest quest = questmanager.quests.Find(q => q.title == title);
//
//         if (collider.CompareTag("Player") && !quest.isCompleted) {
//             isPlayerNearby = true;
//             Button.SetActive(true);
//             questmanager.SetQuest(GetComponent<DialogStart>());
//         }
//         else {
//             Button.SetActive(false);
//         }
//     }
//
//     [Client]
//     void OnTriggerExit2D(Collider2D collider) {
//         quest = questManager.quests.Find(q => q.title == title);
//         if (collider.CompareTag("Player") && !quest.isCompleted) {
//             isPlayerNearby = false;
//             Button.SetActive(false);
//             questManager.SetQuest(GetComponent<DialogStart>());
//         }
//         else {
//             Button.SetActive(false);
//         }
//     }
// }