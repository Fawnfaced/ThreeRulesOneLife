using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIButtons : MonoBehaviour
{
    [Header("Scene names")]
    public string gameSceneName = "Game";
    public string mainMenuSceneName = "MainMenu";

    // Pozovi sa dugmeta "Retry"
    public void OnRetry()
    {
        Time.timeScale = 1f;

        // zaustavi/uništi muziku iz igre da ne „curi“
        var gm = FindObjectOfType<GameMusic>();
        if (gm != null) Destroy(gm.gameObject); // ili gm.Stop();

        SceneManager.LoadScene(gameSceneName);
    }

    // Pozovi sa dugmeta "Main Menu"
    public void OnMainMenu()
    {
        Time.timeScale = 1f;

        var gm = FindObjectOfType<GameMusic>();
        if (gm != null) Destroy(gm.gameObject); // ili gm.Stop();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}