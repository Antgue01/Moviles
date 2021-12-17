using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAd :IUnityAdsInitializationListener
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
            GameManager.instance.GetAdManager().playBanner();
        }
    }

    public void OnInitializationComplete()
    {
        GameManager.instance.GetAdManager().playBanner();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError("Failed on banner init.");
    }

    private float _timePassed;
    private float _timeToNextAdd;

}