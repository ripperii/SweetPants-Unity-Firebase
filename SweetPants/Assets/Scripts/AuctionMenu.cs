using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionMenu : MonoBehaviour {

    public FadeInOutScript fade;
    public float duration;
    public CanvasGroup canvasGroup;

    public Button Close;
    public Button Open;

    private void Awake()
    {
        Close.onClick.RemoveAllListeners();
        Open.onClick.RemoveAllListeners();

        Close.onClick.AddListener(CloseAuctionMenu);
        Open.onClick.AddListener(OpenAuctionMenu);
    }

    void OpenAuctionMenu()
    {
        StartCoroutine(fade.fadeIn(canvasGroup, duration));
    }
    void CloseAuctionMenu()
    {
        StartCoroutine(fade.fadeOut(canvasGroup, duration));
    }
}
