using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager
{
    public void init()
    {
        Advertisement.Initialize(_gameId, false);
    }
    /// <summary>
    /// Plays a banner without notifying anyone if the ad can be loaded
    /// </summary>
    public void playBanner()
    {
        Advertisement.Load(_bannerId);
        if (Advertisement.Banner.isLoaded)
            Advertisement.Show(_bannerId);
    }
    /// <summary>
    /// Plays a banner notifying on ad load if the ad can be loaded
    /// </summary>
    /// <param name="loadListener">The listener to notify</param>
    public void playBanner(IUnityAdsLoadListener loadListener)
    {
        Advertisement.Load(_bannerId, loadListener);
        if (Advertisement.Banner.isLoaded)
            Advertisement.Show(_bannerId);
    }
    /// <summary>
    /// Plays a banner notifying on show (start, completed, click and failure) if the ad can be loaded
    /// </summary>
    /// <param name="showListener">The listener to notify</param>
    public void playBanner(IUnityAdsShowListener showListener)
    {
        Advertisement.Load(_bannerId);
        if (Advertisement.Banner.isLoaded)
            Advertisement.Show(_bannerId, showListener);
    }
    /// <summary>
    /// Plays a banner notifying on load and on show if the ad can be loaded
    /// </summary>
    /// <param name="loadListener">The listener to notify on load</param>
    /// <param name="showListener">The listener to notify on show</param>
    public void playBanner(IUnityAdsLoadListener loadListener, IUnityAdsShowListener showListener)
    {
        Advertisement.Load(_bannerId, loadListener);
        if (Advertisement.Banner.isLoaded)
            Advertisement.Show(_bannerId, showListener);
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
