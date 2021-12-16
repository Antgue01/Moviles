using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAd : IUnityAdsLoadListener
{
    public BannerAd(float NextResetTime)
    {
        _timeToNextAdd = NextResetTime;
        _timePassed = _timeToNextAdd + 1;
    }
    public void Update()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed >= _timeToNextAdd)
        {
            _timePassed = 0;
            GameManager.instance.GetAdManager().playBanner(this);
        }
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (GameManager.instance.GetAdManager().isBanner(placementId))
            Debug.Log("Banner shown");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (GameManager.instance.GetAdManager().isBanner(placementId))
            Debug.LogError("Banner failed to load: " + error.ToString() + ": " + message);
    }

    private float _timePassed;
    private float _timeToNextAdd;

}