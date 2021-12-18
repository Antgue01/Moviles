using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] LevelManager _levelManager;
    public void requestAd() {
        GameManager.instance.GetAdManager().hideBanner();
        GameManager.instance.GetAdManager().playRewardedVideo(this,this); 
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (GameManager.instance.GetAdManager().isRewardedVideo(adUnitId))
            Debug.Log("Rewarded Video Ad Loaded");
    }


    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (GameManager.instance.GetAdManager().isRewardedVideo(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Grant a reward.
            _levelManager.watchVideo();
            GameManager.instance.GetAdManager().showBanner();

        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }
}