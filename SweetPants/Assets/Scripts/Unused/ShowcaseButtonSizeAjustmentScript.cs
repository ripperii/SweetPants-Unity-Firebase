using UnityEngine;
using System.Collections;

public class ShowcaseButtonSizeAjustmentScript : MonoBehaviour {

	void Update () {
        float size;
        size = float.Parse((Screen.width * 0.07).ToString());
        transform.GetComponent<RectTransform>().position = new Vector2(((size / 2)) + (size / 5), ((size / 2)) + (size / 5));
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
	}
}
