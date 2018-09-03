using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEventScript : MonoBehaviour, IPointerClickHandler
{
    public string id;
    public RandomDroppingObjectsScript ObjectManager;

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        if (Input.touchCount == 1)
        {
            Instantiate(Resources.Load("Prefabs/ClickStone"),Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Quaternion.identity);
            Debug.Log("I was clicked");
            ObjectManager.ObjectClicked(gameObject);
        }
        /*
        else
        {
            Instantiate(Resources.Load("Prefabs/ClickStone"), Input.mousePosition, Quaternion.identity);
            Debug.Log("I was clicked");
            ObjectManager.ObjectClicked(gameObject);
        }
        */
    }
}
