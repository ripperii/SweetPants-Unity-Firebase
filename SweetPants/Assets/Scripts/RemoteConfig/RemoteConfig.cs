using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class RemoteConfig : MonoBehaviour {

    public static RemoteConfig rc;

    // Unity RemoteSettings - true / Firebase RemoteSettings - false
    public enum ConfigType { Unity, FireBase };

    [Tooltip("Unity RemoteSettings - true / Firebase RemoteSettings - false")]
    public ConfigType remoteConfigUsed = ConfigType.Unity;

    // Default values - Set via the Unity Editor
    public string adventurerUrl;
    public string serverTimeUrl;
    public int maxSkipShopTimerVideos;
    public int maxRewardingVideos;
    public int shopTimer;
    [Multiline(5)]
    public string levelsJson;
    public int startingDiamondsAmount;
    public int startingGoldAmount;
    public float auctionPricePercentilePerDay;
    public string videoWatchedRewardItem;
    public int videoWatchedRewardAmount;
    public float xpPerGold;

    // Static values
    public static string AdventurerUrl;
    public static string ServerTimeUrl;
    public static int MaxSkipShopTimerVideos;
    public static int MaxRewardingVideos;
    public static int ShopTimer;
    public static string LevelsJson;
    public static int StartingDiamondsAmount;
    public static int StartingGoldAmount;
    public static float AuctionPricePercentilePerDay;
    public static string VideoWatchedRewardItem;
    public static int VideoWatchedRewardAmount;
    public static float XPPerGold;


    // Use this for initialization
    private void Awake()
    {
        rc = this;

        AdventurerUrl = adventurerUrl;
        ServerTimeUrl = serverTimeUrl;
        MaxSkipShopTimerVideos = maxSkipShopTimerVideos;
        MaxRewardingVideos = maxRewardingVideos;
        ShopTimer = shopTimer;
        LevelsJson = levelsJson;
        StartingDiamondsAmount = startingDiamondsAmount;
        StartingGoldAmount = startingGoldAmount;
        AuctionPricePercentilePerDay = auctionPricePercentilePerDay;
        VideoWatchedRewardItem = videoWatchedRewardItem;
        VideoWatchedRewardAmount = videoWatchedRewardAmount;
        XPPerGold = xpPerGold;

        switch (remoteConfigUsed)
        {
            case ConfigType.Unity:
                {
                    RemoteSettings.Updated += new RemoteSettings.UpdatedEventHandler(RemoteConfigUpdatedEvent);
                    RemoteSettings.ForceUpdate();

                    Debug.Log("Unity Remote Config has been used!");
                    RemoteConfigUpdatedEvent();
                    break;
                }
            case ConfigType.FireBase:
                {
                    Dictionary<string, object> defaults = new Dictionary<string, object>
                    {
                        { "Levels_1 - 10", LevelsJson },
                        { "AdventurerUrl", adventurerUrl },
                        { "ServerTimeUrl", serverTimeUrl },
                        { "MaxSkipShopTimerVideos", maxSkipShopTimerVideos },
                        { "MaxRewardingVideos", maxRewardingVideos },
                        { "ShopTimer", shopTimer },
                        { "Starting_Diamonds_Amount", startingDiamondsAmount },
                        { "Starting_Gold_Amount", startingGoldAmount },
                        { "AuctionPricePercentilePerDay", auctionPricePercentilePerDay },
                        { "VideoWatchedRewardItem", videoWatchedRewardItem },
                        { "VideoWatchedRewardAmount", videoWatchedRewardAmount },
                        { "XPPerGold", xpPerGold }
                    };

                    FirebaseRemoteConfig.SetDefaults(defaults);
                    FetchDataAsync();
                    Debug.Log("Firebase Remote Config has been used!");

                    break;
                }
        }
    }

    void Start () {


	}
	
	// Update is called once per frame
	void Update () {
        /*
        if(remoteConfigUsed == ConfigType.FireBase && FirebaseRemoteConfig.Info.FetchTime < DateTime.Now)
        {
            Debug.Log(FirebaseRemoteConfig.Info.FetchTime + " < " + DateTime.Now);
            //FetchDataAsync();
        }
        */
    }
    public void RemoteConfigUpdatedEvent()
    {
        Debug.Log("Remote Config Updated!");
        levelsJson = LevelsJson = RemoteSettings.GetString("Levels_1-10", levelsJson);
        Levels.GetLevels();

        adventurerUrl = AdventurerUrl = RemoteSettings.GetString("AdventurerUrl", adventurerUrl);
        serverTimeUrl = ServerTimeUrl = RemoteSettings.GetString("ServerTimeUrl", serverTimeUrl);
        maxSkipShopTimerVideos = MaxSkipShopTimerVideos = RemoteSettings.GetInt("MaxSkipShopTimerVideos", maxSkipShopTimerVideos);
        maxRewardingVideos = MaxRewardingVideos = RemoteSettings.GetInt("MaxRewardingVideos", maxRewardingVideos);
        shopTimer = ShopTimer = RemoteSettings.GetInt("ShopTimer", shopTimer);
        startingDiamondsAmount = StartingDiamondsAmount = RemoteSettings.GetInt("Starting_Diamonds_Amount", startingDiamondsAmount);
        startingGoldAmount = StartingGoldAmount = RemoteSettings.GetInt("Starting_Gold_Amount", startingGoldAmount);
        auctionPricePercentilePerDay = AuctionPricePercentilePerDay = RemoteSettings.GetFloat("AuctionPricePercentilePerDay", auctionPricePercentilePerDay);
        videoWatchedRewardItem = VideoWatchedRewardItem = RemoteSettings.GetString("VideoWatchedRewardItem", videoWatchedRewardItem);
        videoWatchedRewardAmount = VideoWatchedRewardAmount = RemoteSettings.GetInt("VideoWatchedRewardAmount", videoWatchedRewardAmount);
        xpPerGold = XPPerGold = RemoteSettings.GetFloat("XPPerGold", xpPerGold);
        
        if (RemoteSettings.WasLastUpdatedFromServer())
        {

        }
    }

    public Task FetchDataAsync()
    {
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.
        Task fetchTask = FirebaseRemoteConfig.FetchAsync(TimeSpan.FromMinutes(60));
        
        return fetchTask.ContinueWith(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
            return;
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
            Debug.Log(fetchTask.Exception);
            return;
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = FirebaseRemoteConfig.Info;
        
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                //SetFetchedValues();
                Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
    void SetFetchedValues()
    {
        Debug.Log("Values have been set!");
        levelsJson = LevelsJson = FirebaseRemoteConfig.GetValue("Levels_1-10").StringValue;
        Levels.GetLevels();

        adventurerUrl = AdventurerUrl = FirebaseRemoteConfig.GetValue("AdventurerUrl").StringValue;
        serverTimeUrl = ServerTimeUrl = FirebaseRemoteConfig.GetValue("ServerTimeUrl").StringValue;
        maxSkipShopTimerVideos = MaxSkipShopTimerVideos = Convert.ToInt32(FirebaseRemoteConfig.GetValue("MaxSkipShopTimerVideos").LongValue);
        maxRewardingVideos = MaxRewardingVideos = Convert.ToInt32(FirebaseRemoteConfig.GetValue("MaxRewardingVideos").LongValue);
        shopTimer = ShopTimer = Convert.ToInt32(FirebaseRemoteConfig.GetValue("ShopTimer").LongValue);
        startingDiamondsAmount = StartingDiamondsAmount = Convert.ToInt32(FirebaseRemoteConfig.GetValue("Starting_Diamonds_Amount"));
        startingGoldAmount = StartingGoldAmount = Convert.ToInt32(FirebaseRemoteConfig.GetValue("Starting_Gold_Amount"));
        auctionPricePercentilePerDay = AuctionPricePercentilePerDay = float.Parse(FirebaseRemoteConfig.GetValue("AuctionPricePercentilePerDay").DoubleValue.ToString());
        videoWatchedRewardItem = VideoWatchedRewardItem = FirebaseRemoteConfig.GetValue("VideoWatchedRewardItem").StringValue;
        videoWatchedRewardAmount = VideoWatchedRewardAmount = Convert.ToInt32(FirebaseRemoteConfig.GetValue("VideoWatchedRewardAmount").LongValue);
        xpPerGold = XPPerGold = float.Parse(FirebaseRemoteConfig.GetValue("XPPerGold").DoubleValue.ToString());
    }
}