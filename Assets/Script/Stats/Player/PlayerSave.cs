namespace Script.Stats.Player
{
    public class PlayerSave
    { 
        //public void SavePlayerData()
    //{
    //    Transform transform = GetComponent<Transform>();
    //    PlayerData data = new()
    //    {
    //        maxHp = max_hp,
    //        currentlyHp = currently_hp,
    //        maxMana = max_mana,
    //        currentlyMana = currently_mana,
    //        armor = armor,
    //        xpNeeded = xp_needed,
    //        xpNeededPerLvl = xp_needed_per_lvl,
    //        xpCurrently = xp_currently,
    //        lvl = lvl,
    //        abilityPoints = ability_points,
    //        strength = strength,
    //        sanity = sanity,
    //        agility = agility,
    //        luck = luck,
    //        speed = speed,
    //        //vector2 = transform.position,
    //    };

    //    string json = JsonUtility.ToJson(data);
    //    string path = Application.persistentDataPath + "/playerData.json";
    //    File.WriteAllText(path, json);
    //    Debug.Log("Данные игрока сохранены!");
    //}

    // Метод для загрузки данных игрока
    //public void LoadPlayerData()
    //{
    //    string path = Application.persistentDataPath + "/playerData.json";

    //    if (File.Exists(path))
    //    {
    //        string json = File.ReadAllText(path);
    //        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

    //        max_hp = data.maxHp;
    //        currently_hp = data.currentlyHp;
    //        max_mana = data.maxMana;
    //        currently_mana = data.currentlyMana;
    //        armor = data.armor;
    //        xp_needed = data.xpNeeded;
    //        xp_needed_per_lvl = data.xpNeededPerLvl;
    //        xp_currently = data.xpCurrently;
    //        lvl = data.lvl;
    //        ability_points = data.abilityPoints;
    //        strength = data.strength;
    //        sanity = data.sanity;
    //        agility = data.agility;
    //        luck = data.luck;
    //        speed = data.speed;
    //        //GetComponent<Transform>().position = data.vector2;
    //        Debug.Log("Данные игрока загружены!");
    //    }
    //    else
    //    {
    //        Debug.Log("Файл данных игрока не найден!");
    //    }
    //}

    }
}