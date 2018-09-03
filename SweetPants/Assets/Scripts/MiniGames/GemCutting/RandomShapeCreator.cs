using UnityEngine;
using UnityEngine.UI;

public class RandomShapeCreator : MonoBehaviour {

    public Canvas canvas;
    private bool shapeCreated = false;
    private GameObject shapeHolder;
	// Use this for initialization
	void Start () {
        
        
    }
	
	// Update is called once per frame
	void Update () {

        if (!shapeCreated) 
            CreateShape();
		
	}
    void CreateShape()
    {
        switch (Random.Range(1, 4))
        {
            //Triangle
            case 1:
                shapeHolder = Instantiate(Resources.Load("Prefabs/GemCuttingMiniGame/TriangleShape") as GameObject,new Vector3(Screen.width/2,Screen.height/2),Quaternion.identity, canvas.transform);
                shapeHolder.transform.SetAsFirstSibling();

                break;
            //Square
            case 2:
                break;
        }
        shapeCreated = true;
    }
}
