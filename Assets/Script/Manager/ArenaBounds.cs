using UnityEngine;

public class ArenaBounds : MonoBehaviour
{
    [Header("Arena Settings")]
    public Vector2 arenaSize = new Vector2(50f, 50f);
    public float edgeBuffer = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(arenaSize.x, 1f, arenaSize.y));
    }

    public bool IsOutOfBounds(Vector3 position)
    {
        Vector2 flatPos = new Vector2(position.x, position.z);
        Vector2 center = new Vector2(transform.position.x, transform.position.z);
        Vector2 bounds = arenaSize * 0.5f - Vector2.one * edgeBuffer;

        return Mathf.Abs(flatPos.x - center.x) > bounds.x || 
               Mathf.Abs(flatPos.y - center.y) > bounds.y;
    }

    public Vector3 GetRandomPointInside()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.z);
        Vector2 bounds = arenaSize * 0.5f - Vector2.one * edgeBuffer;

        Vector2 randomPoint = center + new Vector2(
            Random.Range(-bounds.x, bounds.x),
            Random.Range(-bounds.y, bounds.y));

        return new Vector3(randomPoint.x, transform.position.y, randomPoint.y);
    }
}