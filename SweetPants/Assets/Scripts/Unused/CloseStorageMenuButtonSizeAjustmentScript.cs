using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloseStorageMenuButtonSizeAjustmentScript : MonoBehaviour {

    public GameObject menu;
    public float size = 0;
    public float posY = 0;
    public float posX = 0;
	void Update () 
    {
        float width, height;
        width = menu.GetComponent<RectTransform>().rect.width;
        height = menu.GetComponent<RectTransform>().rect.height;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width*size, width*size);

        transform.GetComponent<RectTransform>().position = new Vector2(width*posX,height*posY);


	}
}
