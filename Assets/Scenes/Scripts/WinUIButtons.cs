using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIButtons : MonoBehaviour
{
    [Header("Scene names")]
    public string gameSceneName = "Game";
    public string mainMenuSceneName = "MainMenu";

    "
    public void OnRetry()
    {
        Time.timeScale = 1f;

        
        var gm = FindObjectOfType<GameMusic>();
        if (gm != null) Destroy(gm.gameObject); // ili gm.Stop();

        SceneManager.LoadScene(gameSceneName);
    }

   
    public void OnMainMenu()
    {
        Time.timeScale = 1f;

        var gm = FindObjectOfType<GameMusic>();
        if (gm != null) Destroy(gm.gameObject); 

        SceneManager.LoadScene(mainMenuSceneName);
    }
}