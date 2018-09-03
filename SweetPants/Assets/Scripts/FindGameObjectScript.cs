using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGameObjectScript : MonoBehaviour {

    public static GameObject FindObject(GameObject parent, string name)
    {
        Component[] trs = parent.GetComponentsInChildren(typeof(Transform), true);
        
        foreach (Component t in trs)
        {
            if (t.transform.name == name)
            {
                return t.transform.gameObject;
            }
        }
        return null;
    }
}
