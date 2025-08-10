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
    [Range(0.1f, 2f)] public float sfxPitch = 0.85f; 

   
    public bool fadeOutOnClick = false;
    public float fadeOutDuration = 0.3f;

    private AudioSource sfxSource;
    private bool shownOnce = false;

    void Awake()
    {
        
        gameObject.SetActive(false);

        
        sfxSource = GetComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.dopplerLevel = 0f;
        sfxSource.spatialBlend = 0f; 

    public void Show()
    {
        
        if (shownOnce)
        {
            gameObject.SetActive(true);
            return;
        }
        shownOnce = true;

        gameObject.SetActive(true);

       
        Time.timeScale = 0f;

        
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