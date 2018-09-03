using UnityEngine;
using System.Collections;

public class GoldAmountAjustmentScript : MonoBehaviour {

    public float sizeMultiplierW = 0;
    public float sizeMultiplierH = 0;
    public float widthOffset = 0;
    public float heightOffset = 0;
    public float imageSizeMultiplier = 0;
    public float textFieldSizeWMultiplier = 0;
    public float textFieldSizeHMultiplier = 0;
    public float imageOffset = 0;
    public float textOffset = 0;
	
	void Update () 
    {
        
        float sizeW, sizeH;

        sizeW = float.Parse((Screen.width * sizeMultiplierW).ToString());
        sizeH = float.Parse((Screen.width * sizeMultiplierH).ToString());

        Vector2 PanelPos = new Vector2((Screen.width - (sizeW / 2)) * widthOffset, ((Screen.height - (sizeH / 2)) * heightOffset));
        transform.GetComponent<RectTransform>().position = PanelPos;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeW, sizeH);

        transform.GetChild(0).GetComponent<RectTransform>().position = new Vector2(PanelPos.x * textOffset, PanelPos.y);
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(sizeW * textFieldSizeWMultiplier,sizeH * textFieldSizeHMultiplier);

        transform.GetChild(1).GetComponent<RectTransform>().position = new Vector2(PanelPos.x * imageOffset, PanelPos.y);
        transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(sizeW * imageSizeMultiplier, sizeW * imageSizeMultiplier);

        /*
        transform.GetChild(0).position = new Vector2(Screen.width / 2, (Screen.height - (size + 5)));
        transform.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2, (Screen.height - (size / 2)));
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
         */
	}
}
