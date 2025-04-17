using Mirror;
using UnityEngine;

public abstract class ProjectileBase : NetworkBehaviour {
    [field:SerializeField]
    public ProjectileType ProjectileType { get;private set; }

    [SyncVar] public int damage;
    [SyncVar] public int speed;
    [SyncVar] public int lifetime;
    [SyncVar] public GameObject owner;
    public abstract void Init(GameObject player,int damage, int speed, int lifetime);
}