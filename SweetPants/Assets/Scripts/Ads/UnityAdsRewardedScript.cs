using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Firebase.RemoteConfig;

public class UnityAdsRewardedScript : MonoBehaviour
{
    public Button PlayVideo;
    public MaxVideosWatchedPopUp mvwpu;

    private void Awake()
    {
        PlayVideo.onClick.RemoveAllListeners();
        PlayVideo.onClick.AddListener(ShowRewardedVideo);
    }
    public void ShowRewardedVideo()
    {
        if(Player.currentPlayer.RewardableVideosWatched < RemoteConfig.MaxRewardingVideos)
        {
            ShowOptions options = new ShowOptions
            {
                resultCallback = HandleShowResult
            };

            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            mvwpu.Open();
        }
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            Player.currentPlayer.RewardableVideosWatched++;
            DatabaseManager.sharedInstance.UpdateRewardableVideosCount(Player.currentPlayer.RewardableVideosWatched);
            Player.currentPlayer.Inventory.Where(x => x.Key.id == "2").FirstOrDefault();
            DatabaseManager.sharedInstance.UpdateInventoryItem(Items.GetItemFromList(RemoteConfig.VideoWatchedRewardItem), RemoteConfig.VideoWatchedRewardAmount);
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }
}