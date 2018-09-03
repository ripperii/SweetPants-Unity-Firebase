using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenCheatsMenuButtonScript : MonoBehaviour {

    public Button openCheats;
    public GameObject cheatsMenu;
	
    void Awake()
    {
        openCheats.onClick.RemoveAllListeners();
        openCheats.onClick.AddListener(openCheatsMenu);
    }
    void openCheatsMenu()
    {
        cheatsMenu.transform.gameObject.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
