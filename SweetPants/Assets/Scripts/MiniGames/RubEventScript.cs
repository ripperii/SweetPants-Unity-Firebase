using UnityEngine;
using UnityEngine.EventSystems;

public class RubEventScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public string id;
    public RandomDroppingObjectsScript ObjectManager;

    bool inObject = false;

    public float requiredDistance = 0;
    public float distance = 0;
    Vector2 newPosition;
    Vector2 lastPosition;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                lastPosition = Input.GetTouch(0).position;
            }
            if (inObject)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    newPosition = Input.GetTouch(0).position;
                    distance += (newPosition - lastPosition).magnitude;
                    lastPosition = newPosition;
                }
            }
        }
        if(distance >= requiredDistance)
        {
            ObjectManager.ObjectRubbed(gameObject);
        }
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inObject = false;
    }


}
