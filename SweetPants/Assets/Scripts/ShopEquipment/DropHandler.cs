using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (eventData.pointerDrag == null)
            return;

        DragHandler d = eventData.pointerDrag.GetComponent<DragHandler>();
        if (d != null)
        {
            d.placeHolderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (eventData.pointerDrag == null)
            return;

        DragHandler d = eventData.pointerDrag.GetComponent<DragHandler>();
        if (d != null && d.placeHolderParent == this.transform)
        {
            d.placeHolderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropped on " + gameObject.name);

        DragHandler d = eventData.pointerDrag.GetComponent<DragHandler>();
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }
}
