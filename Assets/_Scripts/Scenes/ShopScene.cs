using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Volt.Shop;

public class ShopScene : BaseScene
{
    enum Loads
    {
        None = 0,
        ShopData = 1 << 0,
        SceneUI = 1 << 1,
        PackageScrollView = 1 >> 2,
        BatteryScrollView = 1 << 3,
        DiamondScrollView = 1 << 4,
        GoldScrollView = 1 << 5,
        RobotSkinScrollView = 1 << 6,
        EmoticonScrollView = 1 << 7,
        All = ShopData | SceneUI | BatteryScrollView |
            DiamondScrollView | GoldScrollView | RobotSkinScrollView |
            EmoticonScrollView | PackageScrollView
    }

    private Loads load;
    public override float Progress
    {
        get
        {
            Array loadList = typeof(Loads).GetEnumValues();
            int max = typeof(Loads).GetEnumValues().Length - 2;
            int count = 0;
            for(int i = 1; i <= max; ++i)
            {
                if ((load & (Loads)loadList.GetValue(i)) != 0)
                    count++;
            }

            return (float)count / max;
        }
    }

    public override bool IsDone
    {
        get { return load == Loads.All; }
    }

    private IEnumerator Start()
    {
        SceneType = Define.Scene.Shop;

        Managers.UI.ShowPopupUIAsync<Fade_Popup>();
        Fade_Popup fadePopup = null;
        yield return new WaitUntil(() =>
        {
            fadePopup = FindObjectOfType<Fade_Popup>();
            return fadePopup != null;
        });
        fadePopup.FadeIn(1f, float.MaxValue);

        Managers.UI.ShowPopupUIAsync<Loading_Popup>(null, false);
        yield return new WaitUntil(() =>
        {
            Loading_Popup loadingPopup = FindObjectOfType<Loading_Popup>();
            return loadingPopup != null && loadingPopup.IsInit;
        });
        fadePopup.IsStartRightAway = true;

        //ShopDataManager.instance.Init();
        load |= Loads.ShopData;

        Managers.UI.ShowSceneUIAsync<ShopScene_UI>();

        yield return new WaitUntil(() => { return IsDone; });

        if(!PlayerPrefs.HasKey("Volt_ShopEnterKey"))
            PlayerPrefs.SetString("Volt_ShopEnterKey", "Package");

        ShopScene_UI sceneUI = Managers.UI.GetSceneUI<ShopScene_UI>();
        sceneUI.ShowItemList((ObjectType)Enum.Parse(typeof(ObjectType), PlayerPrefs.GetString("Volt_ShopEnterKey")));

        Managers.UI.CloseAllPopupUI();
    }

    protected override void Init()
    {

    }

    public override void Clear()
    {
        ShopScene_UI sceneUI = Managers.UI.GetSceneUI<ShopScene_UI>();
        sceneUI.Clear();
        Managers.Clear();
        Camera.main.enabled = false;
    }
    
    public void OnPackageScrollViewInitialized()
    {
        load |= Loads.PackageScrollView;
    }

    public void OnBatteryScrollViewInitialized()
    {
        load |= Loads.BatteryScrollView;
    }

    public void OnDiamondScrollViewInitialized()
    {
        load |= Loads.DiamondScrollView;
    }

    public void OnGoldScrollViewInitialized()
    {
        load |= Loads.GoldScrollView;
    }

    public void OnSkinScrollViewInitialized()
    {
        load |= Loads.RobotSkinScrollView;
    }

    public void OnEmoticonScrollViewInitialized()
    {
        load |= Loads.EmoticonScrollView;
    }

    public void OnShopSceneUIInitialized()
    {
        load |= Loads.SceneUI;
    }
}
