using UnityEngine;
using System.Collections;

public class OpenStoreButtonSizeAjustmentScript : MonoBehaviour {
   

	void Update () 
    {
        float size;
        size = float.Parse((Screen.width * 0.10).ToString());
        transform.GetChild(0).position = new Vector2(Screen.width / 2, (Screen.height - (size + 5)));
        transform.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2, (Screen.height - (size / 2)));
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
	}
}
