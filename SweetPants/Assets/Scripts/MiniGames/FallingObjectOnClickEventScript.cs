using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FallingObjectOnClickEventScript : MonoBehaviour,
     IPointerDownHandler,
     IPointerUpHandler,
     IPointerEnterHandler,
     IPointerExitHandler,
     ISelectHandler
     //, IDragHandler
     //, IDropHandler
   
     
{
    bool mouseButtonDown = false;
    bool inObject = false;

    public float distance = 0;
    Vector2 newPosition;
    Vector2 lastPosition;

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Debug.Log("Mouse Button Down");
            mouseButtonDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Button Up");
            mouseButtonDown = false;
        }
        
        if (inObject && mouseButtonDown)
        {
            newPosition = Input.mousePosition;
            if (Input.touchCount > 0)
                distance += Input.GetTouch(0).deltaPosition.magnitude;
            else if (Input.mousePresent)
                distance += (newPosition - lastPosition).magnitude; ;
            lastPosition = newPosition;
        }
        
	}
    /*
    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
        Debug.Log("I'm being dragged!");
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        drag = false;
        Debug.Log("I'm being dropped!");

    }
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("I'm being entered!");
        if (mouseButtonDown)
        {
            inObject = true;
            Debug.Log("asdasd");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inObject = false;
        Debug.Log("I'm being exited!");
    }

    public void OnSelect(BaseEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
