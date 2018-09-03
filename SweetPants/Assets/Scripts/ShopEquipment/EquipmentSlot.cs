using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler {

    public Item equiped = null;
    public Image icon;
    public GameObject prefab;
    public Transform holder;
    public Text debug;

    private GameObject go = null;
    private bool hold = false;
    private float timer = 0f; 

    public void EquipItemInSlot(Item item)
    {
        Debug.Log("EquipItemInSlot - " + gameObject.name);
        equiped = item;

        icon.sprite = Resources.Load<Sprite>(equiped.icon);
        icon.color = new Color(255,0,0,255);
    }
    public void WipeSlot()
    {
        equiped = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    public void UnequipItemInSlot()
    {
        if (null == equiped) return;
        
        if(Player.currentPlayer.Inventory.ContainsKey(equiped))
        {
            Player.currentPlayer.Inventory[equiped] += 1;
            DatabaseManager.sharedInstance.UpdateInventoryItem(equiped, Player.currentPlayer.Inventory[equiped]);
        }
        else
        {
            Player.currentPlayer.Inventory.Add(equiped, 1);
            DatabaseManager.sharedInstance.UpdateInventoryItem(equiped, 1);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        timer = 0f;
        hold = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        hold = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*
        if(null != equiped)
        {
            go = OpenShopEquipmentMenu.InstantiateEquipmentItem(equiped, 0, transform.parent, prefab);

            if (Input.touchSupported)
                go.transform.position = Input.GetTouch(0).position;
            else
                go.transform.position = Input.mousePosition;
        }*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Destroy(go);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(timer<1)
        {
            Debug.Log("EquipmentSlot PointerUp Triggered!");
            DatabaseManager.sharedInstance.RemoveEquipment(transform.name);
            Player.currentPlayer.Equipment.Remove(transform.name);
           
            UnequipItemInSlot();
        }
        else
        {
            //DestroyImmediate(go);
        }
        timer = 0f;
        hold = false;
    }
    void Update()
    {
        if(hold)
        {
            timer += Time.deltaTime;
            if (timer > 1)
                if (null != equiped)
                {
                    if (null != go) return;
                    go = ContextMenu.InstantiateInfoContext(equiped, GetComponentInParent<OpenShopEquipmentMenu>().infoContextPrefab, transform.parent);
                }
        }
        if(Debug.isDebugBuild)
        {
            debug.text = timer.ToString();
            debug.text += " " + hold;
        }

    }
}
