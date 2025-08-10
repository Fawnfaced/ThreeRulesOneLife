using UnityEngine;

public class PlayerBoundaries : MonoBehaviour
{
    public Transform backgroundTransform;

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float halfWidth;
    private float halfHeight;

    void Start()
    {
        if (backgroundTransform == null)
        {
            Debug.LogWarning("PlayerBoundaries: Background Transform not assigned.");
            enabled = false;
            return;
        }

        BoxCollider2D boundsCollider = backgroundTransform.GetComponent<BoxCollider2D>();
        if (boundsCollider == null)
        {
            Debug.LogWarning("PlayerBoundaries: Background needs a BoxCollider2D covering the map.");
            enabled = false;
            return;
        }

        Bounds bounds = boundsCollider.bounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.size.x / 2f;
            halfHeight = sr.bounds.size.y / 2f;
        }
    }

    void LateUpdate()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        p.y = Mathf.Clamp(p.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        transform.position = p;
    }

    // === GETTER METODE ZA GRENICE ===
    public Vector2 GetMinBounds()
    {
        return new Vector2(minBounds.x + halfWidth, minBounds.y + halfHeight);
    }

    public Vector2 GetMaxBounds()
    {
        return new Vector2(maxBounds.x - halfWidth, maxBounds.y - halfHeight);
    }
}
