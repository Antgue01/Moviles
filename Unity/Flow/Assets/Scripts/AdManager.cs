using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager
{
    public void init(IUnityAdsInitializationListener lis)
    {
        Advertisement.Initialize(_gameId, true, lis);
    }
    /// <summary>
    /// Loads a banner without notifying anyone if the ad can be loaded
    /// </summary>
    public void loadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {

            loadCallback = onBannerLoaded,
            errorCallback = onBannerError
        };
        Advertisement.Banner.Load(_bannerId,options);
    }
    public void hideBanner()
    {
        Advertisement.Banner.Hide();
    }
    public void showBanner()
    {
        Debug.Log("SHOW BANNER ESTA AQUI");
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
        Advertisement.Banner.Show(_bannerId,options);
    }

    private void OnBannerShown()
    {
        Debug.Log("ON BANNER SHOW");
    }

    private void OnBannerHidden()
    {
        Debug.Log("ON BANNER HIDDEN");
    }

    private void OnBannerClicked()
    {
        Debug.Log("ON BANNER CLICKED");

    }

    void onBannerError(string err)
    {
        Debug.Log("BANNER ERROR");

        Debug.Log(err);
    }
    void onBannerLoaded()
    {
        Debug.Log("ON BANNER LOADED");

        showBanner();
    }
    /// <summary>
    /// Plays a rewarded video without notifying anyone     
    /// </summary>
    public void playRewardedVideo()
    {
        Advertisement.Load(_rewardId);
        Advertisement.Show(_rewardId);
    }
    /// <summary>
    /// Plays a rewarded video notifying on ad load
    /// </summary>
    /// <param name="loadListener">The listener to notify</param>
    public void playRewardedVideo(IUnityAdsLoadListener loadListener)
    {
        Advertisement.Load(_rewardId, loadListener);
        Advertisement.Show(_rewardId);
    }
    /// <summary>
    /// Plays a rewarded ad notifying on show (start, completed, click and failure)
    /// </summary>
    /// <param name="showListener">The listener to notify</param>
    public void playRewardedVideo(IUnityAdsShowListener showListener)
    {
        Advertisement.Load(_rewardId);
        Advertisement.Show(_rewardId, showListener);
    }
    /// <summary>
    /// Plays a banner notifying on load and on show
    /// </summary>
    /// <param name="loadListener">The listener to notify on load</param>
    /// <param name="showListener">The listener to notify on show</param>
    public void playRewardedVideo(IUnityAdsLoadListener loadListener, IUnityAdsShowListener showListener)
    {
        Advertisement.Load(_rewardId, loadListener);
        Advertisement.Show(_rewardId, showListener);
    }
    public void setBannerPosition(BannerPosition pos)
    {
        Advertisement.Banner.SetPosition(pos);
    }
    public bool isBanner(string adUnitId) { return adUnitId.Equals(_bannerId); }
    public bool isRewardedVideo(string adUnitId) { return adUnitId.Equals(_rewardId); }
    private const string _gameId = "4506825";
    private const string _rewardId = "Rewarded_Android";
    private const string _bannerId = "Banner_Android";
}
