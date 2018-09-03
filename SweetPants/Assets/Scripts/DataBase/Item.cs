using Firebase.Database;
using System;
using System.Collections.Generic;
using UnityEditor;

public class Items
{
    public static List<Item> items;

    public static Item GetItemFromList(string id)
    {
        return items.Find(x => x.id == id);
    }
    public static bool TryGetItemFromList(string id, out Item item)
    {
        item = items.Find(x => x.id == id);

        return null != item;
    }
    public static void AddItemToList(Item it)
    {
        items.Add(it);
    }

}

public class Item
{
    public string id;
    public string name;
    public string description;
    public string type;
    public string subtype;
    public bool equipable;
    public string rarity;
    public string icon;
    public int baseValue;
    public List<ItemStat> itemStats;

    public Item(string id, string name, string type, string subtype, bool equipable, string rarity, string icon, List<ItemStat> itemStats, string description, int baseValue)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        this.subtype = subtype;
        this.equipable = equipable;
        this.rarity = rarity;
        this.icon = icon;
        this.baseValue = baseValue;
        this.itemStats = itemStats;
    }

    public Item(string id, IDictionary<string, object> dict)
    {
        this.id = id;
        name = dict["name"].ToString();
        description = dict["description"].ToString();
        type = dict["type"].ToString();
        subtype = dict["subtype"].ToString();
        equipable = bool.Parse(dict["equipable"].ToString());
        rarity = dict["rarity"].ToString();
        icon = dict["icon"].ToString();
        baseValue = int.Parse(dict["baseValue"].ToString());
        itemStats = new List<ItemStat>();

        foreach(var stat in (IDictionary<string, object>)dict["stats"])
        {
            itemStats.Add(new ItemStat(stat.Key, int.Parse(stat.Value.ToString())));
        }

    }
    public static Item GetItemById(string id)
    {
        Item it = null;
        DatabaseManager.sharedInstance.GetItemById(item =>
        {
            it = item;
        }, id);
        return it;
    }

}
public class ItemStat
{
    public string name;
    public int value;

    public ItemStat(string name, int value)
    {
        this.name = name;
        this.value = value;
    }
}
public class ShowcaseItem
{
    public string slotName;
    public Item item;
    public int price;
    public string currency;

    public ShowcaseItem(Item item, string slotName, int price, string currency)
    {
        this.item = item;
        this.slotName = slotName;
        this.price = price;
        this.currency = currency;
    }
}