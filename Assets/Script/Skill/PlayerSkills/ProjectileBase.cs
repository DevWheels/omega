using Mirror;
using UnityEngine;

public abstract class ProjectileBase : NetworkBehaviour {
    [field:SerializeField]
    public ProjectileType ProjectileType { get;private set; }

    [HideInInspector]
    [SyncVar] public int damage;
    [HideInInspector]
    [SyncVar] public int speed;
    [HideInInspector]
    [SyncVar] public int lifetime;
    [SyncVar] public GameObject owner;
    [SyncVar] protected Vector2 TargetDirection;
    public abstract void Init(GameObject player,int damage, int speed, int lifetime);

    public void InitDirection( Vector3 dir) {
        TargetDirection = dir.normalized;
    }
}