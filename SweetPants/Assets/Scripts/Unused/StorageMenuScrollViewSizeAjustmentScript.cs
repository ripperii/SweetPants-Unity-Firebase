using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class StorageMenuScrollViewSizeAjustmentScript : MonoBehaviour {

    public GameObject menu;
    public GameObject contentPanel;
    public float sizeW = 0;
    public float sizeH = 0;
    public float posY = 0;
    public float posX = 0;
    public float contentSize = 0;
    void Update()
    {
        float width, height;
        width = menu.GetComponent<RectTransform>().rect.width;
        height = menu.GetComponent<RectTransform>().rect.height;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width * sizeW, height * sizeH);

        transform.GetComponent<RectTransform>().position = new Vector2(width * posX, height * posY);

        contentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width * sizeW, contentPanel.GetComponent<RectTransform>().sizeDelta.y);
        //contentPanel.GetComponent<RectTransform>().position = new Vector2(width * posX, height * posY);

        foreach(Transform child in contentPanel.transform)
        {
            child.GetComponent<LayoutElement>().minHeight = width * contentSize;
        }
    }
}
