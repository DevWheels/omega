using Mirror;

public abstract class ProjectileBase : NetworkBehaviour {
    [SyncVar] public int damage;
    [SyncVar] public int speed;

    public abstract void Init(int damage, int speed);
}