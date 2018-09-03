using UnityEngine;
using UnityEngine.UI;

public class OpenPauseMenuScript : MonoBehaviour {

    private Button pause;
    private Button close;
    public CanvasGroup pauseMenu;
    public GameObject ROD;
    private RandomDroppingObjectsScript rod;
    private FadeInOutScript fadeInOut;
    public float duration = 0.25f;

    // Use this for initialization
    void Start () {
        pause = this.GetComponent<Button>();
        pause.onClick.RemoveAllListeners();
        pause.onClick.AddListener(OpenPauseMenu);
        close = GameObject.Find("No").GetComponent<Button>();
        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(ClosePauseMenu);
        rod = ROD.GetComponent<RandomDroppingObjectsScript>();
        fadeInOut = this.GetComponent<FadeInOutScript>();
    }
	
    void OpenPauseMenu()
    {
        rod.paused = true;
        StartCoroutine(fadeInOut.fadeIn(pauseMenu, duration));
    }
    void ClosePauseMenu()
    {
        rod.paused = false;
        StartCoroutine(fadeInOut.fadeOut(pauseMenu, duration));
    }
}
