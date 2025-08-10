using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 2f;
    public float damage = 20f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Launch(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
        dir.Normalize();
        rb.velocity = dir * speed;

        if (sr != null) sr.flipX = (dir.x < 0f);

        CancelInvoke();
        Invoke(nameof(SelfDestruct), lifetime);
    }

    void SelfDestruct()
    {
        if (this != null && gameObject != null) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var eh = other.GetComponent<EnemyHealth>();
            if (eh != null) eh.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}