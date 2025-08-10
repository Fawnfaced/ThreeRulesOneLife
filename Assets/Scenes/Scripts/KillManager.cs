

using UnityEngine;
using TMPro;

public class KillManager : MonoBehaviour
{
    public static KillManager Instance { get; private set; }

    [Header("Win Condition")]
    public int targetKills = 10;
    private int currentKills = 0;

    [Header("UI")]
    public TMP_Text killsText;   
    public WinUI winUI;          

    public AudioClip winSFX;
    [Range(0f, 1f)] public float winVolume = 0.9f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start() => UpdateUI();

    public void RegisterKill()
    {
        currentKills++;
        UpdateUI();
        if (currentKills >= targetKills) OnWin();
    }

    void UpdateUI()
    {
        if (killsText != null)
            killsText.text = $"Kills: {currentKills}/{targetKills}";
    }

    void OnWin()
    {
        if (winSFX != null)
        {
            var pos = Camera.main ? Camera.main.transform.position : Vector3.zero;
            AudioSource.PlayClipAtPoint(winSFX, pos, winVolume);
        }

        if (winUI != null) winUI.Show();  //  POZIV NA WinUI
        else
        {
            Debug.Log("YOU WIN!");
            Time.timeScale = 0f;
        }
    }
}
