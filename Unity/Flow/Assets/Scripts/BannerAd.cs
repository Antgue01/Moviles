using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAd 
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


    private float _timePassed;
    private float _timeToNextAdd;

}