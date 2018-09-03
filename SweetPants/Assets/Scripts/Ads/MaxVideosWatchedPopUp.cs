using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxVideosWatchedPopUp : MonoBehaviour {

    public FadeInOutScript fade;
    public Button close;
    public float duration = 0.5f;

    private void Awake()
    {
        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(Close);
    }
    private void Close()
    {
        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));
    }
    public void Open()
    {
        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
    }
}
