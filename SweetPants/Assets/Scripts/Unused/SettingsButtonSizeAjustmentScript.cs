using UnityEngine;
using System.Collections;

public class SettingsButtonSizeAjustmentScript : MonoBehaviour {

	// Update is called once per frame
	void Update () 
    {
        float size;
        size = float.Parse((Screen.width*0.07).ToString());
        transform.GetComponent<RectTransform>().position = new Vector2((Screen.width - (size / 2)) - (size/5), (Screen.height - (size / 2)) - (size/5));
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
	}
}
