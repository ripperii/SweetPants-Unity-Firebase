using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Adventurer
{
    public string id;
    public string name;
    public string description;
    public string image;
    public string line;
    public string buyItem;
    public int type;
    public Order order;
    public IDictionary<string, string> items;

    public Adventurer()
    {
        type = 0;
        id = "";
        name = "";
        description = "";
        image = "";
        image = "";
        line = "";
        buyItem = "";
        order = new Order();
        items = new Dictionary<string, string>();
    }
    public Adventurer(string adv)
    {
        JObject obj = (JObject)JsonConvert.DeserializeObject(adv);
        Debug.Log("Adventurer JSON 2:" + obj.ToString());

        id = obj["id"].Value<string>();
        type = obj["type"].Value<int>();
        name = obj["name"].Value<string>();
        line = obj["line"].Value<string>();
        description = obj["description"].Value<string>();

        switch (type)
        {
            case 1:

                items = JsonConvert.DeserializeObject<Dictionary<string, string>>(obj["items"].ToString());

                break;
            case 2:

                buyItem = obj["buyItem"].Value<string>();

                break;
            case 3:
                Debug.Log("Current Time Adventurer: " + obj["currentTime"].Value<double>());
                DateTime now = TimeStamp.Convert(obj["currentTime"].Value<double>());
                Dictionary<string, string> temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(obj["order"].ToString());

                Item item;
                List<OrderItem> list = new List<OrderItem>();
                foreach(var i in temp)
                {
                    Dictionary<string, string> it = JsonConvert.DeserializeObject<Dictionary<string, string>>(i.Value);
                    if (Items.TryGetItemFromList(it["orderItemType"], out item))
                    {
                        list.Add(new OrderItem(item, "", int.Parse(temp["amount"])));
                    }
                    else
                    {
                        list.Add(new OrderItem(null, temp["orderItemType"], int.Parse(temp["amount"])));
                    }

                }

                Debug.Log("Orders Amount: " + Player.currentPlayer.Orders.ToString());
                order = new Order("",this,TimeSpan.Zero,list);

                Debug.Log("Order Timer: " + order.timer);

                break;
        }
    }
    public IDictionary<Item, int> GetItems()
    {
        IDictionary<Item, int> temp = new Dictionary<Item, int>();

        foreach(var it in items)
        {
            temp.Add(Items.GetItemFromList(it.Key), int.Parse(it.Value));
        }

        return temp;
    }
}
