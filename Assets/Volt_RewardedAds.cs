using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Volt_RewardedAds : MonoBehaviour
{
    public static Volt_RewardedAds S;
    public RewardedAd rewardedVideoAd;
    
    string adUnitId = "ca-app-pub-3940256099942544/5224354917"; //Test ID
    //스토어 공개시 위 아이디는 주석처리, 밑 아이디는 주석 풀고 빌드할것.
    //string adUnitId = "ca-app-pub-7889081100357038/3031026591";

    private void Awake()
    {
        S = this;
    }

    public void CreateAd()
    {
        rewardedVideoAd = new RewardedAd(adUnitId);
        AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).Build();
        //스토어 공개시 위 문장은 주석처리, 밑 문장은 주석 풀고 빌드할것.
        //AdRequest request = new AdRequest.Builder().Build();
        rewardedVideoAd.LoadAd(request);
        rewardedVideoAd.OnAdClosed += HandleOnAdClosed;
        rewardedVideoAd.OnAdLoaded += HandleOnAdLoaded;
        rewardedVideoAd.OnAdOpening += HandleOnAdOpening;
        rewardedVideoAd.OnAdFailedToLoad += HandleOnAdFailedToLoaded;
        rewardedVideoAd.OnAdFailedToShow += HandleOnAdFailedToShow;
        //rewardedVideoAd.OnPaidEvent += HandleOnPaidEvent;
        rewardedVideoAd.OnUserEarnedReward += HandleOnUserEarnedReawrd;
    }

   
    public void ShowRewardBasedAd()
    {
        if (rewardedVideoAd.IsLoaded())
        {
            Debug.Log("ShowRewardBaseAd");
            rewardedVideoAd.Show();
        }
        else
        {
            print("Not loaded yet");
        }
    }
    public void HandleOnPaidEvent(object sender, EventArgs args)
    {
        print("On Paid Event");
    }
    public void HandleOnAdFailedToShow(object sender, EventArgs args)
    {
        print("On ad Fail to show");
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        print("On Ad Loaded");
    }
    public void HandleOnAdFailedToLoaded(object sender, AdErrorEventArgs args)
    {
        print("On Ad Fail to load");
    }
    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        print("On Ad Opening");
    }
    
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        print("On Ad Closed");
        rewardedVideoAd = null;
        CreateAd();
    }

    public void HandleOnUserEarnedReawrd(object sender, EventArgs args)
    {
        print("On User Earned Reward");
        PacketTransmission.SendAdsWatch();
    }
}
