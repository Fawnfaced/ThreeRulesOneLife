using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public float enemyRushInterval = 2f;

    private float timer;
    private RuleManager ruleManager;

    void Start()
    {
        ruleManager = FindObjectOfType<RuleManager>();
        timer = 0f;
    }

    void Update()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || enemyPrefab == null) return;

        timer += Time.deltaTime;

        float interval = spawnInterval;
        if (ruleManager != null && ruleManager.IsRuleActive("Enemy Rush"))
        {
            interval = enemyRushInterval;
        }

        if (timer >= interval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        var sp = spawnPoints[index];
        Instantiate(enemyPrefab, sp.position, Quaternion.identity);
    }
}