using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniGamesMenuScript : MonoBehaviour {

    public Button close;
    public Button open;
    public float duration;
    public GameObject itemHolder;
    public static string selectedItem;
    public FadeInOutScript fade;

    void Awake()
    {
        open.onClick.RemoveAllListeners();
        open.onClick.AddListener(OpenMiniGamesMenu);

        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseMiniGamesMenu);

    }
    void CloseMiniGamesMenu()
    {
        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));
    }
    void OpenMiniGamesMenu()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
    }
}
