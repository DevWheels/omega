using UnityEngine;

public class ItemDropSystem : MonoBehaviour
{
    [SerializeField] private ItemFactory itemFactory;

    [System.Serializable]
    public class RankDropChance
    {
        public float S = 0.02f;  
        public float A = 0.08f;  
        public float B = 0.15f;  
        public float C = 0.25f;  
        public float D = 0.5f;   
    }

    [SerializeField] private RankDropChance rankDropChances = new RankDropChance();

    public GameObject TryDropItem(int playerLevel, Vector3 spawnPosition)
    {
        
        float totalDropChance = rankDropChances.S + rankDropChances.A + rankDropChances.B +
                              rankDropChances.C + rankDropChances.D;
        float roll = Random.Range(0f, totalDropChance);

   
        ItemRank droppedRank = ItemRank.D; 

        if (roll <= rankDropChances.S)
        {
            droppedRank = ItemRank.S;
        }
        else if (roll <= rankDropChances.S + rankDropChances.A)
        {
            droppedRank = ItemRank.A;
        }
        else if (roll <= rankDropChances.S + rankDropChances.A + rankDropChances.B)
        {
            droppedRank = ItemRank.B;
        }
        else if (roll <= rankDropChances.S + rankDropChances.A + rankDropChances.B + rankDropChances.C)
        {
            droppedRank = ItemRank.C;
        }

        GameObject itemObject = itemFactory.CreateSpecificRankItemObject(droppedRank, playerLevel, spawnPosition);

        if (itemObject != null)
        {
            Iteme droppedItem = itemObject.GetComponent<ItemWorld>().GetItem();
            LogItemDetails(droppedItem);
            return itemObject;
        }

        Debug.Log("No item dropped this time");
        return null;
    }

    private void LogItemDetails(Iteme item)
    {
        string color = item.Rank switch
        {
            ItemRank.S => "yellow",
            ItemRank.A => "magenta",
            ItemRank.B => "cyan",
            ItemRank.C => "lime",
            _ => "white"
        };

        Debug.Log($"<color={color}>════════════ ITEM DROPPED ════════════</color>");
        Debug.Log($"<color={color}>Player Level:</color> {item.Level - 2}"); 
        Debug.Log($"<color={color}>Item Level:</color> {item.Level}");
        Debug.Log($"<color={color}>Name:</color> {item.Name1}");
        Debug.Log($"<color={color}>Rank:</color> {item.Rank}");
        Debug.Log($"<color={color}>Health:</color> {item.Health}");
        Debug.Log($"<color={color}>Armor:</color> {item.Armor}");
        Debug.Log($"<color={color}>Attack:</color> {item.Attack}");

        Debug.Log($"<color={color}>Special Stats:</color>");
        if (item.SpecialStats.Count > 0)
        {
            foreach (var stat in item.SpecialStats)
            {
                Debug.Log($"<color={color}>- {stat}</color>");
            }
        }
        else
        {
            Debug.Log($"<color={color}>- None</color>");
        }
        Debug.Log($"<color={color}>══════════════════════════════════════</color>");
    }
}