//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Api;
//using System;

//public class RewardAds : MonoBehaviour
//{
//    private RewardBasedVideoAd rewardBasedVideoAd;
//    // Start is called before the first frame update
//    void Start()
//    {
//        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

//        rewardBasedVideoAd.OnAdClosed += HandleOnAdClosed;
//        rewardBasedVideoAd.OnAdFailedToLoad += HandleOnAdFailedToLoaded;
//        rewardBasedVideoAd.OnAdLeavingApplication += HandleOnLeavingApp;
//        rewardBasedVideoAd.OnAdLoaded += HandleOnAdLoaded;
//        rewardBasedVideoAd.OnAdOpening += HandleOnAdOpening;
//        rewardBasedVideoAd.OnAdRewarded += HandleOnAdRewarded;
//        rewardBasedVideoAd.OnAdStarted += HandleOnAdStarted;

//        LoadRewardBaseAd();
//    }

//    private void LoadRewardBaseAd()
//    {
//#if UNITY_EDITOR
//        string adUnitId = "unused";
//#elif UNITY_ANDROID
//        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
//#elif UNITY_IPHONE
//        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
//#else
//        string adUnitId = "unexpected_platform";
//#endif
//        rewardBasedVideoAd.LoadAd(new AdRequest.Builder().Build(),adUnitId);
//        ShowRewardBasedAd();
//    }
//    private void ShowRewardBasedAd()
//    {
//        if (rewardBasedVideoAd.IsLoaded())
//        {
//            rewardBasedVideoAd.Show();
//        }
//        else
//        {
//            print("Not loaded yet");
//        }
//    }
//    public void HandleOnAdLoaded(object sender, EventArgs args)
//    {
//        print("On Ad Loaded");
//    }
//    public void HandleOnAdFailedToLoaded(object sender, AdFailedToLoadEventArgs args)
//    {
//        print("On Ad Fail to load");
//    }
//    public void HandleOnAdOpening(object sender, EventArgs args)
//    {
//        print("On Ad Opening");
//    }
//    public void HandleOnAdStarted(object sender, EventArgs args)
//    {
//        print("On Ad Started");
//    }
//    public void HandleOnAdClosed(object sender, EventArgs args)
//    {
//        print("On Ad Closed");
//    }
//    public void HandleOnAdRewarded(object sender, EventArgs args)
//    {
//        print("On Ad Rewarded");
//    }
//    public void HandleOnLeavingApp(object sender, EventArgs args)
//    {
//        print("On Leaving App");
//    }
//}
