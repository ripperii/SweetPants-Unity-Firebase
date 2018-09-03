using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IAPMenuScript : MonoBehaviour {

    public Button close;
    public Button open;
    public float duration;
    public FadeInOutScript fade;

    void Awake()
    {
        open.onClick.RemoveAllListeners();
        open.onClick.AddListener(OpenStorageMenu);

        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseStorageMenu);

    }
    void CloseStorageMenu()
    {
        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));
    }
    void OpenStorageMenu()
    {
        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
    }
    
}
