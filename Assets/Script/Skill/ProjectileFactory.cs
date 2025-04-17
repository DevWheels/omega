using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour {
    public static ProjectileFactory Instance;

    [SerializeField]
    private List<ProjectileBase> _projectiles;

    private void Awake() {
        Instance = this;
    }

    public ProjectileBase GetProjectileByType(ProjectileType type) {
        var projectilePrefab = _projectiles.First(p => p.ProjectileType == type);
        return projectilePrefab;
    }
}

[Serializable]
public enum ProjectileType {
    Fireball,
    FireExplosion,
    HealParticles
}