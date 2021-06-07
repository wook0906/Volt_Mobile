using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAdConfirm_Popup : UI_Popup
{
    enum Buttons
    {
        Yes_Btn,
        No_Btn
    }

    enum Labels
    {
        RemainViewCount_Label
    }

    enum Sprites
    {
        AdPopupBlock
    }

    public override void Init()
    {
        base.Init();
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));
        Bind<UISprite>(typeof(Sprites));

        GetButton((int)Buttons.No_Btn).onClick.Add(new EventDelegate(() =>
        {
            FindObjectOfType<ShopScene_UI>().InActiveBlock();
            ClosePopupUI();
        }));

        GetButton((int)Buttons.Yes_Btn).onClick.Add(new EventDelegate(OnClickConfirm));
    }

    private void OnEnable()
    {
        Invoke("DelayedOnEnable", 0.05f);
    }
    void DelayedOnEnable()
    {
        GetLabel((int)Labels.RemainViewCount_Label).text = "[FFFF00]" + Volt_PlayerData.instance.RemainAdCnt + "회 남음[-]";
    }

    private void OnClickConfirm()
    {
        //Debug.Log("SendAdWatch");
        //PacketTransmission.SendAdsWatch();
        Volt_RewardedAds.S.ShowRewardBasedAd();
        ShopScene_UI scene_UI = Managers.UI.GetSceneUI<ShopScene_UI>();
        ClosePopupUI();
        scene_UI.ActiveBlock();
        
    }

    
}
