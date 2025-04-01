using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.IO;
using Mirror;
using Script.Stats.Player;

public class PlayerStats : NetworkBehaviour
{

    [SyncVar]
    protected int max_hp = 300;
    [SyncVar]
    protected int currently_hp = 300;
    [SyncVar]
    protected int max_mana = 65;
    [SyncVar]
    protected int currently_mana = 65;

    [SyncVar]
    protected int armor = 35;
    [SyncVar]
    protected int xp_needed = 1000;
    [SyncVar]
    protected int xp_needed_per_lvl = 1000;
    [SyncVar]
    protected int xp_currently = 0;
    [SyncVar]
    protected int lvl = 1;
    [SyncVar]
    protected int ability_points;

    [SyncVar]
    protected int strength = 1;
    [SyncVar]
    protected int sanity = 1;
    [SyncVar]
    protected int agility = 1;
    [SyncVar]
    protected int luck = 1;
    [SyncVar]
    protected int speed = 1;

    [SyncVar]
    protected float timer = 0f;
    [SyncVar]
    protected float interval = 1f;

    [SyncVar]
    protected float hp_regeneration;
    [SyncVar]
    protected float mana_regeneration;



    protected PlayerMovement playerMovement;
    private PlayerUI playerUI;

    
    private void OnApplicationQuit()
    {
        //SavePlayerData(); // Сохраняем данные при выходе
    }
    void Start()
    {
        FindPlayerComponents();
        UpdateEverything();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        AddExperience(2); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!ДЛЯ ТЕСТА удалить потом
        CheckHpAndMana();
        playerUI.UpdateUI();

        // Регенерация здоровья и маны раз в секунду, если не максимальное здоровье или мана
        // может быть переделать это в карутину в будущем
        timer += Time.deltaTime;
        if (timer >= interval && max_mana >= currently_mana && max_hp >= currently_hp)
        {
            Regeneration();
            timer = 0f; // Сброс времени на 1 секунду
        }
    }

    public void UseItem(Item item)
    {
        if (item.isHealing)
        {
            currently_hp += (int)item.HealingPower;
            if (currently_hp > max_hp)
            {
                currently_hp = max_hp; // Ограничиваем здоровье максимальным значением
            }
        }

        if (item.isMana)
        {
            currently_mana += (int)item.ManaPower;
            if (currently_mana > max_mana)
            {
                currently_mana = max_mana; // Ограничиваем ману максимальным значением
            }
        }

        playerUI.UpdateUI(); // Обновляем интерфейс после использования предмета
    }
    

    private void LevelUp()
    {

        // сохраняем избыток опыта, для перевода его в следующий уровень
        int extra_xp = xp_currently - xp_needed;

        //Повторная проверка на условие опыта и проверка на условный ограничитель в 100 уровней 
        if (lvl < 100 && xp_currently >= xp_needed)
        {
            lvl += 1;

            xp_currently = 0;
            ability_points += 1;
            playerUI.SetStateOfAbilityUpdateButtons();
        }

        //если уровень выше 100, то просто прибавляем к нему 1, ничего не выдавая
        if (lvl >= 100 && xp_currently >= xp_needed)
        {
            lvl += 1;
            xp_currently = 0;

        }

        //выдаём остаток опыта, если таковой есть
        xp_currently += extra_xp;
        UpdateAllStats();

    }
    /// <summary>
    /// Обновляет характеристика игрока, выдавая им новые значения
    /// </summary>
    protected void UpdateAllStats()
    {
        xp_needed = (int)(xp_needed_per_lvl + xp_needed_per_lvl * (lvl - 1));
        int strength_hp = (strength - 1) * 20;
        max_hp = (int)300 + strength_hp;
        int sanity_mana = (sanity - 1) * 10;
        max_mana = (int)65 + sanity_mana;
        playerUI.UpdateUI();
    }

    public void AddExperience(int experience)
    {
        xp_currently += experience;
        if (xp_currently >= xp_needed)
        {
            LevelUp();
        }
        playerUI.UpdateUI();
    }
    [Client]
    private void Regeneration()
    {
        hp_regeneration += 0.05f; // восстановление единицы хп раз в 20 секунд 
        mana_regeneration += 0.2f; // восстановление единицы маны раз в 5 секунд 

        if (hp_regeneration >= 1)
        {
            currently_hp += (int)hp_regeneration;
            hp_regeneration = 0f; // Сбросить значения регенерации
            playerUI.UpdateUI();

        }
        if (mana_regeneration >= 1 && currently_mana > max_mana * 0.1f)
        {
            currently_mana += (int)mana_regeneration;
            mana_regeneration = 0f; // Сбросить значения регенерации
            playerUI.UpdateUI();
        }
    }
    public void TakeHit(int damage)
    {
        currently_hp -= damage;
        UpdateEverything();
    }

    [Client]
    private void CheckHpAndMana()
    {
        if (currently_hp <= 0)
        {
             //Здоровье закончилось, можно умереть
            Destroy(gameObject);
        }
        else if (currently_hp >= max_hp)
        {
            currently_hp = max_hp;
        }
        if (currently_mana < 0)
        {
            currently_mana = 0;
        }
        else if (currently_mana >= max_mana)
        {
            currently_mana = max_mana;
        }
    }
    public int GetCurretlyMana()
    {
        return currently_mana;
    }
    public int GetMaxMana()
    {
        return max_mana;
    }
    public void ConsumeMana(int mana)
    {
        currently_mana -= mana;
    }

    public bool HasEnoughMana(int cost)
    {
        return currently_mana >= cost; // Проверка достаточности маны
    }
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
    /// <summary>
    /// Обновляет все данные при запуске игры
    /// </summary>
    [Client]
    private void UpdateEverything()
    {
        playerUI.UpdateUI();
        UpdateAllStats();
        //LoadPlayerData();
    }
    /// <summary>
    /// Корутина для поиска всех UI элементов
    /// </summary>
    [Client]
    private void FindPlayerComponents()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerUI = gameObject.GetComponent<PlayerUI>();
    }

}
