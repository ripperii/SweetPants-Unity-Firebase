using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuPosition : MonoBehaviour {

	public static RectTransform SetPivot(RectTransform rt)
    {
        Vector2 localpoint;

        if (Input.touchSupported)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.GetTouch(0).position, rt.GetComponentInParent<Canvas>().worldCamera, out localpoint);
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, rt.GetComponentInParent<Canvas>().worldCamera, out localpoint);
        }

        rt.anchoredPosition = localpoint;

        Vector3 pivot = new Vector3();

        if (rt.position.y > Screen.height / 2)
        {
            pivot.y = 1;
        }
        else
        {
            pivot.y = 0;
        }
        if(rt.position.x > Screen.width / 2)
        {
            pivot.x = 1;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x - 40, rt.anchoredPosition.y);
        }
        else
        {
            pivot.x = 0;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + 40, rt.anchoredPosition.y);
        }

        rt.pivot = pivot;

        return rt;
    }
}
