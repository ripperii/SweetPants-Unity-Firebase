using System;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using Firebase.Auth;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database;

public class DatabaseManager : MonoBehaviour {

    public static DatabaseManager sharedInstance = null;
    public static FirebaseUser user = null;
    public static FirebaseApp app = null;

    void Awake () {
		if(sharedInstance == null)
        {
            sharedInstance = this;
        }else if(sharedInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //Debug.Log("FirebaseApp Name: " + FirebaseApp.DefaultInstance.Name);
        /*
        if(Application.isEditor)
        {
        */
            AppOptions options = new AppOptions
            {
                DatabaseUrl = new Uri("https://sweetpants-f86be.firebaseio.com/"),
                ApiKey = "AIzaSyCouarIG3ozowYHMUvobATg_0Dj_sn6jQk",
                AppId = "409092652571:android:5042a12f3ae39edc",
                ProjectId = "sweetpants-f86be",

            };

            app = FirebaseApp.Create(options,"EditorName");

            
            app.SetEditorDatabaseUrl("https://sweetpants-f86be.firebaseio.com/");
           // app.SetEditorP12FileName("SweetPants-1149947bc28e.p12");
           // app.SetEditorServiceAccountEmail("ripperii@sweetpants-f86be.iam.gserviceaccount.com");
           // app.SetEditorP12Password("notasecret");
            Debug.Log("FirebaseApp Name: " + app.Name);
        /*
        }
        else
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sweetpants-f86be.firebaseio.com/");
            // FirebaseApp.DefaultInstance.SetEditorP12FileName("SweetPants-1149947bc28e.p12");
            // FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("ripperii@sweetpants-f86be.iam.gserviceaccount.com");
            // FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
            Debug.Log("FirebaseApp Name: " + FirebaseApp.DefaultInstance.Name);
        }
        */
        //Router.Players().SetValueAsync("testing 1, 2");
        
	}
    #region Create/Push Methods
    public Task CreateNewPlayer(Player player, string uid)
    {
        string playerJSON = JsonUtility.ToJson(player);
        Debug.Log("JSON: " + playerJSON);
        return Router.PlayerWithUID(uid).SetRawJsonValueAsync(playerJSON);
    }
    public Task CreateInventoryWithStarterItems(IDictionary<string,object> dict)
    {
        return Router.PlayerWithUID(user.UserId).Child("Inventory").SetValueAsync(dict);
    }
    public Task AddItemToShowcase(Item it, string currency, int price)
    {
        
        IDictionary<string, object> temp = new Dictionary<string, object>
        {
            { "currency", currency },
            { "price", price },
            { "item", it.id }
        };

        return Router.GetShowcaseItems(user.UserId).Push().SetValueAsync(temp);
    }
    public Task AddOrder(Adventurer adv, DateTime timer, Dictionary<Item, int> items)
    {
        IDictionary<string, object> it = new Dictionary<string, object>();
        foreach(var item in items)
        {
            it.Add(item.Key.id, item.Value);
        }
        IDictionary<string, object> temp = new Dictionary<string, object>
        {
            { "adventurer", adv.id },
            { "timer",  timer.ToString()},
            { "OrderItems", it }
        };
        return Router.PlayerOrderById(user.UserId, Router.PlayerOrders(user.UserId).Push().Key).SetValueAsync(temp);
    }
    public Task PushItemInInventory(Item item, int amount)
    {
        return Router.GetInventory(user.UserId).Child(item.id).SetValueAsync(amount);
    }
    #endregion

    #region Get Methods

    public void GetPlayers(Action<List<Player>> CompletionBlock)
    {
        List<Player> temp = new List<Player>();

        Router.Players().GetValueAsync().ContinueWith(task => 
        {
            DataSnapshot players = task.Result;

            foreach(DataSnapshot playerNode in players.Children)
            {
                var playerDict = (IDictionary<string, object>)playerNode.Value;

                Player newPlayer = new Player(playerDict);
                temp.Add(newPlayer);
            }
            CompletionBlock(temp);
        });
    }
    public void GetAdventurerByID(string id, Action<Adventurer> CompletionBlock)
    {
        Adventurer adv = new Adventurer();

        Router.AdventurerByID(id).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot temp = task.Result;
            var advDict = (IDictionary<string, object>)temp.Value;

            adv.id = temp.Key;
            adv.name = advDict["name"].ToString();

            CompletionBlock(adv);
        });
    }
    public void GetInventoryItems(Action<IDictionary<Item, int>> CompletionBlock)
    {
        IDictionary<Item, int> temp = new Dictionary<Item, int>();
        
        Router.PlayerWithUID(user.UserId).Child("Inventory").GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot playerItems = task.Result;

            foreach (DataSnapshot itemNode in playerItems.Children)
            {
                if (itemNode.Key == "owned") continue;

                Item it = Items.GetItemFromList(itemNode.Key);
                
                temp.Add(it, int.Parse(itemNode.Value.ToString()));
            }
            CompletionBlock(temp);
        });
    }
    public void GetEquipment(Action<IDictionary<string, Item>> CompletionBlock)
    {
        IDictionary<string, Item> temp = new Dictionary<string, Item>();

        Router.PlayerWithUID(user.UserId).Child("Equipment").GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot playerItems = task.Result;

            foreach (DataSnapshot itemNode in playerItems.Children)
            {

                Item it = Items.GetItemFromList(itemNode.Value.ToString());

                temp.Add(itemNode.Key, it);
            }
            CompletionBlock(temp);
        });
    }
    public void GetShowcaseItems(Action<IDictionary<string, ShowcaseItem>> CompletionBlock)
    {
        IDictionary<string, ShowcaseItem> temp = new Dictionary<string, ShowcaseItem>();

        Router.PlayerWithUID(user.UserId).Child("ShowcaseItems").GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot slots = task.Result;

            foreach (DataSnapshot slot in slots.Children)
            {
                Item it = Items.GetItemFromList(slot.Child("item").Value.ToString());
                int price = int.Parse(slot.Child("price").Value.ToString());
                string currency = slot.Child("currency").Value.ToString();
                string slotName = slot.Key;
                temp.Add(slot.Key, new ShowcaseItem(it, slotName, price,currency));
            }
            CompletionBlock(temp);
        });
    }
    public void GetAllItems(Action<List<Item>> CompletionBlock)
    {
        List<Item> temp = new List<Item>();

        Router.Items().GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot items = task.Result;

            foreach (DataSnapshot itemNode in items.Children)
            {
                var itemDict = (IDictionary<string, object>)itemNode.Value;

                List<ItemStat> stats = new List<ItemStat>();

                if (itemNode.HasChild("stats"))
                {
                    foreach (var stat in (IDictionary<string, object>)itemDict["stats"])
                    {
                        stats.Add(new ItemStat(stat.Key, int.Parse(stat.Value.ToString())));
                    }
                }
                Item it = new Item(
                                       itemNode.Key,
                                       itemDict["name"].ToString(),
                                       itemDict["type"].ToString(),
                                       itemDict["subtype"].ToString(),
                                       itemNode.HasChild("equipable"),
                                       itemDict["rarity"].ToString(),
                                       itemDict["icon"].ToString(),
                                       stats,
                                       (itemNode.HasChild("Description") ? itemDict["Description"].ToString() : ""),
                                       int.Parse(itemDict["baseValue"].ToString())
                                   );   

                temp.Add(it);
            }
            CompletionBlock(temp);
        });
    }
    public void GetPendingAdventurer(Action<Adventurer> CompletionBlock)
    {
        Adventurer temp;
        Router.PendingAdventurer(user.UserId).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot adv = task.Result;

            temp = new Adventurer(adv.GetRawJsonValue());

            CompletionBlock(temp);
        });
    }
    public void GetPlayer(Action<Player> CompletionBlock)
    {
        Player temp;
        string getPlayer = "GetPlayer: ";
        Router.PlayerWithUID(user.UserId).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot p = task.Result;
            var playerDict = (IDictionary<string, object>)p.Value;
            temp = new Player(playerDict)
            {
                Orders = new List<Order>(),
                Equipment = new Dictionary<string, Item>(),
                ShowcaseItems = new Dictionary<string, ShowcaseItem>()
            };
            sharedInstance.GetInventoryItems(result =>
            {
                temp.Inventory = result;
            });
            if (p.HasChild("Equipment"))
            {
                getPlayer += " Equipment ";
                sharedInstance.GetEquipment(result =>
                {
                    temp.Equipment = result;
                });
            }
            if(p.HasChild("Orders"))
            {
                getPlayer += " Orders ";
                sharedInstance.GetPlayerOrders(result =>
                {
                    temp.Orders = result;
                });
            }
            if (p.HasChild("PendingAdventurer"))
            {
                getPlayer += " PendingAdventurer ";
                sharedInstance.GetPendingAdventurer(result =>
                {
                    temp.pendingAdventurer = result;
                });
            }
            Debug.Log(getPlayer);
            CompletionBlock(temp);
        });

    }
    public void GetPlayerOrders(Action<List<Order>> CompletionBlock)
    {
        List<Order> orders = new List<Order>();

        Router.PlayerWithUID(user.UserId).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot o = task.Result;

            foreach (DataSnapshot order in o.Children)
            {
                List<OrderItem> list = new List<OrderItem>();

                foreach (DataSnapshot item in order.Child("OrderItems").Children)
                {
                    var itemDict = (IDictionary<string, object>)item.Value;
                    int itemKey;
                    if (int.TryParse(item.Key, out itemKey))
                    {
                        Item it = Items.GetItemFromList(item.Key);
                        list.Add(new OrderItem(it, "", int.Parse(item.Value.ToString())));
                    }
                    else
                    {
                        list.Add(new OrderItem(null, item.Key, int.Parse(item.Value.ToString())));
                    }

                }
                var orderDict = (IDictionary<string, object>)order.Value;
                http.sharedInstance.Get(RemoteConfig.ServerTimeUrl).Then(time =>
                {
                    Order temp = new Order()
                    {
                        id = order.Key,
                        timer = TimeSpan.Zero,
                        orderItems = list
                    };
                    temp.timer = DateTime.Parse(orderDict["OrderTimer"].ToString()) - DateTime.Parse(time);
                    GetAdventurerByID(orderDict["adventurer"].ToString(), result =>
                    {
                        temp.adventurer = result;
                        orders.Add(temp);
                    });
                });
            }
            CompletionBlock(orders);
        });
    }
    public void GetShopTimer(Action<DateTime> CompletionBlock)
    {
        Router.GetShopTimer(user.UserId).GetValueAsync().ContinueWith(task => 
        {
            DataSnapshot time = task.Result;

            DateTime st = TimeStamp.Convert(double.Parse(time.Value.ToString()));

            CompletionBlock(st);
        });
    }
    public void GetItemById(Action<Item> CompletionBlock, string id)
    {
        Router.Item(id).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot result = task.Result;

            Item item = new Item(id, (IDictionary<string, object>)result.Value);

            CompletionBlock(item);
        });
    }
    #endregion

    #region Update Methods

    public Task UpdateEquipment(Item item, string slot)
    {
        IDictionary<string, object> temp = new Dictionary<string, object>()
        {
            { slot, item.id }
        };

        return Router.GetEquipment(user.UserId).UpdateChildrenAsync(temp);
    }

    public Task UpdateInventoryItem(Item item, int amount)
    {
        IDictionary<string, object> temp = new Dictionary<string, object>()
        {
            { item.id, amount }
        };
        if (amount > 0)
            return Router.GetInventory(user.UserId).UpdateChildrenAsync(temp);
        else
            return RemoveInventoryItem(item.id);
    }
    public Task UpdateInventoryItem(IDictionary<string, object> item)
    {
        return Router.GetInventory(user.UserId).UpdateChildrenAsync(item);
    }
    public Task UpdateShowcaseItem(ShowcaseItem sci, string slotName)
    {
        IDictionary<string, object> temp = new Dictionary<string, object>()
        {
            {"item", sci.item.id },
            {"price", sci.price },
            {"currency", sci.currency }
        };

        return Router.ShowcaseItemSlot(user.UserId, slotName).UpdateChildrenAsync(temp);
    }
    //Generic UpdatePlayer Method
    /*
    public void UpdatePlayer(UpdatePlayerParameter upp, object value)
    {
        IDictionary<string, object> temp = new Dictionary<string, object>();
        temp[upp.ToString()] = value.ToString();

        Router.PlayerWithUID(user.UserId).UpdateChildrenAsync(temp);
    }
    */
    //
    public Task UpdateShopTimer(double dt)
    {
        return Router.PlayerWithUID(user.UserId).UpdateChildrenAsync(new Dictionary<string, object>() { { "ShopTimer", dt.ToString() } });
    }
    public Task UpdateSkipTimerCount(int count)
    {
        return Router.PlayerWithUID(user.UserId).UpdateChildrenAsync(new Dictionary<string, object>() { { "ShopTimerSkipedCount", count} });
    }
    public Task UpdateRewardableVideosCount(int count)
    {
        return Router.PlayerWithUID(user.UserId).UpdateChildrenAsync(new Dictionary<string, object>() { { "RewardableVideosWatched", count } });
    }
    public Task UpdateUsername(string name)
    {
        Debug.Log("UserID in OpenUserNameInputMenu: " + user.UserId);
        List<Task> tasks = new List<Task>
        {
            user.UpdateUserProfileAsync(new UserProfile() { DisplayName = name }),
            Router.PlayerWithUID(user.UserId).UpdateChildrenAsync(new Dictionary<string, object>() { { "Username", name } })
        };
        return Task.WhenAll(tasks.ToArray());
    }
    public Task UpdateXP(int xp)
    {
        return Router.PlayerWithUID(user.UserId).UpdateChildrenAsync(new Dictionary<string, object>() { { "XP", xp } });
    }
    #endregion

    #region Remove Methods
    public Task RemoveEquipment(string slot)
    {
        return Router.GetEquipment(user.UserId).Child(slot).RemoveValueAsync();
    }
    public Task RemovePendingAdventurer()
    {
        return Router.PendingAdventurer(user.UserId).RemoveValueAsync();
    }
    public Task RemovePlayerOrder(string id)
    {
        return Router.PlayerOrderById(user.UserId, id).RemoveValueAsync();
    }
    public Task RemoveInventoryItem(string id)
    {
        return Router.InventoryItem(id, user.UserId).RemoveValueAsync();
    }
    public Task RemoveShowcaseItem(string slot)
    {
        return Router.ShowcaseItemSlot(user.UserId, slot).RemoveValueAsync();
    }
    #endregion
}
public class Router : MonoBehaviour
{
    private static DatabaseReference baseRef = /* Application.isEditor ? */ FirebaseDatabase.GetInstance(DatabaseManager.app).RootReference /* : FirebaseDatabase.DefaultInstance.RootReference */;

