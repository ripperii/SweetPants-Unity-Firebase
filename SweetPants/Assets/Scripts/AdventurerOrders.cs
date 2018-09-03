using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerOrders : MonoBehaviour {

    public FadeInOutScript fade;
    public float duration;
    public CanvasGroup cg;

    public Button close;
    public Button open;

    private List<Order> orders;
    private Dictionary<Text, TimeSpan> timers;
    public Transform orderHolder;
    public GameObject orderItemPrefab;
    public GameObject orderPrefab;

    private void Awake()
    {
        close.onClick.RemoveAllListeners();
        open.onClick.RemoveAllListeners();

        close.onClick.AddListener(OpenOrdersMenu);
        open.onClick.AddListener(CloseOrderMenu);
    }
    private void Update()
    {
        if(timers.Count > 0)
        {
            foreach(var timer in timers)
            {
                TimeSpan t = timer.Value.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
                timer.Key.text = String.Format("{0}:{1}:{2}", t.Hours, t.Minutes, t.Seconds);
            }
        }
    }
    public void OpenOrdersMenu()
    {
        orders = new List<Order>();
        timers = new Dictionary<Text, TimeSpan>();

        DatabaseManager.sharedInstance.GetPlayerOrders(result =>
        {
            foreach (Order o in result)
            {
                Order temp = new Order(o.id, o.adventurer, o.timer, o.orderItems);
                InstantiateOrder(orderHolder, orderPrefab, orderItemPrefab, temp);
                orders.Add(temp);
            }

            Player.currentPlayer.Orders = orders;
            StartCoroutine(fade.fadeIn(cg, duration));
        });
    }

    public void CloseOrderMenu()
    {
        for (int i = 0; i < orderHolder.childCount; i++)
            Destroy(orderHolder.GetChild(i).gameObject);

        StartCoroutine(fade.fadeOut(cg, duration));
    }
    public GameObject InstantiateOrder(Transform orderHolder, GameObject orderPrefab, GameObject orderItemPrefab, Order o)
    {
        GameObject order = Instantiate(orderPrefab, orderHolder);

        order.transform.GetChild(0).GetComponent<Text>().text = o.adventurer.name;
        timers.Add(order.transform.GetChild(1).GetComponent<Text>(), o.timer);

        Transform orderItemsHolder = order.transform.GetChild(2).transform;

        for (int i = 0; i < o.orderItems.Count; i++)
        {
            GameObject orderItem = Instantiate(orderItemPrefab, orderItemsHolder);

            if (o.orderItems[i].itemType != "")
            {
                o.orderItems[i].ownedItems = Player.currentPlayer.Inventory[o.orderItems[i].item];
            }
            else
            {
                o.orderItems[i].ownedItems = Player.currentPlayer.Inventory.Where(it => it.Key.subtype == o.orderItems[i].itemType).Sum(x => x.Value);
            }
            orderItem.transform.GetChild(0).GetComponent<Text>().text = o.orderItems[i].item != null ? o.orderItems[i].item.name : o.orderItems[i].itemType;
            orderItem.transform.GetChild(1).GetComponent<Text>().text = o.orderItems[i].ownedItems + "/" + o.orderItems[i].reqItems;
        }
        
        return order;
    }
}

