using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player {

    public static Player currentPlayer;

    public string Username;
    public string ShopTimer;
    public int XP;
    public int ShopTimerSkipedCount;
    public int RewardableVideosWatched;
    public IDictionary<Item,int> Inventory;
    public IDictionary<string, Item> Equipment;
    public IDictionary<string, ShowcaseItem> ShowcaseItems;
    public List<Order> Orders;
    public Adventurer pendingAdventurer;

    public Player(string username, DateTime shopTimer, int xp, int stsc, int rvw)
    {
        Username = username;
        ShopTimer = shopTimer.ToString();
        XP = xp;
        ShopTimerSkipedCount = stsc;
        RewardableVideosWatched = rvw;
    }

    public Player(IDictionary<string, object> dict)
    {
        Username = dict["Username"].ToString();
        ShopTimer = dict["ShopTimer"].ToString();
        XP = int.Parse(dict["XP"].ToString());
        ShopTimerSkipedCount = int.Parse(dict["ShopTimerSkipedCount"].ToString());
        RewardableVideosWatched = int.Parse(dict["RewardableVideosWatched"].ToString());
        Inventory = new Dictionary<Item, int>();
        Equipment = new Dictionary<string, Item>();
        ShowcaseItems = new Dictionary<string, ShowcaseItem>();
    }
    public void SetShopTimer(DateTime time)
    {
        this.ShopTimer = time.ToString();
    }
    public DateTime GetShopTimer()
    {
        return DateTime.Parse(ShopTimer);
    }
    public int GetShowcaseSlotsAmount()
    {
        int slots = 0;
        foreach (var equipment in Equipment)
        {
            if (equipment.Value.itemStats.Where(x => x.name == "slots").ToList().Count > 0)
            {
                slots += equipment.Value.itemStats.Where(x => x.name == "slots").FirstOrDefault().value;
            }
        }
        return slots;
    }
    public Player AddStarterItems()
    {
        Debug.Log("Add Starter Items Method called!");

        if (Items.items.Count <= 0)
            DatabaseManager.sharedInstance.GetAllItems(result =>
            {
                Items.items = result;
            });

        IDictionary<string, object> temp = new Dictionary<string, object>()
        {
            { "1", RemoteConfig.StartingDiamondsAmount },
            { "2", RemoteConfig.StartingGoldAmount }
        };

        DatabaseManager.sharedInstance.CreateInventoryWithStarterItems(temp);
        return currentPlayer;
    }

    public Player UpdatePlayerInventory()
    {
        Debug.Log("Update Player Inventory Method called!");
        DatabaseManager.sharedInstance.GetInventoryItems(result =>
        {
            UpdateCurrentPlayer().Inventory = result;
        });
        return currentPlayer;
    }
    public Player UpdatePlayerEquipment()
    {
        Debug.Log("Update Player Equipment Method called!");
        DatabaseManager.sharedInstance.GetEquipment(result =>
        {
            UpdateCurrentPlayer().Equipment = result;
        });
        return currentPlayer;
    }
    public Player UpdatePlayerShowcaseItems()
    {
        Debug.Log("Update Player Showcase Items Method called!");
        DatabaseManager.sharedInstance.GetShowcaseItems(result =>
        {
            UpdateCurrentPlayer().ShowcaseItems = result;
        });
        return currentPlayer;
    }
    public Player UpdatePlayerPendingAdventurer()
    {
        Debug.Log("Update Player Pending Adventurer Method called!");
        DatabaseManager.sharedInstance.GetPendingAdventurer(result =>
        {
            UpdateCurrentPlayer().pendingAdventurer = result;
        });
        return currentPlayer;
    }
    public Player UpdateCurrentPlayer()
    {
        if (null == currentPlayer)
        {
            Debug.Log("Update Current Player Method Called!");
            DatabaseManager.sharedInstance.GetPlayer(player => {
                currentPlayer = player;
            });
        }
        return currentPlayer;
    }
    public Player UpdatePlayerOrders()
    {
        Debug.Log("Update Player Orders Method called!");

        currentPlayer.Orders = new List<Order>();

        DatabaseManager.sharedInstance.GetPlayerOrders(result =>
        {
            Debug.Log("Orders Amount: " + result.Count);

            UpdateCurrentPlayer().Orders = result;
        });
        return currentPlayer;
    }
}
