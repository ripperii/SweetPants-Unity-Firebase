using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsSimpleScript : MonoBehaviour {

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}
