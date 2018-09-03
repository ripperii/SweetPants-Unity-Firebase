using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class EnterNameScript : MonoBehaviour {

    public Button enter;
    public new InputField name;
    public Text error;

    public float duration = 0.5f;
    public FadeInOutScript fade;
    // Use this for initialization
    void Awake () {
        
        enter.onClick.RemoveAllListeners();
        enter.onClick.AddListener(SendName);
	}
	public void OpenUserNameInputMenu()
    {
        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
    }
    void SendName()
    {
        if(name.text.Length<6)
        {
            error.gameObject.SetActive(true);
            error.text = "The Name must be at least 6 letters long!";
            return;
        }

        DatabaseManager.sharedInstance.UpdateUsername(name.text);

        //StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));

        SceneManager.LoadScene(1);
    }

	
}
