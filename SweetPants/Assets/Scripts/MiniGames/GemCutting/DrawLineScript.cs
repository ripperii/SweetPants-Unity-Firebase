using UnityEngine;
using UnityEngine.UI;

public class DrawLineScript : MonoBehaviour {

    private Image image;
    public bool lineStarted = false;
    private RectTransform imageRectTransform;
    public float lineWidth = 5f;
    public Vector3 firstRing;
    public GameObject line;
    public GameObject oldLine;
    public GameObject startedFrom;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) && lineStarted)
        {
            DrawLine(firstRing, Input.mousePosition);
        }
        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("EndLine");
            EndLine();
        }
    }
    public void StartLine(Vector3 pos, GameObject sf)
    {
        if (!lineStarted)
        {
            Debug.Log("Start line");
            startedFrom = sf;
            line = Instantiate(Resources.Load("Prefabs/GemCuttingMiniGame/Line") as GameObject);
            image = line.GetComponent<Image>();
            image.transform.SetParent(this.GetComponent<RandomShapeCreator>().canvas.transform);
            imageRectTransform = image.rectTransform;
            firstRing = pos;
            lineStarted = true;
            oldLine = line;
        }
    }
    public void EndLine()
    {
        Destroy(line);
        lineStarted = false;
    }
    public void DrawLine(Vector3 pointA, Vector3 pointB)
    {
        Vector3 differenceVector = pointB - pointA;

        imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
        imageRectTransform.pivot = new Vector2(0, 0.5f);
        imageRectTransform.position = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
