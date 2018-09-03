using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipShopTimerPopUpScript : MonoBehaviour {

    public FadeInOutScript fade;
    public Button close;
    public Button WatchButton;
    public Button open;
    public MaxVideosWatchedPopUp mvwpu;
    public float duration = .5f;

    void Awake()
    {

        open.onClick.RemoveAllListeners();
        open.onClick.AddListener(OpenSkipShopTimerPopup);

        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseSkipShopTimerPopup);

        WatchButton.onClick.RemoveAllListeners();
        WatchButton.onClick.AddListener(CloseSkipShopTimerPopup);

    }
    void CloseSkipShopTimerPopup()
    {
        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));
    }
    public void OpenSkipShopTimerPopup()
    {
        Debug.Log("Timer Skip Button Clicked!");
        if (Player.currentPlayer.ShopTimerSkipedCount < RemoteConfig.MaxSkipShopTimerVideos)
        {
            StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
        }
        else
        {
            mvwpu.Open();
        }
    }
}
