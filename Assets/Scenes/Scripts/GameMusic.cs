using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMusic : MonoBehaviour
{
    [Header("Music")]
    public AudioClip gameLoopMusic;
    [Range(0f, 1f)] public float volume = 0.5f;
    public string gameSceneName = "Game";

    private AudioSource audioSource;

    
    private static GameMusic instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameLoopMusic;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D
        audioSource.dopplerLevel = 0f;
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void Start()
    {
       
        if (SceneManager.GetActiveScene().name == gameSceneName)
            Play();
        else
            Stop();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        audioSource.volume = volume;

        if (scene.name == gameSceneName)
        {
            
            if (gameLoopMusic != null)
            {
                if (audioSource.clip != gameLoopMusic)
                    audioSource.clip = gameLoopMusic;

                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
        }
        else
        {
            
            Stop();
        }
    }

    public void Play()
    {
        if (audioSource != null && !audioSource.isPlaying && audioSource.clip != null)
            audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; 
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; 
    }
}
