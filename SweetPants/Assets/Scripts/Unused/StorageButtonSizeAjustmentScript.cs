using UnityEngine;
using System.Collections;

public class StorageButtonSizeAjustmentScript : MonoBehaviour {

	
	void Update () 
    {
        float size;
        size = float.Parse((Screen.width * 0.07).ToString());
        transform.GetComponent<RectTransform>().position = new Vector2((Screen.width - (size / 2)) - (size / 5), (size / 2) + (size / 8));
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
	}
   
}
