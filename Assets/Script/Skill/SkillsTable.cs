using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillsTable : MonoBehaviour {
    public List<SkillConfig> SkillConfigs;
    public static SkillsTable Instance;

    private void Awake() {
        Instance = this;
    }

    public List<string> GetSkillsForItem() {
        if (Random.Range(0,4) == 0) {
            return new List<string>();
        }
        return SkillConfigs.OrderBy((e) => Random.Range(0, 1f))
            .Take(Random.Range(1, 4))
            .Select((e) => e.Name).ToList();
    }
}