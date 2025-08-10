using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    public AudioClip deathSFX;
    [Range(0f, 1f)] public float deathVolume = 0.8f;
    public GameObject deathVFX;

    
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die(true); 
        }
    }

    
    public void DieWithoutKill()
    {
        Die(false); 
    }

    private void Die(bool countKill)
    {
        if (isDead) return;
        isDead = true;

        
        if (countKill && KillManager.Instance != null)
        {
            KillManager.Instance.RegisterKill();
        }

        
        if (deathSFX != null)
        {
            var listenerPos = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(deathSFX, listenerPos, deathVolume);
        }

        
        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
