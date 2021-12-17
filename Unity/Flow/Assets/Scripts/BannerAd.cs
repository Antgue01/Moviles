using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAd :IUnityAdsInitializationListener, IUnityAdsLoadListener
{
    public BannerAd(float NextResetTime)
    {
        _timeToNextAdd = NextResetTime;
        _timePassed = _timeToNextAdd + 1;
    }
   

    public void OnInitializationComplete()
    {
        Debug.Log("INIT COMPLETE ");

        GameManager.instance.GetAdManager().setBannerPosition(BannerPosition.BOTTOM_CENTER);
        GameManager.instance.GetAdManager().loadBanner();

    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError("Failed on banner init.");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        //GameManager.instance.GetAdManager().loadBanner();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError("Error showing banner");
    }

    private float _timePassed;
    private float _timeToNextAdd;

}