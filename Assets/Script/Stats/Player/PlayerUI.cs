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
    public class PlayerUI
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
        private Find Find;
        
        
        


    }
}