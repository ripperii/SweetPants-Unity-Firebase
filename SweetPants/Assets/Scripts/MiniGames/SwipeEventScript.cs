using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeEventScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{ 
    private bool enter = false;
    private bool exit = false;
    public string id;

    public RandomDroppingObjectsScript ObjectManager;
    
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                if (enter && exit)
                {
                    enter = exit = false;
                    ObjectManager.ObjectSwiped(gameObject);
                    Debug.Log("I was Swiped!");
                }
            if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                enter = exit = false;
            }
        }
        if (enter && exit)
        {
            enter = exit = false;
            ObjectManager.ObjectSwiped(gameObject);
            Debug.Log("I was Swiped!");
        }


    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("I'm being entered!");
        enter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("I'm being exited!");
        exit = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        enter = exit = false;
    }
}
