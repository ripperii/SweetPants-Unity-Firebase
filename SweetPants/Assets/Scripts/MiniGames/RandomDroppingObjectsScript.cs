using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomDroppingObjectsScript : MonoBehaviour {

    public Canvas canvas;

    public bool paused = false;

    public float fallSpeed = 8.0f;
    public float maxDistance = 1000;
    public int maxClickCount = 10;
    public int maxSwipeCount = 5; 

    public float localPositionX;
    public float localPositionY;

    public float canvasWidth;
    public float canvasHeight;

    public static Canvas canvasRef;
    public static GameObject fallingObject;

    public static float width, height;

    

	// Use this for initialization
	void Start () {

        SpawnObject();

        canvasRef = canvas;
	}
    public void SpawnObject()
    {
        fallingObject = Instantiate(Resources.Load("Prefabs/FallingObject")) as GameObject;
        Text text;
        text = fallingObject.GetComponentInChildren<Text>();
        
        switch ((int)Mathf.Ceil(Random.Range(1, 5)))
        {
            case 1:
                fallingObject.AddComponent<ClickEventScript>().ObjectManager = this;
                text.text = "Clickable";
                break;
            case 2:
                fallingObject.AddComponent<SwipeEventScript>().ObjectManager = this;
                text.text = "Swipeable";
                break;
            case 3:
                RubEventScript rub = fallingObject.AddComponent<RubEventScript>();
                rub.requiredDistance = maxDistance;
                rub.ObjectManager = this;
                text.text = "Rub";
                break;
            case 4:
                fallingObject.AddComponent<DoubleClickEventScript>().ObjectManager = this;
                text.text = "Double Clickable";
                break;
            default:
                text.text = "Does not work!";
                break;
        }

        fallingObject.transform.SetParent(canvas.transform);
        fallingObject.transform.SetAsFirstSibling();

        width = canvas.GetComponent<RectTransform>().rect.width;
        height = canvas.GetComponent<RectTransform>().rect.height;

        Debug.Log("Width: " + width + " Height: " + height);

        fallingObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        fallingObject.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-width / 2, width / 2 - 100), height / 2 + 100);
    }
    public static void Rotated()
    {
        /*
        if(fallingObject != null)
        {
            
            Vector3 lastPosition = fallingObject.GetComponent<RectTransform>().localPosition;

            float oldWidth = (lastPosition.x + (width / 2)) / width;
            Debug.Log("OldWidth: "+ oldWidth);
            float oldHeight = (lastPosition.y + (height / 2)) / height;
            Debug.Log("OldHeight: " + oldHeight);

            //Vector3 newPosition = new Vector3();

            //fallingObject.GetComponent<RectTransform>().localPosition = newPosition;
        }
         */
    }
	public void ObjectClicked(GameObject go)
    {
        Destroy(go);
        SpawnObject();
    }
    public void ObjectSwiped(GameObject go)
    {
        /*GameObject anim = */
        Instantiate(Resources.Load<GameObject>("Prefabs/Swipe_Animation"), go.transform.position, Quaternion.identity, canvas.transform);
        
        Destroy(go);
        SpawnObject();
    }
    public void ObjectRubbed(GameObject go)
    {
        Destroy(go);
        SpawnObject();
    }
    
    // Update is called once per frame
    void Update () {
        if (paused) return;
        if(fallingObject.GetComponent<RectTransform>().localPosition.y<-height/2)
        {
            //Debug.Log(fallingObject.GetComponent<FallingObjectOnClickEventScript>().distance);
            Destroy(fallingObject);
            SpawnObject();
            
        }
        
        fallingObject.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        localPositionX = fallingObject.GetComponent<RectTransform>().localPosition.x;
        localPositionY = fallingObject.GetComponent<RectTransform>().localPosition.y;

        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
	}
}
