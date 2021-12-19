using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd :IUnityAdsInitializationListener, IUnityAdsLoadListener
{

    public void OnInitializationComplete()
    {
        GameManager.instance.GetAdManager().setBannerPosition(BannerPosition.BOTTOM_CENTER);
        GameManager.instance.GetAdManager().loadBanner();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError("Failed on banner init.");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError("Error showing banner");
    }


}