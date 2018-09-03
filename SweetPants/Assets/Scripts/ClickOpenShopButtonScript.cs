using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;
using Firebase.Auth;
using System.Collections.Generic;
using Newtonsoft.Json;
using Firebase.Database;
using UnityEngine.Networking;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class ClickOpenShopButtonScript : MonoBehaviour {

    public Button openShopButton;
    
    public static bool adWatched = false;
    public static bool keyboardActive = false;
    public int advType = 0;

    public string text;
    public string Token;
    public Text timer;
    public static TimeSpan timeLeft;
    public static bool timerActive = false;
    public Button SkipShopTimer;
    public Image SkipShopTimerImage;
    public SkipShopTimerPopUpScript sstps;
    public MaxVideosWatchedPopUp mvwpu;

    public Animator ShopSignAnimator;

    public AdventurerMenu advMenu;
    public Adventurer adventurer = null;
    // Use this for initialization

    private void Start()
    {
        Router.GetShopTimer(DatabaseManager.user.UserId).ValueChanged += ShopTimerSetEvent;
    }
    void Awake () 
    {
        openShopButton.onClick.RemoveAllListeners();
        openShopButton.onClick.AddListener(OnShopOpen);
    }
	public void ShopTimerSet()
    {
        if(null != DatabaseManager.user)
        {
            http.sharedInstance.Get(RemoteConfig.ServerTimeUrl)
                .Then(time =>
                {
                    DateTime now = TimeStamp.Convert(double.Parse(time));

                    DatabaseManager.sharedInstance.GetShopTimer(st =>
                    {
                        if (st > now)
                        {
                            timer.gameObject.SetActive(true);
                            openShopButton.interactable = false;
                            SkipShopTimer.interactable = true;
                            SkipShopTimerImage.raycastTarget = true;
                            timerActive = true;

                            timeLeft = st - now;

                            if (ShopSignAnimator.GetCurrentAnimatorStateInfo(0).IsName("SignOpenAnimation"))
                                ShopSignAnimator.SetTrigger("ShopOpen");
                        }
                        else
                        {
                            timeLeft = st - now;

                            if (ShopSignAnimator.GetCurrentAnimatorStateInfo(0).IsName("SignClosedAnimation"))
                                ShopSignAnimator.SetTrigger("ShopClose");
                        }
                    });
                });
        }
    }
    public void ShopTimerSetEvent(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("ShopTimerSet - Value Changed Event");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        ShopTimerSet();
    }
    // Update is called once per frame
    void Update () 
    {
         if (timerActive)
            {
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(Time.deltaTime));

                text = String.Format("{0:00}:{1:00}:{2:00}", timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
                
                if (timeLeft.TotalSeconds < 0)
                {
                    timer.gameObject.SetActive(false);
                    openShopButton.interactable = true;
                    SkipShopTimer.interactable = false;
                    SkipShopTimerImage.raycastTarget = false;
                    timerActive = false;
                    ShopSignAnimator.SetTrigger("ShopClose");
                }
                timer.text = text;
            }
            
        //openShopButton.gameObject.SetActive(false);
	}
    
    void OnShopOpen()
    {
        if (null != DatabaseManager.user)
        {
            string url = RemoteConfig.AdventurerUrl;
            string data = String.Format("{0}\"seconds\":{1},\"level\":{2},\"type\":{3}{4}","{", RemoteConfig.ShopTimer, Levels.FindPlayerLevel(Player.currentPlayer.XP).Key, advType, "}");

            ShopSignAnimator.SetTrigger("ShopOpen");
            //http.sharedInstance.Get(url)
            http.sharedInstance.Post(url, data)
                .Then(json =>
                {
                    Debug.Log("Adventurer Json: " + json);
                    adventurer = new Adventurer(json);

                    Debug.Log("Reached Before Opening Adventurer Menu!");
                    /*
                    timer.gameObject.SetActive(true);
                    openShopButton.interactable = false;
                    SkipShopTimer.interactable = true;
                    SkipShopTimerImage.raycastTarget = true;
                    timerActive = true;

                    timeLeft = TimeSpan.FromSeconds(RemoteConfig.ShopTimer);
                    */
                    advMenu.OpenAdventurerMenu(adventurer);

                    Debug.Log("Trigger after adventurer!");
                });
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("ShopTimerSet - OnApplicationFocus Method");
        
        if (focus)
            if (!adWatched && !keyboardActive)
            {
                ShopTimerSet();
            }
            else
            {
                Debug.Log("Ad Watched do not Set Shop Timer!");
                adWatched = false;
            }
        else
        {
            keyboardActive = TouchScreenKeyboard.visible;
        }
    }
}
