using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Vector3 spawnPosition;

    public int maxLives = 3;
    private int currentLives;

    private UIManager uiManager;
    private RuleManager ruleManager;
    private PlayerMovement playerMovement;

    public float standingDamagePerSecond = 10f;
    private float standingDamageTimer = 0f;

    [Header("Audio")]
    public AudioClip deathSFX;
    [Range(0f, 1f)] public float deathVolume = 0.9f;

    
    public GameOverUI gameOverUI;

    void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;

        uiManager = FindObjectOfType<UIManager>();
        ruleManager = FindObjectOfType<RuleManager>();
        playerMovement = GetComponent<PlayerMovement>();

        spawnPosition = transform.position; // pamti pocetnu poziciju

        UpdateUI();
    }

    void Update()
    {
        // Test tipka
        //if (Input.GetKeyDown(KeyCode.H)) TakeDamage(25f);

        // Standing Damage pravilo
        if (ruleManager != null && ruleManager.IsRuleActive("Standing Damage"))
        {
            if (playerMovement != null && playerMovement.IsStandingStill())
            {
                standingDamageTimer += Time.deltaTime;
                if (standingDamageTimer >= 1f)
                {
                    TakeDamage(standingDamagePerSecond);
                    standingDamageTimer = 0f;
                }
            }
            else
            {
                standingDamageTimer = 0f;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            currentLives--;

            // Pusti death zvuk 
            if (deathSFX != null)
            {
                var listenerPos = Camera.main != null ? Camera.main.transform.position : transform.position;
                AudioSource.PlayClipAtPoint(deathSFX, listenerPos, deathVolume);
            }

            if (currentLives > 0)
            {
                Respawn();
            }
            else
            {
                Die();
            }
        }

        UpdateUI();
    }

    void Respawn()
    {
        currentHealth = maxHealth;

        // vrati igraca na startnu poziciju
        transform.position = spawnPosition;

        //nova pravila generise
        if (ruleManager == null) ruleManager = FindObjectOfType<RuleManager>();
        if (ruleManager != null) ruleManager.GenerateNewRules();

        UpdateUI();
    }

    void Die()
    {
        Debug.Log("GAME OVER");

        
        var gm = FindObjectOfType<GameMusic>();
        if (gm != null) gm.FadeOut(0.6f);

        // Pusti death zvuk 
        if (deathSFX != null)
        {
            var listenerPos = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(deathSFX, listenerPos, deathVolume);
        }

        
        var goUI = gameOverUI != null ? gameOverUI : FindObjectOfType<GameOverUI>(true);
        if (goUI != null)
        {
            goUI.Show();
        }
        else
        {
            Debug.LogWarning("GameOverUI nije pronađen u sceni.");
        }
    }

    void UpdateUI()
    {
        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.SetHealth(currentHealth);
            uiManager.SetLives(currentLives);
        }
    }

    public float GetCurrentHealth() => currentHealth;
    public int GetCurrentLives() => currentLives;
}
