using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSFX;
    [Range(0f, 1f)] public float volume = 0.8f;

    void Start()
    {
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayClick);
    }

    void PlayClick()
    {
        if (clickSFX != null)
        {
            var listenerPos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
            AudioSource.PlayClipAtPoint(clickSFX, listenerPos, volume);
        }
    }
}
