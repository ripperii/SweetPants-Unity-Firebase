using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;

public class UnityAdsSkipShopTimerScript : MonoBehaviour
{
    public ClickOpenShopButtonScript cosb;
    public FadeInOutScript fade;
    public CanvasGroup canvasGroup;
    public float duration = 0.5f;
    private double current;

    public string url = "https://us-central1-sweetpants-f86be.cloudfunctions.net/date?format=x";

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            ClickOpenShopButtonScript.adWatched = true;
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The Skip Shop Timer AD was successfully shown.");
                Player.currentPlayer.ShopTimerSkipedCount++;
                DatabaseManager.sharedInstance.UpdateSkipTimerCount(Player.currentPlayer.ShopTimerSkipedCount);
                UpdateShopTimer(RemoteConfig.ShopTimer);
                //StartCoroutine(UpdateShopTimer());
                
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
    void UpdateShopTimer(int seconds)
    {
        DatabaseManager.sharedInstance.UpdateShopTimer(double.Parse(Player.currentPlayer.ShopTimer) - seconds*1000).ContinueWith(task =>
        {
            Debug.Log("ShopTimer Skipped by Watching Video AD!");
        });
    }
}
