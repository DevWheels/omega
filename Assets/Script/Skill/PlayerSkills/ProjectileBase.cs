using Mirror;

public abstract class ProjectileBase : NetworkBehaviour {
    [SyncVar] public int damage;
    [SyncVar] public int speed;
    [SyncVar] public int lifetime;

    public abstract void Init(int damage, int speed, int lifetime);
}