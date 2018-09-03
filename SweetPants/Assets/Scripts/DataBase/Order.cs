using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Order
{
    public string id;
    public Adventurer adventurer;
    public TimeSpan timer;
    public List<OrderItem> orderItems;
    
    public Order(string id, Adventurer adv, TimeSpan timer, List<OrderItem> items)
    {
        this.id = id;
        this.timer = timer;
        this.adventurer = adv;
        this.orderItems = items;
    }
    public Order()
    {
        id = "";
        timer = TimeSpan.Zero;
        adventurer = null;
        orderItems = new List<OrderItem>();
    }
}
/*
public class InstOrder : MonoBehaviour
{
    public static void Instantiate(Transform orderHolder, GameObject orderPrefab, GameObject orderItemPrefab, Order o)
    {

        GameObject order = Instantiate(orderPrefab, orderHolder);

        o.adventurerNameText = order.transform.GetChild(0).GetComponent<Text>();
        o.timerText = order.transform.GetChild(1).GetComponent<Text>();
        o.orderItemsHolder = order.transform.GetChild(2).transform;

        for (int i = 0; i < o.orderItems.Count; i++)
        {
            GameObject orderItem = Instantiate(orderItemPrefab, o.orderItemsHolder);
            int ownedItems = 0;
            if (o.orderItems[i].itemType != "")
            {
                ownedItems = Player.currentPlayer.Inventory[o.orderItems[i].item];
            }
            else
            {
                ownedItems = Player.currentPlayer.Inventory.Where(it => it.Key.subtype == o.orderItems[i].itemType).Sum(x => x.Value);
            }

            o.orderItems[i].InstantiateOrderItem(orderItem.transform.GetChild(0).GetComponent<Text>(), orderItem.transform.GetChild(1).GetComponent<Text>(), ownedItems);
        }
    }
}
*/
public class OrderItem
{
    public Item item;
    public string itemType;
    public int reqItems;
    public int ownedItems;
    /*
    public void InstantiateOrderItem(Text itName, Text reqItem, int ownedIt)
    {
        ItemName = itName;
        RequiredItem = reqItem;
        ownedItems = ownedIt > reqItems ? reqItems : ownedIt;
        RequiredItem.text = ownedIt + "/" + reqItems;
    }
    */
    public OrderItem(Item it, string itType, int reqIt)
    {
        item = it;
        itemType = itType;
        reqItems = reqIt;
    }
}
