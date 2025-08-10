using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage to Player")]
    public float damageToPlayer = 25f;

    [Header("How to detect contact")]
    public bool useTrigger = true; 

    private bool consumed = false; // sprečava dupli hit u istom kontaktu
    private EnemyHealth enemyHealth;
    private Collider2D col;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        col = GetComponent<Collider2D>();

        
        if (useTrigger && col != null) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger || consumed) return;
        TryHit(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger || consumed) return;
        TryHit(collision.collider.gameObject);
    }

    private void TryHit(GameObject other)
    {
        var player = other.GetComponent<PlayerHealth>();
        if (player == null) return; // nije player

        consumed = true;

        
        player.TakeDamage(damageToPlayer);

        
        if (col != null) col.enabled = false;

        
        if (enemyHealth != null)
        {
            enemyHealth.DieWithoutKill();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
