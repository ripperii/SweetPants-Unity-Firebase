using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Assets.Scripts;
using System.Linq;

public class CloseStorageMenuScript : MonoBehaviour {

    public Button close;
    public Button open;
    public float duration;
    public GameObject itemHolder;
    public static string selectedItem;
    public FadeInOutScript fade;

    public GameObject itemPrefab;
    public GameObject contextPrefab;
    public GameObject showcasePopupPrefab;

    void Awake()
    {
        open.onClick.RemoveAllListeners();
        open.onClick.AddListener(OpenStorageMenu);
        
        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseStorageMenu);

    }
    void CloseStorageMenu()
    {
        Router.GetInventory(DatabaseManager.user.UserId).ValueChanged -= GetStorageItemsEvent;
        
        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(),duration));
    }
    void OpenStorageMenu()
    {
        Router.GetInventory(DatabaseManager.user.UserId).ValueChanged += GetStorageItemsEvent;
        
        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
    }
    public void GetStorageItemsEvent(object sender, ValueChangedEventArgs args)
    {
        for (int i = 0; i < itemHolder.transform.childCount; i++)
            Destroy(itemHolder.transform.GetChild(i).gameObject);

        IDictionary<Item, int> storageItems = new Dictionary<Item, int>();

        foreach (DataSnapshot itemNode in args.Snapshot.Children)
        {
            if (itemNode.Key == "owned") continue;

            Item it = Items.GetItemFromList(itemNode.Key);

            storageItems.Add(it, int.Parse(itemNode.Value.ToString()));
        }

        Player.currentPlayer.Inventory = storageItems;

        if (storageItems.Count > 0)
        {
            foreach (var item in storageItems)
            {
                if (item.Key.type != "Currency")
                {
                    GameObject go = InstantiateItem(item.Key, item.Value, itemPrefab);

                    go.GetComponent<Button>().onClick.RemoveAllListeners();
                    go.GetComponent<Button>().onClick.AddListener(() => ContextMenu.InstantiateContextMenu(item.Key,contextPrefab,transform));
                }
            }
        }

        transform.GetChild(0).gameObject.SetActive(true);
    }
    public GameObject InstantiateItem(Item it, int amount, GameObject prefab)
    {
        GameObject go = Instantiate(prefab, itemHolder.transform);
        
        Image img = go.transform.GetChild(1).GetComponent<Image>();
        Image bg = go.transform.GetChild(0).GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>(it.icon);

        go.transform.GetChild(2).GetComponent<Text>().text = it.name;
        go.transform.GetChild(2).GetComponent<Text>().color = bg.color = ItemRarityColors.colors.Where(x => x.Key.ToLower() == it.rarity.ToLower()).FirstOrDefault().Value;

        go.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = amount.ToString();
        go.transform.GetChild(3).GetComponent<Text>().text += it.type;
        go.transform.GetChild(4).GetComponent<Text>().text += it.subtype;

        go.GetComponent<Button>().onClick.AddListener(() => ContextMenu.InstantiateContextMenu(it,contextPrefab, transform));

        return go;
    }
    /*
    public void ContextMenu(Item it)
    {
        
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/ContextMenu/ContextMenu"), transform);

        GameObject inspect = Instantiate(Resources.Load<GameObject>("Prefabs/ContextMenu/Inspect"), go.transform.GetChild(0));
        GameObject putOnShowcase = Instantiate(Resources.Load<GameObject>("Prefabs/ContextMenu/PutOnShowcase"), go.transform.GetChild(0));
        GameObject Equip;
        if (it.type.ToLower() == "equipment")
             Equip = Instantiate(Resources.Load<GameObject>("Prefabs/ContextMenu/Equip"), go.transform.GetChild(0));

        RectTransform rt = go.transform.GetChild(0).GetComponent<RectTransform>();

        rt = ContextMenuPosition.SetPivot(rt);


        //go.GetComponent<CloseContextMenuScript>().id = i.ToString();

        //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));

    }*/
}
