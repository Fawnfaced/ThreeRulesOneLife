using UnityEngine;

public class LowVisibility : MonoBehaviour
{
    public GameObject fogOverlay; // objekat koji smanjuje vidljivost
    private RuleManager ruleManager;

    void Start()
    {
        ruleManager = FindObjectOfType<RuleManager>();

        if (fogOverlay != null)
            fogOverlay.SetActive(false);
    }

    void Update()
    {
        if (ruleManager != null && fogOverlay != null)
        {
            bool active = ruleManager.IsRuleActive("Low Visibility");
            fogOverlay.SetActive(active);
        }
    }
}