    public static DatabaseReference Players()
    {
        return baseRef.Child("players");
    }

    public static DatabaseReference PlayerWithUID(string uid)
    {
        return Players().Child(uid);
    }
    public static DatabaseReference AdventurerByID(string aid)
    {
        return baseRef.Child("adventurers").Child(aid);
    }
    public static DatabaseReference PendingAdventurer(string uid)
    {
        return PlayerWithUID(uid).Child("PendingAdventurer");
    }
    public static DatabaseReference GetShopTimer(string uid)
    {
        return PlayerWithUID(uid).Child("ShopTimer");
    }
    public static DatabaseReference GetInventory(string uid)
    {
        return PlayerWithUID(uid).Child("Inventory");
    }
    public static DatabaseReference GetEquipment(string uid)
    {
        return PlayerWithUID(uid).Child("Equipment");
    }
    public static DatabaseReference GetShowcaseItems(string uid)
    {
        return PlayerWithUID(uid).Child("ShowcaseItems");
    }
    public static DatabaseReference ShowcaseItemSlot(string uid, string slotName)
    {
        return GetShowcaseItems(uid).Child(slotName);
    }
    public static DatabaseReference InventoryItem(string itemid , string uid)
    {
        return GetInventory(uid).Child(itemid);
    }
    public static DatabaseReference PlayerOrders(string uid)
    {
        return PlayerWithUID(uid).Child("Orders");
    }
    public static DatabaseReference PlayerOrderById(string uid, string oid)
    {
        return PlayerOrders(uid).Child(oid);
    }
    public static DatabaseReference Item(string id)
    {
        return Items().Child(id);
    }
    public static DatabaseReference Items()
    {
        return baseRef.Child("items");
    }
}
