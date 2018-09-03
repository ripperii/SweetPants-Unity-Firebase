using UnityEngine;
using System.Collections;

public class SizeAjustmentScript : MonoBehaviour {
    public float x, y;
	void Update () 
    {
        
        x =float.Parse((Screen.width * 0.50).ToString());
        y =float.Parse((Screen.height * 0.50).ToString());
        
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(-x, -y);
	}
}
