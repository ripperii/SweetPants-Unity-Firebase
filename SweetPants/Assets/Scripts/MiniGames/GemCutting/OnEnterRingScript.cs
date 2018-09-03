using UnityEngine;
using UnityEngine.EventSystems;

public class OnEnterRingScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private DrawLineScript dl;

	// Use this for initialization
	void Start () {
        dl = GameObject.Find("RandomShapeCreator").GetComponent<DrawLineScript>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        if (Input.GetMouseButton(0))
        {
            dl.StartLine(this.transform.position, this.gameObject);
        }
        if(dl.lineStarted)
        {
            Debug.Log("Started and Entered");
            Debug.Log(dl.startedFrom.name + " = " + this.gameObject.name);
            if(dl.startedFrom.name != this.gameObject.name)
            {
                Debug.Log("Are they equal?");
                dl.DrawLine(dl.firstRing, this.transform.position);
                dl.lineStarted = false;
                dl.StartLine(this.transform.position,this.gameObject);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            dl.startedFrom = this.gameObject;
            dl.StartLine(this.transform.position, this.gameObject);
        }
    }
}
