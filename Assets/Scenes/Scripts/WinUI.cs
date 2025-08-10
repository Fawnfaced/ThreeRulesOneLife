using UnityEngine;

public class WinUI : MonoBehaviour
{
    [Header("SFX (optional)")]
    public AudioClip victorySFX;
    [Range(0f, 1f)] public float victoryVolume = 0.9f;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        
        if (victorySFX != null)
        {
            var pos = Camera.main ? Camera.main.transform.position : Vector3.zero;
            AudioSource.PlayClipAtPoint(victorySFX, pos, victoryVolume);
        }

        // pauziraj igru dok je win ekran otvoren
        Time.timeScale = 0f;

        
        var gm = FindObjectOfType<GameMusic>();
        if (gm != null) gm.FadeOut(0.6f);
    }
}
