using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerMenu: MonoBehaviour
{
    private Adventurer adventurer = null;

    public FadeInOutScript fade;
    public Transform OfferItemHolder;
    public GameObject OfferItemPrefab;
    public float duration;
    public float red = 0, green = 0, blue = 0;
    public InputField input;
    public UserDataUpdateScript udus;
    public float baseValue = 0;

    //Buttons
    public Button accept;
    public Button decline;
    public Button minus;
    public Button plus;

    void Awake()
    {

        plus.onClick.RemoveAllListeners();
        plus.onClick.AddListener(PlusButton);

        minus.onClick.RemoveAllListeners();
        minus.onClick.AddListener(MinusButton);

        decline.onClick.RemoveAllListeners();
        decline.onClick.AddListener(DeclineButton);
        
        input.onValueChanged.AddListener(ChangeInputColor);
    }

    public void OpenAdventurerMenu(Adventurer adv)
    {
        try
        {

            for (int i = 0; i < OfferItemHolder.transform.childCount; i++)
                Destroy(OfferItemHolder.transform.GetChild(i).gameObject);

            if(!plus.gameObject.activeSelf && !minus.gameObject.activeSelf && !input.gameObject.activeSelf)
            {
                plus.gameObject.SetActive(true);
                minus.gameObject.SetActive(true);
                input.gameObject.SetActive(true);
            }
            adventurer = adv;
            
            accept.onClick.RemoveAllListeners();
            accept.onClick.AddListener(() => AcceptButton(adv.type));

            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = adv.name;
            transform.GetChild(1).GetComponent<Text>().text = adv.line;
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(adv.image);

            string offer = "";
            
            switch (adv.type)
            {
                case 1:
                
                    foreach (var it in adv.GetItems())
                    {
                        InstantiateOfferItem(it.Key, it.Value);
                    }
                    baseValue = adv.GetItems().Sum(x => x.Key.baseValue * x.Value);
                    input.text = baseValue.ToString();
                
                    break;
                case 2:
                    DatabaseManager.sharedInstance.GetShowcaseItems(result =>
                    {
                        ShowcaseItem si = result.Where(x => x.Key == adv.buyItem).FirstOrDefault().Value;
                        InstantiateOfferItem(si.item, 1);
                        baseValue = si.price;
                        input.text = baseValue.ToString();
                    });

                    break;
                case 3:
                    Debug.Log("Adventurer with Type 3 came in!");
                    plus.gameObject.SetActive(false);
                    minus.gameObject.SetActive(false);
                    input.gameObject.SetActive(false);

                    foreach (var item in adv.order.orderItems)
                    {
                        if(item.itemType != "")
                        {
                            offer = item.itemType;
                        }
                        else if(null != item.item)
                        {
                            InstantiateOfferItem(item.item, item.reqItems);
                        }
                        else
                        {
                            Debug.Log("Order Item is Invalid!");
                        }
                    }
                    break;
                default:
                    Debug.Log("Unknown type recieved!");
                    break;
            }

            transform.GetChild(3).GetComponent<Text>().text = offer;
            StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), duration));
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
    public void ChangeInputColor(string text)
    {
        Debug.Log("Text in Event: " + text);
        float f;
        if(!float.TryParse(text, out f))
        {
            f = 1;
            input.text = "1";
        }
        if(f < 1)
        {
            f = 1;
            input.text = "1";
        }
        if(f > Player.currentPlayer.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value)
        {
            f = Player.currentPlayer.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value;
            input.text = Player.currentPlayer.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value.ToString();
        }
        if (f == baseValue)
        {
            green = red = 1f;
        }
        if (f > baseValue)
        {
            red = 1 - ((f - baseValue) / baseValue);
            green = 1f;
        }
        else if ( f < baseValue)
        {
            green = f / baseValue;
            red = 1f;
        }

        input.GetComponent<Image>().color = new Color(red, green,0);
    }
    public void AcceptButton(int type)
    {
        Debug.Log("Accept Button Clicked!");
        IDictionary<string, object> temp = new Dictionary<string, object>();

        switch (type)
        {
            case 1:
                
                foreach (var it in adventurer.GetItems())
                {
                    if (Player.currentPlayer.Inventory.ContainsKey(it.Key))
                    {
                        temp.Add(it.Key.id, Player.currentPlayer.Inventory[it.Key] + it.Value);
                    }
                    else
                    {
                        temp.Add(it.Key.id, it.Value);
                    }
                }
                temp.Add("2", Player.currentPlayer.Inventory[Items.GetItemFromList("2")] - int.Parse(input.text));

                DatabaseManager.sharedInstance.UpdateInventoryItem(temp).ContinueWith(task =>
                {
                    Debug.Log("Adventurer Menu - Gold: " + Player.currentPlayer.Inventory[Items.GetItemFromList("2")]);
                    Player.currentPlayer.Inventory[Items.GetItemFromList("2")] -= int.Parse(input.text);
                    // Add XP Boosting here
                    int xp = Player.currentPlayer.XP + (int)Math.Ceiling(RemoteConfig.XPPerGold * baseValue);
                    DatabaseManager.sharedInstance.UpdateXP(xp);

                    DatabaseManager.sharedInstance.RemovePendingAdventurer();
                });
                break;
            case 2:

                temp.Add("2", Player.currentPlayer.Inventory[Items.GetItemFromList("2")] - int.Parse(input.text));
                DatabaseManager.sharedInstance.UpdateInventoryItem(temp).ContinueWith(task =>
                {
                    Player.currentPlayer.Inventory[Items.GetItemFromList("2")] -= int.Parse(input.text);
                    // Add XP Boosting here
                    int xp = Player.currentPlayer.XP + (int)Math.Ceiling(RemoteConfig.XPPerGold * baseValue);
                    DatabaseManager.sharedInstance.UpdateXP(xp);
                    DatabaseManager.sharedInstance.RemoveShowcaseItem(adventurer.buyItem);

                    DatabaseManager.sharedInstance.RemovePendingAdventurer();
                });
                
                break;
            case 3:

                break;
        }

        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));

    }
    public void DeclineButton()
    {
        Debug.Log("Decline Button Clicked!");
        
        DatabaseManager.sharedInstance.RemovePendingAdventurer();
        StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), duration));
    }
    public void PlusButton()
    {
        input.text = (int.Parse(input.text) + 10).ToString();
    }
    public void MinusButton()
    {
        input.text = (int.Parse(input.text) - 10).ToString();
    }
    public GameObject InstantiateOfferItem(Item it, int amount)
    {
        GameObject go = Instantiate(OfferItemPrefab, OfferItemHolder);
        go.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(it.icon);
        go.GetComponentInChildren<Text>().text = amount.ToString();

        switch (it.rarity)
        {
            case "common":
                go.transform.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f);
                break;
            case "uncommon":
                go.transform.GetComponent<Image>().color = Color.green;
                break;
            case "rare":
                go.transform.GetComponent<Image>().color = Color.blue;
                break;
            case "epic":
                go.transform.GetComponent<Image>().color = Color.magenta;
                break;
            case "legendary":
                go.transform.GetComponent<Image>().color = Color.yellow;
                break;
        }
        return go;
    }

}
