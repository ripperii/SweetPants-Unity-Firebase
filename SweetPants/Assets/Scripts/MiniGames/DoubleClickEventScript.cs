using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickEventScript : MonoBehaviour, IPointerClickHandler
{
    public string id;
    public RandomDroppingObjectsScript ObjectManager;

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        if(Input.touchCount == 2)
        {
            Debug.Log("I was double clicked");
            ObjectManager.ObjectClicked(gameObject);
        }
        
    }
}
