using UnityEngine;
using System.Collections;

public class StorageMenuSizeAjustmentScript : MonoBehaviour {

    public float sizeWmultiplier = 0;
    public float sizeHmultiplier = 0;
    public float menuWidthOffset = 0;
    public float sizewidht, sizeheight;
	void Update () 
    {
        
        float sizeW, sizeH;
        sizeW = float.Parse((Screen.width / sizeWmultiplier).ToString());
        sizeH = float.Parse((Screen.height / sizeHmultiplier).ToString());
        
        transform.GetComponent<RectTransform>().position = new Vector2(Screen.width/2, Screen.height/2 + (Screen.height * menuWidthOffset));
        sizeheight = sizeH;
        sizewidht = sizeW;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeW, sizeH);
	}
}
