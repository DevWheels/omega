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


public class PlayerStats : NetworkBehaviour {

    [SyncVar] private int max_hp = 300;
    [SyncVar] private int currently_hp = 300;
    [SyncVar] private int max_mana = 65;
    [SyncVar] private int currently_mana = 65;

    [SyncVar] private int armor = 35;
    [SyncVar] private int xp_needed = 1000;
    [SyncVar] private int xp_needed_per_lvl = 1000;
    [SyncVar] private int xp_currently = 0;
    [SyncVar] private int lvl = 1;
    [SyncVar] private int ability_points;

    [SyncVar] private int strength = 1;
    [SyncVar] private int sanity = 1;
    [SyncVar] private int agility = 1;
    [SyncVar] private int luck = 1;
    [SyncVar] private int speed = 1;


    public int MaxHp
    {
        get { return max_hp; }
        set { max_hp = value; }
    }

    public int CurrentlyHp
    {
        get { return currently_hp; }
        set { currently_hp = value; }
    }

    public int MaxMana
    {
        get { return max_mana; }
        set { max_mana = value; }
    }

    public int CurrentlyMana
    {
        get { return currently_mana; }
        set { currently_mana = value; }
    }

    public int Armor
    {
        get { return armor; }
        set { armor = value; }
    }

    public int Lvl
    {
        get { return lvl; }
        set { lvl = value; }
    }

    public int XpNeeded
    {
        get { return xp_needed; }
        set { xp_needed = value; }
    }

    public int XpCurrently
    {
        get { return xp_currently; }
        set { xp_currently = value; }
    }

    public int AbilityPoints
    {
        get { return ability_points; }
        set { ability_points = value; }
    }

    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    public int Sanity
    {
        get { return sanity; }
        set { sanity = value; }
    }

    public int Agility
    {
        get { return agility; }
        set { agility = value; }
    }

    public int Luck
    {
        get { return luck; }
        set { luck = value; }
    }

    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }



    private PlayerMovement playerMovement;
    private PlayerUI playerUI;


    private void OnApplicationQuit()
    {
        //SavePlayerData(); // Сохраняем данные при выходе
    }
    void Start()
    {
        FindPlayerComponents();
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
    private void UpdateAllStats()
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
    
    public void IncreaseStrength()
    {
        if (ability_points > 0)
        {
            strength++;
            ability_points -= 1;
            UpdateAllStats();
            playerUI.SetStateOfAbilityUpdateButtons();
        }
    }
    public void IncreaseSanity()
    {
        if (ability_points > 0)
        {
            sanity++;
            ability_points -= 1;
            UpdateAllStats();
            playerUI.SetStateOfAbilityUpdateButtons();
        }

    }
    public void IncreaseAgility()
    {
        if (ability_points > 0)
        {
            agility++;
            ability_points -= 1;
            UpdateAllStats();

            playerUI.SetStateOfAbilityUpdateButtons();

        }
    }
    public void IncreaseLuck()
    {
        if (ability_points > 0)
        {
            luck++;
            ability_points -= 1;
            UpdateAllStats();
            playerUI.SetStateOfAbilityUpdateButtons();


        }
    }
    public void IncreaseSpeed()
    {
        if (ability_points > 0)
        {
            speed++;
            ability_points -= 1;
            float speed_multiply = 0.05f;
            playerMovement.moveSpeed += speed_multiply;
            UpdateAllStats();
            playerUI.SetStateOfAbilityUpdateButtons();
        }
    }
    
    [Client]
    private void UpdateEverything()
    {
        playerUI.UpdateUI();
        UpdateAllStats();
    }

    [Client]
    private void FindPlayerComponents()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerUI = GetComponent<PlayerUI>();;
        
    }
}
