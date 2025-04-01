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

namespace Script.Stats.Player
{
    public class PlayerUI : PlayerStats
    {
        [Header("Player Stats texts")]
        public TMP_Text hp_text;
        public TMP_Text hp_text_inv;

        public TMP_Text mana_text;
        public TMP_Text mana_text_inv;

        public TMP_Text armor_text;
        public TMP_Text strength_text;
        public TMP_Text sanity_text;
        public TMP_Text agility_text;
        public TMP_Text luck_text;
        public TMP_Text speed_text;
        public TMP_Text level_text;
        public TMP_Text level_text_percent_for_new_level;
        public TMP_Text ability_points_text;
        public TMP_Text exp_text;

        [Header("Player Stats UI buttons")]
        public GameObject strength_up;
        public GameObject sanity_up;
        public GameObject agility_up;
        public GameObject luck_up;
        public GameObject speed_up;
        
        public Image healthBarFill;  //Добавил Денис для отображения hp на canvas
        public Image manahBarFill;  //Добавил Денис для отображения mana на canvas
        public Image expBarFill;  //Добавил Денис для отображения hp на персанаже на canvas

        private PlayerStats playerStats;
        private Find find => new();

        void Start()
        {
            playerStats = gameObject.GetComponent<PlayerStats>();
        }
        
        
        public void UpdateUI()
        {
            //Обновляет визуально картинку для игрока в процентном соотношении
            UpdateHealthBar();
            UpdateManaBar();
            UpdateEXPBar();

            //Обновляет текст
            hp_text.text = $"{currently_hp}/{max_hp}";
            hp_text_inv.text = $"{currently_hp}/{max_hp}";

            mana_text.text = $"{currently_mana} / {max_mana}";
            mana_text_inv.text = $"{currently_mana} / {max_mana}";

            armor_text.text = $"{armor}";

            level_text.text = $"{lvl}";
            exp_text.text = $"Опыт: {xp_currently}/{xp_needed}";

            strength_text.text = $"{strength}";
            sanity_text.text = $"{sanity}";
            agility_text.text = $"{agility}";
            luck_text.text = $"{luck}";
            speed_text.text = $"{speed}";
            if (ability_points > 0)
                ability_points_text.text = $"Очки характеристиков: {ability_points}";
            else
                ability_points_text.text = "";

        }
        private void UpdateHealthBar() //Добавил Денис для отображения hp на canvas
        {

            if (healthBarFill != null)
            {
                float healthPercentage = (float)currently_hp / max_hp;
                healthBarFill.fillAmount = healthPercentage;
            }
        }

        private void UpdateEXPBar() //Добавил Юра для отображения exp на canvas
        {
            if (expBarFill != null)
            {
                float expPercentage = (float)xp_currently / xp_needed;
                expBarFill.fillAmount = expPercentage;
                level_text_percent_for_new_level.text = expPercentage.ToString("0.00%");
            }
        }

        private void UpdateManaBar() //Добавил Денис для отображения mana на canvas
        {
            if (manahBarFill != null)
            {
                float manaPercentage = (float)currently_mana / max_mana;
                manahBarFill.fillAmount = manaPercentage;
            }
        }
        public void IncreaseStrength()
        {
            if (ability_points > 0)
            {
                strength++;
                ability_points -= 1;
                UpdateAllStats();
                SetStateOfAbilityUpdateButtons();
            }
        }
        public void IncreaseSanity()
        {
            if (ability_points > 0)
            {
                sanity++;
                ability_points -= 1;
                UpdateAllStats();
                SetStateOfAbilityUpdateButtons();


            }

        }
        public void IncreaseAgility()
        {
            if (ability_points > 0)
            {
                agility++;
                ability_points -= 1;
                UpdateAllStats();

                SetStateOfAbilityUpdateButtons();

            }
        }
        public void IncreaseLuck()
        {
            if (ability_points > 0)
            {
                luck++;
                ability_points -= 1;
                UpdateAllStats();
                SetStateOfAbilityUpdateButtons();

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
                SetStateOfAbilityUpdateButtons();

            }
        }
        
        public void SetStateOfAbilityUpdateButtons()
        {
            if (ability_points > 0)
            {
                strength_up.transform.localScale = new Vector3(0.3f, 1, 1); // Устанавливаем нормальный размер, кнопка активна
                sanity_up.transform.localScale = new Vector3(0.3f, 1, 1);
                agility_up.transform.localScale = new Vector3(0.3f, 1, 1);
                luck_up.transform.localScale = new Vector3(0.3f, 1, 1);
                speed_up.transform.localScale = new Vector3(0.3f, 1, 1);
                UpdateUI();
            }
            else
            {
                strength_up.transform.localScale = Vector3.zero; // Устанавливаем размер в ноль, кнопка неактивна
                sanity_up.transform.localScale = Vector3.zero;
                agility_up.transform.localScale = Vector3.zero;
                luck_up.transform.localScale = Vector3.zero;
                speed_up.transform.localScale = Vector3.zero;
                ability_points_text.text = "";
            }
        }
        private void FindAllUI()
        {
            
            hp_text = find.FindUIElement<TMP_Text>("hp text");
            hp_text_inv = find.FindUIElement<TMP_Text>("hp text inv");

            mana_text = find.FindUIElement<TMP_Text>("mana text");
            mana_text_inv = find.FindUIElement<TMP_Text>("mana text inv");

            armor_text = find.FindUIElement<TMP_Text>("armor text");

            strength_text = find.FindUIElement<TMP_Text>("strength text");
            sanity_text = find.FindUIElement<TMP_Text>("sanity text");
            agility_text = find.FindUIElement<TMP_Text>("agility text");
            luck_text = find.FindUIElement<TMP_Text>("luck text");
            speed_text = find.FindUIElement<TMP_Text>("speed text");
            level_text = find.FindUIElement<TMP_Text>("lvl full");
            level_text_percent_for_new_level = find.FindUIElement<TMP_Text>("lvl procent");

            ability_points_text = find.FindUIElement<TMP_Text>("ability points text");
            exp_text = find.FindUIElement<TMP_Text>("exp text");
            healthBarFill = find.FindUIElement<Image>("Healthbar");
            manahBarFill = find.FindUIElement<Image>("Manabar");
            expBarFill = find.FindUIElement<Image>("xp bar filled");

            strength_up = find.FindGameObject("StrengthUp");
            strength_up.GetComponent<Button>().onClick.AddListener(IncreaseStrength);
            sanity_up = find.FindGameObject("SanityUp");
            sanity_up.GetComponent<Button>().onClick.AddListener(IncreaseSanity);
            agility_up = find.FindGameObject("AgilityUp");
            agility_up.GetComponent<Button>().onClick.AddListener(IncreaseAgility);
            luck_up = find.FindGameObject("LuckUp");
            luck_up.GetComponent<Button>().onClick.AddListener(IncreaseLuck);
            speed_up = find.FindGameObject("SpeedUp");
            speed_up.GetComponent<Button>().onClick.AddListener(IncreaseSpeed);

            UpdateUI();
        }

    }
    
}