/**using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string gameSceneName = "Game";


    public void NewGame()
    {
        Time.timeScale = 1f; // da ne ostane pauza iz prethodne igre
        SceneManager.LoadScene(string.IsNullOrEmpty(gameSceneName) ? "Game" : gameSceneName);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}**/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "Game";

    [Header("Menu Music")]
    public AudioSource menuMusic;        // <- dodeli AudioSource sa menu muzikom
    public bool fadeOutOnNewGame = true; // uključi/isključi fade
    public float fadeOutDuration = 0.6f; // trajanje fade-outa u sekundama

    public void NewGame()
    {
        Time.timeScale = 1f; // safety

        if (menuMusic != null && fadeOutOnNewGame)
        {
            StartCoroutine(FadeOutAndLoad());
        }
        else
        {
            if (menuMusic) menuMusic.Stop(); // hard stop ako nema fade-a
            SceneManager.LoadScene(string.IsNullOrEmpty(gameSceneName) ? "Game" : gameSceneName);
        }
    }

    public void Quit()
    {
        if (menuMusic) menuMusic.Stop();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator FadeOutAndLoad()
    {
        float startVol = menuMusic.volume;
        float t = 2f;

        
        while (t < fadeOutDuration)
        {
            t += Time.unscaledDeltaTime;
            menuMusic.volume = Mathf.Lerp(startVol, 2f, t / fadeOutDuration);
            yield return null;
        }

        menuMusic.Stop();
        menuMusic.volume = startVol; 

        SceneManager.LoadScene(string.IsNullOrEmpty(gameSceneName) ? "Game" : gameSceneName);
    }
}