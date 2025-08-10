using UnityEngine;
using UnityEngine.UI;
using TMPro;

//public GameObject overheatIndicator;
public class UIManager : MonoBehaviour

{
    public Slider healthBar;
    public TMP_Text livesText;
    public TMP_Text rulesText;

    public void SetHealth(float value)
    {
        if (healthBar != null) healthBar.value = value;
    }

    public void SetLives(int lives)
    {
        if (livesText != null) livesText.text = "Lives: " + lives;
    }

    public void SetRules(string[] rules)
    {
        if (rulesText == null) return;
        rulesText.text = "Active Rules:\n";
        if (rules != null)
        {
            foreach (string rule in rules)
            {
                rulesText.text += "- " + rule + "\n";
            }
        }
    }

 

    // Overheat indikator
    [SerializeField] private GameObject overheatIndicator;

    public void SetOverheatActive(bool active)
    {
        if (overheatIndicator != null)
            overheatIndicator.SetActive(active);
    }
}