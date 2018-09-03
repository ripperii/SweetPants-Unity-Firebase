using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NECScript : MonoBehaviour {


    public Text text, convertrate;
    public Button yesButton, noButton;
    public static GameObject go;

	// Use this for initialization
	void Awake () {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        noButton.onClick.AddListener(noButtonFunc);
        yesButton.onClick.AddListener(yesButtonFunc);
        go = this.gameObject;
	
	}
    void noButtonFunc()
    {
        Destroy(go);
    }
    
	void yesButtonFunc()
    {
        //NetworkingWebGL.NECConfirmation();
        Destroy(go);
    }
    
	// Update is called once per frame
	void Update () {
	
	}
}
