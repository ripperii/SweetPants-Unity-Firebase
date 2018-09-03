using Assets.Scripts;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenShopEquipmentMenu : MonoBehaviour
{

    public Button close;
    public Button open;
    public float duration;
    public static string selectedItem;
    public FadeInOutScript fade;

    public GameObject equipmentPrefab;
    public GameObject infoContextPrefab;

    public Transform holder;
    public List<EquipmentSlot> slots;

    public static Dictionary<Item, GameObject> itemGameObjects;

    public Text debug;

    public Player player;

    void Awake()
    {
        open.onClick.RemoveAllListeners();
        open.onClick.AddListener(OpenShopEquipment);

        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseShopEquipmentMenu);

        itemGameObjects = new Dictionary<Item, GameObject>();
    }
    void CloseShopEquipmentMenu()
    {
        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));
        foreach (Transform child in holder.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(EquipmentSlot es in transform.GetComponentsInChildren<EquipmentSlot>().Where(x => null != x.equiped ))
        {
            es.UnequipItemInSlot();
        }
        Router.GetEquipment(DatabaseManager.user.UserId).ValueChanged -= GetEquipedItemsEvent;
        Router.GetInventory(DatabaseManager.user.UserId).ValueChanged -= GetInventoryItemsListEvent;
    }
    void OpenShopEquipment()
    {
        player = Player.currentPlayer;

        Router.GetEquipment(DatabaseManager.user.UserId).ValueChanged += GetEquipedItemsEvent;
        Router.GetInventory(DatabaseManager.user.UserId).ValueChanged += GetInventoryItemsListEvent;

        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
    }
    public void GetInventoryItemsListEvent(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("GetInventoryItemsListEvent Called!");
        IDictionary<Item, int> temp = new Dictionary<Item, int>();
        
        foreach (DataSnapshot itemNode in args.Snapshot.Children)
        {
            if (itemNode.Key == "owned") continue;

            Item it = Items.GetItemFromList(itemNode.Key);

            temp.Add(it, int.Parse(itemNode.Value.ToString()));
        }
        player.Inventory = temp;

        for (int i = 0; i < holder.childCount; i++)
            Destroy(holder.GetChild(i).gameObject);

        itemGameObjects.Clear();
        Debug.Log("Loading Equipment...");
        foreach (var i in player.Inventory)
        {
            InstantiateEquipmentItem(i.Key, i.Value, holder, equipmentPrefab);
        }
    }
    public void GetEquipedItemsEvent(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("GetEquipedItemsEvent Called!");
        IDictionary<string, Item> temp = new Dictionary<string, Item>();

        foreach (DataSnapshot itemNode in args.Snapshot.Children)
        {
            Item it = Items.GetItemFromList(itemNode.Value.ToString());

            temp.Add(itemNode.Key, it);
        }

        player.Equipment = temp;

        foreach (EquipmentSlot es in slots)
        {
            es.WipeSlot();
        }
        if (null != player.Equipment)
        {
            foreach (var e in player.Equipment)
            {
                transform.GetChild(0).Find(e.Key).GetComponent<EquipmentSlot>().EquipItemInSlot(e.Value);
            }
        }
    }

    public static GameObject InstantiateEquipmentItem(Item item, int amount, Transform parent, GameObject prefab)
    {
        if (item.equipable && item.type == "Shop Equipment")
        {
            GameObject go = Instantiate(prefab, parent);

            go.name = item.id;

            Image bg = go.transform.GetChild(0).GetComponent<Image>();
            Image icon = go.transform.GetChild(1).GetComponent<Image>();

            icon.sprite = Resources.Load<Sprite>(item.icon);

            go.transform.GetChild(2).GetComponent<Text>().text = item.name;
            go.transform.GetChild(2).GetComponent<Text>().color = bg.color = ItemRarityColors.colors.Where(x => x.Key.ToLower() == item.rarity.ToLower()).FirstOrDefault().Value;

            if (amount > 1)
            {
                go.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = amount.ToString();
            }

            //Stats Loading....
            if (item.itemStats.Count > 0)
            {
                string stats = "";

                foreach (var stat in item.itemStats)
                {
                    stats += stat.name + ": " + stat.value + "\n";
                }

                go.transform.GetChild(3).GetComponent<Text>().text = stats;
            }
            go.GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<OpenShopEquipmentMenu>().EquipItem(item));
            Debug.Log("Equipment Item Name: " + go.name);

            itemGameObjects.Add(item, go);

            return go;
        }
        return null;

    }
    public void EquipItem(Item item)
    {
        EquipmentSlot slot;

        /*
        switch(item.subtype)
        {
            case "Curtains" :
                {
                    slot = slots[0].GetComponent<EquipmentSlot>();
                    
                    slots[0].GetComponent<EquipmentSlot>().EquipItemInSlot(item);
                    slotName = slots[0].name;
                    break;
                }
            case "Carpet":
                {
                    slots[4].GetComponent<EquipmentSlot>().UnequipItemInSlot();
                    slots[4].GetComponent<EquipmentSlot>().EquipItemInSlot(item);
                    slotName = slots[4].name;
                    break;
                }
            case "Wallpaper":
                {

                    slots[5].GetComponent<EquipmentSlot>().UnequipItemInSlot();
                    slots[5].GetComponent<EquipmentSlot>().EquipItemInSlot(item);
                    slotName = slots[5].name;
                    break;
                }
            case "Floor":
                {
                    slots[6].GetComponent<EquipmentSlot>().UnequipItemInSlot();
                    slots[6].GetComponent<EquipmentSlot>().EquipItemInSlot(item);
                    slotName = slots[6].name;
                    break;
                }
            case "Showcase":
                {
                    slots[1].GetComponent<EquipmentSlot>().UnequipItemInSlot();
                    slots[1].GetComponent<EquipmentSlot>().EquipItemInSlot(item);
                    slotName = slots[1].name;
                    break;
                }
            case "VendingMachine":
                {
                    slots[7].GetComponent<EquipmentSlot>().UnequipItemInSlot();
                    slots[7].GetComponent<EquipmentSlot>().EquipItemInSlot(item);
                    slotName = slots[7].name;
                    break;
                }
            default: Debug.Log("Error: Incorrect subtype in EquipItem button!"); break;

        }
        
        foreach(var s in slots)
        {
            Debug.Log("Slot Name: " + s.name);
        }

        Debug.Log("Item Subtype: " + item.subtype);
        */
        List <EquipmentSlot> temp = slots.Where(x => x.name.ToLower().IndexOf(item.subtype.ToLower()) != -1).ToList();

        Debug.Log("Item Subtype: " + item.subtype + " Equipment Slots Count: " + temp.Count);

        if(temp.Count > 1)
        {
            slot = temp.Where(x => null == x.equiped).FirstOrDefault();
            if(slot == null)
            {
                slot = temp.FirstOrDefault();
            }
        }
        else
        {
            slot = temp.FirstOrDefault();
        }
        
        if (null != slot.equiped)
        {
            Debug.Log("Slot has Item!");
            slot.icon.color = new Color(0, 0, 0, 0);
            player.Inventory[slot.equiped]++;
            slot.equiped = null;
        }

        player.Inventory[item]--;
        if (player.Inventory[item] == 0)
        {
            Debug.Log("Object should be destroyed!");
            Debug.Log("GO Name: " + itemGameObjects[item].name);
            Destroy(itemGameObjects[item]);
        }

        slot.EquipItemInSlot(item);

        DatabaseManager.sharedInstance.UpdateEquipment(item, slot.name);
        DatabaseManager.sharedInstance.UpdateInventoryItem(item, player.Inventory[item]);
        
    }
    
}