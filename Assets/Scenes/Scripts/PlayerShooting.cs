using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;   
    public Transform firePoint;       
    public float bulletSpeed = 12f;
    public float bulletLifetime = 2f;

    [Header("Fire")]
    public float fireRate = 0.25f;   
    private float nextFireTime = 0f;

    [Header("Overheat Rule")]
    public int overheatShotsLimit = 3;   
    public float overheatCooldown = 2f;  
    private int shotsSinceOverheat = 0;
    private bool isCoolingDown = false;

    private RuleManager ruleManager;
    private PlayerMovement pm;

    [Header("Shooting SFX")]
    public AudioClip shootSFX;
    [Range(0f, 1f)] public float shootVolume = 0.8f;

    [Header("Overheat SFX")]
    public AudioClip overheatSFX;
    [Range(0f, 1f)] public float overheatVolume = 0.9f;
    private bool overheatSoundPlayed = false;

    void Start()
    {
        ruleManager = FindObjectOfType<RuleManager>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isCoolingDown) return;
        if (Time.time < nextFireTime) return;

        //if (Input.GetButton("Fire1"))
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            Shoot();
            nextFireTime = Time.time + fireRate;

            bool overheatActive = (ruleManager != null && ruleManager.IsRuleActive("Overheat Gun"));
            if (overheatActive)
            {
                shotsSinceOverheat++;
                if (shotsSinceOverheat >= overheatShotsLimit)
                {
                    if (!overheatSoundPlayed && overheatSFX != null)
                    {
                        var listenerPos = Camera.main != null ? Camera.main.transform.position : transform.position;
                        AudioSource.PlayClipAtPoint(overheatSFX, listenerPos, overheatVolume);
                        overheatSoundPlayed = true;
                    }
                    StartCoroutine(Cooldown());
                }
            }
            else
            {
                shotsSinceOverheat = 0;
                overheatSoundPlayed = false;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("PlayerShooting: bulletPrefab ili firePoint nije dodeljen!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (shootSFX != null)
            AudioSource.PlayClipAtPoint(shootSFX, transform.position, shootVolume);

        
        Vector2 dir = new Vector2(pm != null ? pm.Facing : 1, 0f);

        // postavi brzinu
        var rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = dir * bulletSpeed;

        // ignorisi koliziju sa playerom
        var bulletCol = bullet.GetComponent<Collider2D>();
        var playerCol = GetComponent<Collider2D>();
        if (bulletCol && playerCol) Physics2D.IgnoreCollision(bulletCol, playerCol, true);

        // --- GRANICE: preuzmi iz PlayerBoundaries i postavi metku ---
        var pb = FindObjectOfType<PlayerBoundaries>();
        if (pb != null)
        {
            var bb = bullet.AddComponent<BulletBoundary>();
            bb.SetBounds(pb.GetMinBounds(), pb.GetMaxBounds());  
        }

        
        Destroy(bullet, bulletLifetime);
    }

    IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(overheatCooldown);
        shotsSinceOverheat = 0;
        isCoolingDown = false;
        overheatSoundPlayed = false;
    }
}
