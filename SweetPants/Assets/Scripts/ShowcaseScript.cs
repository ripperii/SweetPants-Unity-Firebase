using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;

public class ShowcaseScript : MonoBehaviour {

    public Button close;
    public Button open;
    public float duration;
    public FadeInOutScript fade;
    public GameObject itemHolder;

    public GameObject itemPrefab;

    public static string selectedItem;


    void Awake()
    {
        open.onClick.RemoveAllListeners();
        open.onClick.AddListener(OpenShowcaseMenu);

        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseShowcaseMenu);
    }
    void CloseShowcaseMenu()
    {
        for (int i = 0; i < itemHolder.transform.childCount; i++)
            Destroy(itemHolder.transform.GetChild(i).gameObject);

        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));

    }
    void OpenShowcaseMenu()
    {
        Debug.Log("Showcase Opened!");

        for (int i = 0; i < itemHolder.transform.childCount; i++)
            Destroy(itemHolder.transform.GetChild(i).gameObject);
        
        IDictionary<string, ShowcaseItem> showcaseItems = Player.currentPlayer.ShowcaseItems;
        int itemSlots = Player.currentPlayer.GetShowcaseSlotsAmount();

        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = showcaseItems.Count + " / " + itemSlots;
        Debug.Log("Showcase Items Count: " + showcaseItems.Count);
        if (showcaseItems.Count > 0)
        {
            foreach(var item in showcaseItems)
            {
                GameObject go = InstantiateItem(item.Value, itemPrefab, itemHolder.transform);
                
                go.GetComponent<Button>().onClick.RemoveAllListeners();
                //AddListener(go.GetComponent<Button>(), 1);
            }
        }
        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
    }
    public GameObject InstantiateItem(ShowcaseItem it, GameObject prefab, Transform holder)
    {
        GameObject go = Instantiate(prefab, holder);
        
        Image bg = go.transform.GetChild(0).GetComponent<Image>();
        Image img = go.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>(it.item.icon);

        go.transform.GetChild(1).GetComponent<Text>().color = bg.color = ItemRarityColors.colors.Where(x => x.Key.ToLower() == it.item.rarity.ToLower()).FirstOrDefault().Value;

        go.transform.GetChild(1).GetComponent<Text>().text = it.item.name;
        go.transform.GetChild(2).GetComponent<Text>().text += it.item.type;
        go.transform.GetChild(3).GetComponent<Text>().text += it.item.subtype;
        go.transform.GetChild(4).GetComponent<Text>().text = it.price.ToString();
        
        img = go.transform.GetChild(5).GetComponent<Image>();

        switch (it.currency)
        {
            case "gold":
                img.sprite = Resources.Load<Sprite>("Rewards_Coins");
                img.color = Color.yellow;
                break;
            case "diamonds":
                img.sprite = Resources.Load<Sprite>("Rewards_Diamond");
                img.color = Color.cyan;
                break;
        }
        go.GetComponent<Button>().onClick.AddListener(() => ContextMenu(it));
        
        return go;
    }

    public void ContextMenu(ShowcaseItem it)
    {

        GameObject context = Instantiate((GameObject)Resources.Load("Prefabs/ContextMenu/ContextMenu"), transform);

        //GameObject 

        context.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        //go.GetComponent<CloseContextMenuScript>().id = i.ToString();

        Debug.Log(Input.mousePosition);

    }
}
