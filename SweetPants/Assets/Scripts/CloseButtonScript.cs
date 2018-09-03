using UnityEngine;
using UnityEngine.UI;

public class CloseButtonScript : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        Debug.Log("Close Menu - start");
        transform.GetComponent<Button>().onClick.RemoveAllListeners();
        transform.GetComponent<Button>().onClick.AddListener(Close);
    }
	public void Close()
    {
        Debug.Log("Close Menu");
        transform.parent.gameObject.SetActive(false);
    }
}
