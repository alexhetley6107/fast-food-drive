using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    private string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        //"ca-app-pub-6213054885767367/9071434413";
    private InterstitialAd interstitial;
    private int nowLoses;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DestroyAndStartNew(true);
    }
    private void Update()
    {
        if(interstitial.IsLoaded() && GameControl.countLoses % 3 == 0 && GameControl.countLoses != 0
            && GameControl.countLoses != nowLoses)
        {
            nowLoses = GameControl.countLoses;
            interstitial.Show();
        }            
    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DestroyAndStartNew();
    }     
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        DestroyAndStartNew();
    }
    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        DestroyAndStartNew();
    }

   void  DestroyAndStartNew(bool isFirst = false)
    {
        if(!isFirst)
            interstitial.Destroy();
    
        interstitial = new InterstitialAd(adUnitId);
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
}
