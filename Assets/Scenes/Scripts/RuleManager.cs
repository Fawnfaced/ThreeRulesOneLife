using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    // 6 pravila; za rand generisanje 
    public string[] allRules = new string[]
    {
        "Standing Damage",
        "Slow Speed",
        "Enemy Rush",
        "Reverse Controls",
        "Overheat Gun",
        "Low Visibility"
    };

    public List<string> activeRules = new List<string>();

    private UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        GenerateNewRules();
    }

    public void GenerateNewRules()
    {
        activeRules.Clear();

        // Napravi lokalni "pool" iz koga biramo bez ponavljanja
        var pool = new List<string>(allRules);
        int toPick = Mathf.Min(3, pool.Count);

        for (int i = 0; i < toPick; i++)
        {
            int idx = Random.Range(0, pool.Count);
            activeRules.Add(pool[idx]);
            pool.RemoveAt(idx); // ukloni da se ne ponovi u istom životu
        }

        if (uiManager != null)
        {
            uiManager.SetRules(activeRules.ToArray());
        }

        Debug.Log("New Active Rules:");
        foreach (var r in activeRules) Debug.Log("- " + r);
    }

    public bool IsRuleActive(string ruleName)
    {
        return activeRules.Contains(ruleName);
    }
}
