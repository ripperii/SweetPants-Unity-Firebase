using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using Firebase.Database;
using System.Collections.Generic;

public class UserDataUpdateScript : MonoBehaviour {

    public Text Level;
    public Text goldAmount;
    public Text diamondsAmount;
    public Player p;
    public CanvasGroup canvas;
    public FadeInOutScript fade;
    public float duration = 0.25f;

    public ClickOpenShopButtonScript cosbs;
    public XPBarScript XPBar;
    public AdventurerMenu advMenu;

    void Update()
    {

    }
    public void Start()
    {

        p = Player.currentPlayer;

        //p.UpdatePlayerInventory().UpdatePlayerOrders().UpdatePlayerPendingAdventurer();

        Router.GetInventory(DatabaseManager.user.UserId).ValueChanged += UpdateUserDataEvent;
        Router.GetEquipment(DatabaseManager.user.UserId).ValueChanged += UpdateUserEquipmentEvent;
        Router.PlayerWithUID(DatabaseManager.user.UserId).Child("XP").ValueChanged += UpdateUserXPEvent;
    }
    private void UpdateUserXPEvent(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("UpdateUserXP Event called!");
        int xp = Convert.ToInt32(args.Snapshot.Value);

        //Debug.Log("Updated XP: " + xp);
        //Debug.Log("Current XP: " + p.XP);

        int curXP = p.XP;
        p.XP = xp;
        XPBar.AddXP(curXP, xp);
    }
    private void UpdateUserEquipmentEvent(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("UpdateUserEquipment Event called!");
        foreach (DataSnapshot itemNode in args.Snapshot.Children)
        {
            Item it = Items.GetItemFromList(itemNode.Value.ToString());

            if (Player.currentPlayer.Equipment.ContainsKey(itemNode.Key))
            {
                if (Player.currentPlayer.Equipment[itemNode.Key] != it)
                {
                    Player.currentPlayer.Equipment[itemNode.Key] = it;
                }
            }
            else
            {
                Player.currentPlayer.Equipment.Add(itemNode.Key, it);
            }
        }
    }

    public void UpdateUserDataEvent(object sender, ValueChangedEventArgs args)
    {

        Debug.Log("UpdateUserData Event called!");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot playerItems = args.Snapshot;

        Debug.Log("ValueChangedEventArgs Snapshot: " + args.Snapshot.GetRawJsonValue());
        Debug.Log("Player Items exist: " + playerItems.HasChildren + " Children Count:" + playerItems.ChildrenCount);

        IDictionary<Item, int> temp = new Dictionary<Item, int>();

        foreach (DataSnapshot itemNode in playerItems.Children)
        {
            if (itemNode.Key == "owned") continue;

            Item it = Items.GetItemFromList(itemNode.Key);

            temp.Add(it, int.Parse(itemNode.Value.ToString()));
            Debug.Log("UpdateUserData Event: Item - " + it.name + " Amount - " + itemNode.Value.ToString());
        }

        p.Inventory = temp;

        UpdateUserData();
    }
    public void UpdateUserData()
    { 
        Debug.Log("UpdateUserData called!");
        
        foreach(var inv in p.Inventory)
        {
            Debug.Log("Inventory item: " + inv.Key.id + ": "+ inv.Key.name + " - " + inv.Value);
        }

        //cosbs.timer.gameObject.SetActive(true);
        //cosbs.openShopButton.interactable = false;

        //cosbs.SkipShopTimer.interactable = cosbs.SkipShopTimerImage.raycastTarget = !u.SkipShopTimerLimit;
        //Debug.Log("SkipShopTimerLimit: " + u.SkipShopTimerLimit);

        //VideoButton.interactable = !u.RewardableLimit;
        if(null == Levels.list || Levels.list.Count < 0)
        {
            Levels.GetLevels();
        }
        string lv = Levels.FindPlayerLevelString(p.XP);
        Level.text = lv;

        goldAmount.text = p.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value.ToString();
        diamondsAmount.text = p.Inventory.Where(x => x.Key.id == "1").FirstOrDefault().Value.ToString();

        Debug.Log("Gold Field: " + p.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value.ToString());
        Debug.Log("Diamonds Field: " + p.Inventory.Where(x => x.Key.id == "1").FirstOrDefault().Value.ToString());
        
        /*
        int curXP = (p.XP - Levels.list.Where(x => x.Key == int.Parse(lv)).FirstOrDefault().Value[1]);
        int reqXP = Levels.list.Where(x => x.Key == int.Parse(lv)).FirstOrDefault().Value[0];
        
        XPBar.UpdateXP(curXP,reqXP);
        */
    }
    public void ShowHUD()
    {
        Debug.Log("Is this ShowHUD called?");
        StartCoroutine(transform.GetComponent<FadeInOutScript>().fadeIn(canvas, duration));
    }
    public void OpenAdventurerMenuAfterAnimation()
    {
        if (p.pendingAdventurer != null)
        {
            //show adventurer dialog box
            advMenu.OpenAdventurerMenu(p.pendingAdventurer);
        }
    }
}
