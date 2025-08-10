using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [Header("Scene names")]
    public string gameSceneName = "Game";
    public string mainMenuSceneName = "MainMenu";

    [Header("Game Over SFX")]
    public AudioClip gameOverSFX;
    [Range(0f, 1f)] public float sfxVolume = 0.9f;
    [Range(0.1f, 2f)] public float sfxPitch = 0.85f; // <1.0 = sporije

    // (opciono) umesto trenutnog prekida, blago utišaj SFX pri kliku
    public bool fadeOutOnClick = false;
    public float fadeOutDuration = 0.3f;

    private AudioSource sfxSource;
    private bool shownOnce = false;

    void Awake()
    {
        // panel/canvas je sakriven na startu
        gameObject.SetActive(false);

        // pripremi AudioSource za SFX
        sfxSource = GetComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.dopplerLevel = 0f;
        sfxSource.spatialBlend = 0f; // 2D zvuk
    }

    public void Show()
    {
        // ako je već prikazan, samo ga pokaži (ne puštaj SFX ponovo)
        if (shownOnce)
        {
            gameObject.SetActive(true);
            return;
        }
        shownOnce = true;

        gameObject.SetActive(true);

        // pauziraj igru dok je ekran vidljiv
        Time.timeScale = 0f;

        // pusti SFX jednom, sa sniženim pitch-om
        if (gameOverSFX != null)
        {
            sfxSource.Stop();
            sfxSource.clip = gameOverSFX;
            sfxSource.volume = sfxVolume;
            sfxSource.pitch = sfxPitch;
            sfxSource.Play();
        }
    }

    public void OnRetry()
    {
        if (fadeOutOnClick && sfxSource != null && sfxSource.isPlaying)
        {
            StartCoroutine(FadeOutAndLoad(
                string.IsNullOrEmpty(gameSceneName) ? SceneManager.GetActiveScene().name : gameSceneName
            ));
            return;
        }

        if (sfxSource != null) sfxSource.Stop();
        Time.timeScale = 1f;
        SceneManager.LoadScene(string.IsNullOrEmpty(gameSceneName) ? SceneManager.GetActiveScene().name : gameSceneName);
    }

    public void OnMainMenu()
    {
        if (fadeOutOnClick && sfxSource != null && sfxSource.isPlaying)
        {
            StartCoroutine(FadeOutAndLoad(mainMenuSceneName));
            return;
        }

        if (sfxSource != null) sfxSource.Stop();
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float start = sfxSource != null ? sfxSource.volume : 0f;
        float t = 0f;

       
        while (sfxSource != null && t < fadeOutDuration)
        {
            t += Time.unscaledDeltaTime;
            sfxSource.volume = Mathf.Lerp(start, 0f, t / fadeOutDuration);
            yield return null;
        }

        if (sfxSource != null)
        {
            sfxSource.Stop();
            sfxSource.volume = start; 
        }

        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }
}