using UnityEngine;

public class BulletBoundary : MonoBehaviour
{
    private Vector2 minBounds;
    private Vector2 maxBounds;

    
    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.x < minBounds.x || pos.x > maxBounds.x || pos.y < minBounds.y || pos.y > maxBounds.y)
        {
            Destroy(gameObject);
        }
    }
}
