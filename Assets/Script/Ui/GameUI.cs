using System;
using UnityEngine;

public class GameUI : MonoBehaviour{
    public static GameUI Instance {get; private set;}
    [field:SerializeField] public SkillContainerView SkillContainerView{get; private set;}
    
    [SerializeField] public GameObject button;

    private void Awake() {
        Instance = this;
    }
}
