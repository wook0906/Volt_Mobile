using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Volt.Shop;

public class ShopScene_UI : UI_Scene
{
    enum Buttons
    {
        Back_Btn,
        Option_Btn,
        Package_Btn,
        Battery_Btn,
        Dia_Btn,
        Gold_Btn,
        Skin_Btn,
        Emoticon_Btn,
        DiamondAdd_Btn,
        BatteryAdd_Btn,
        SkinMercuryTap,
        SkinReaperTap,
        SkinHoundTap,
        SkinVoltTap,
        EmoMercuryTap,
        EmoReaperTap,
        EmoHoundTap,
        EmoVoltTap,
        RestorePurchase_Btn
    }

    enum ScrollViews
    {
        Package_ScrollView,
        Dia_ScrollView,
        Battery_ScrollView,
        Gold_ScrollView,
        Skin_ScrollView,
        Emoticon_ScrollView,
        RobotSkinType_ScrollView,
        SelectRobotType_ScrollView,
    }

    enum Labels
    {
        GoldCount_Label,
        DiamondCount_Label,
        BatteryCount_Label,
        BatteryTime_Label
    }

    enum GameObjects
    {
        RobotSkin,
        Emoticon,
        Battery_Time,
        BlockPanel
    }

    public override void Init()
    {
        base.Init();
        ShopScene scene = FindObjectOfType<ShopScene>();

        Bind<UILabel>(typeof(Labels));
        Bind<UIScrollView>(typeof(ScrollViews));
        Bind<UIButton>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.Option_Btn).onClick.Add(new EventDelegate(() =>
        {
            Managers.UI.ShowPopupUIAsync<SystemOption_Popup>();
        }));

        GetButton((int)Buttons.SkinHoundTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickSkinHoundTap();
        }));

        GetButton((int)Buttons.SkinMercuryTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickSkinMercuryTap();
        }));

        GetButton((int)Buttons.SkinVoltTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickSkinVoltTap();
        }));

        GetButton((int)Buttons.SkinReaperTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickSkinReaperTap();
        }));

        GetButton((int)Buttons.EmoHoundTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoHoundTap();
        }));

        GetButton((int)Buttons.EmoMercuryTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoMercuryTap();
        }));

        GetButton((int)Buttons.EmoReaperTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoReaperTap();
        }));

        GetButton((int)Buttons.EmoVoltTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoVoltTap();
        }));

        GetButton((int)Buttons.Back_Btn).onClick.Add(new EventDelegate(OnClickBackButton));
        GetButton((int)Buttons.Dia_Btn).onClick.Add(new EventDelegate(OnClickDiamonTapButton));
        GetButton((int)Buttons.Gold_Btn).onClick.Add(new EventDelegate(OnClickGoldTapButton));
        GetButton((int)Buttons.Skin_Btn).onClick.Add(new EventDelegate(OnClickSkinTapButton));
        GetButton((int)Buttons.Emoticon_Btn).onClick.Add(new EventDelegate(OnClickEmotionTapButton));
        GetButton((int)Buttons.Battery_Btn).onClick.Add(new EventDelegate(OnClickBatteryTapButton));
        GetButton((int)Buttons.Package_Btn).onClick.Add(new EventDelegate(OnClickPackageTapButton));
        GetButton((int)Buttons.BatteryAdd_Btn).onClick.Add(new EventDelegate(OnClickBatteryTapButton));
        GetButton((int)Buttons.DiamondAdd_Btn).onClick.Add(new EventDelegate(OnClickDiamonTapButton));

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            GetButton((int)Buttons.RestorePurchase_Btn).onClick.Add(new EventDelegate(IAPManager.Instance.RestorePurchase));
        }
        else
        {
            GetButton((int)Buttons.RestorePurchase_Btn).gameObject.SetActive(false);
        }

        GetLabel((int)Labels.GoldCount_Label).text = Volt_Utils.GetGoldCountLabel(Volt_PlayerData.instance.GoldCount);
        GetLabel((int)Labels.DiamondCount_Label).text = Volt_Utils.GetDiamondCountLabel(Volt_PlayerData.instance.DiamondCount);
        GetLabel((int)Labels.BatteryCount_Label).text = $"{Volt_PlayerData.instance.BatteryCount}/5";

        //GetScrollView((int)ScrollViews.Package_ScrollView).gameObject.SetActive(false);
        GetScrollView((int)ScrollViews.Package_ScrollView).GetComponent<PackageItemScrollView>().onCompletedInit += () =>
        {
            scene.OnPackageScrollViewInitialized();
        };
        GetScrollView((int)ScrollViews.Package_ScrollView).GetComponent<PackageItemScrollView>().Init();

        GetScrollView((int)ScrollViews.Battery_ScrollView).GetComponent<BatteryItemScrollView>().onCompletedInit += () =>
        {
            scene.OnBatteryScrollViewInitialized();
        };
        GetScrollView((int)ScrollViews.Battery_ScrollView).GetComponent<BatteryItemScrollView>().Init();

        GetScrollView((int)ScrollViews.Dia_ScrollView).GetComponent<DiamondItemScrollView>().onCompletedInit += () =>
        {
            scene.OnDiamondScrollViewInitialized();
        };
        GetScrollView((int)ScrollViews.Dia_ScrollView).GetComponent<DiamondItemScrollView>().Init();

        GetScrollView((int)ScrollViews.Gold_ScrollView).GetComponent<GoldItemScrollView>().onCompletedInit += () =>
        {
            scene.OnGoldScrollViewInitialized();
        };
        GetScrollView((int)ScrollViews.Gold_ScrollView).GetComponent<GoldItemScrollView>().Init();

        GetScrollView((int)ScrollViews.Skin_ScrollView).GetComponent<RobotSkinItemScrollView>().onCompletedInit += () =>
        {
            scene.OnSkinScrollViewInitialized();
        };
        GetScrollView((int)ScrollViews.Skin_ScrollView).GetComponent<RobotSkinItemScrollView>().Init();

        GetScrollView((int)ScrollViews.Emoticon_ScrollView).GetComponent<EmoticonItemScrollView>().onCompletedInit += () =>
        {
            scene.OnEmoticonScrollViewInitialized();
        };
        GetScrollView((int)ScrollViews.Emoticon_ScrollView).GetComponent<EmoticonItemScrollView>().Init();

        scene.OnShopSceneUIInitialized();
        InActiveBlock();
    }

    private void OnClickBackButton()
    {
        Managers.Scene.LoadSceneAsync(Define.Scene.Lobby);
    }

    private void OnClickSkinVoltTap()
    {
        GetScrollView((int)ScrollViews.Skin_ScrollView).
            GetComponent<RobotSkinItemScrollView>().
            OnClickSkinTapButton(ShopRobotSelectType.Volt);
        for (int i = (int)Buttons.SkinMercuryTap; i <= (int)Buttons.SkinVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.SkinVoltTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickSkinHoundTap()
    {
        GetScrollView((int)ScrollViews.Skin_ScrollView).
            GetComponent<RobotSkinItemScrollView>().
            OnClickSkinTapButton(ShopRobotSelectType.Hound);
        for (int i = (int)Buttons.SkinMercuryTap; i <= (int)Buttons.SkinVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.SkinHoundTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickSkinReaperTap()
    {
        GetScrollView((int)ScrollViews.Skin_ScrollView).
            GetComponent<RobotSkinItemScrollView>().
            OnClickSkinTapButton(ShopRobotSelectType.Reaper);
        for (int i = (int)Buttons.SkinMercuryTap; i <= (int)Buttons.SkinVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.SkinReaperTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickSkinMercuryTap()
    {
        GetScrollView((int)ScrollViews.Skin_ScrollView).
            GetComponent<RobotSkinItemScrollView>().
            OnClickSkinTapButton(ShopRobotSelectType.Mercury);
        for (int i = (int)Buttons.SkinMercuryTap; i <= (int)Buttons.SkinVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.SkinMercuryTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickEmoVoltTap()
    {
        GetScrollView((int)ScrollViews.Emoticon_ScrollView).
            GetComponent<EmoticonItemScrollView>().
            OnClickEmoticonTapButton(ShopRobotSelectType.Volt);

        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.EmoVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoVoltTap).normalSprite = "Btn_button03_p";   
    }

    private void OnClickEmoHoundTap()
    {
        GetScrollView((int)ScrollViews.Emoticon_ScrollView).
            GetComponent<EmoticonItemScrollView>().
            OnClickEmoticonTapButton(ShopRobotSelectType.Hound);

        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.EmoVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoHoundTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickEmoReaperTap()
    {
        GetScrollView((int)ScrollViews.Emoticon_ScrollView).
            GetComponent<EmoticonItemScrollView>().
            OnClickEmoticonTapButton(ShopRobotSelectType.Reaper);
        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.EmoVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoReaperTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickEmoMercuryTap()
    {
        GetScrollView((int)ScrollViews.Emoticon_ScrollView).
            GetComponent<EmoticonItemScrollView>().
            OnClickEmoticonTapButton(ShopRobotSelectType.Mercury);
        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.EmoVoltTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoMercuryTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickDiamonTapButton()
    {
        for (int i = (int)Buttons.Package_Btn; i <= (int)Buttons.BatteryAdd_Btn; ++i)
        {
            GetButton(i).GetComponent<UISprite>().depth = 1;
            GetButton(i).enabled = true;
            GetButton(i).GetComponent<UISprite>().spriteName = "Btn_button01_n";
        }
        GetButton((int)Buttons.Dia_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.Dia_Btn).enabled = false; // 현재 보고있는 아이템 항목을 보는 버튼을 눌러도 갱신 되지 않게하기 위해
        GetButton((int)Buttons.DiamondAdd_Btn).enabled = false;
        GetButton((int)Buttons.Dia_Btn).GetComponent<UISprite>().spriteName = "Btn_button01_p";

        for (int i = (int)ScrollViews.Package_ScrollView; i <= (int)ScrollViews.Gold_ScrollView; ++i)
        {
            GetScrollView(i).gameObject.SetActive(false);
        }

        for (int i = (int)GameObjects.RobotSkin; i <= (int)GameObjects.Emoticon; ++i)
        {
            GetGameObject(i).SetActive(false);
        }

        GetScrollView((int)ScrollViews.Dia_ScrollView).gameObject.SetActive(true);
    }

    private void OnClickGoldTapButton()
    {
        for (int i = (int)Buttons.Package_Btn; i <= (int)Buttons.BatteryAdd_Btn; ++i)
        {
            GetButton(i).GetComponent<UISprite>().depth = 1;
            GetButton(i).enabled = true;
            GetButton(i).GetComponent<UISprite>().spriteName = "Btn_button01_n";
        }
        GetButton((int)Buttons.Gold_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.Gold_Btn).enabled = false;
        GetButton((int)Buttons.Gold_Btn).GetComponent<UISprite>().spriteName = "Btn_button01_p";


        for (int i = (int)ScrollViews.Package_ScrollView; i <= (int)ScrollViews.Gold_ScrollView; ++i)
        {
            GetScrollView(i).gameObject.SetActive(false);
        }

        for (int i = (int)GameObjects.RobotSkin; i <= (int)GameObjects.Emoticon; ++i)
        {
            GetGameObject(i).SetActive(false);
        }

        GetScrollView((int)ScrollViews.Gold_ScrollView).gameObject.SetActive(true);
    }

    private void OnClickEmotionTapButton()
    {
        for (int i = (int)Buttons.Package_Btn; i <= (int)Buttons.BatteryAdd_Btn; ++i)
        {
            GetButton(i).GetComponent<UISprite>().depth = 1;
            GetButton(i).enabled = true;
            GetButton(i).GetComponent<UISprite>().spriteName = "Btn_button01_n";
        }
        GetButton((int)Buttons.Emoticon_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.Emoticon_Btn).enabled = false;
        GetButton((int)Buttons.Emoticon_Btn).GetComponent<UISprite>().spriteName = "Btn_button01_p";


        for (int i = (int)ScrollViews.Package_ScrollView; i <= (int)ScrollViews.Gold_ScrollView; ++i)
        {
            GetScrollView(i).gameObject.SetActive(false);
        }

        for (int i = (int)GameObjects.RobotSkin; i <= (int)GameObjects.Emoticon; ++i)
        {
            GetGameObject(i).SetActive(false);
        }

        GetGameObject((int)GameObjects.Emoticon).SetActive(true);
        OnClickEmoMercuryTap();
    }

    private void OnClickSkinTapButton()
    {

        for (int i = (int)Buttons.Package_Btn; i <= (int)Buttons.BatteryAdd_Btn; ++i)
        {
            GetButton(i).GetComponent<UISprite>().depth = 1;
            GetButton(i).enabled = true;
            GetButton(i).GetComponent<UISprite>().spriteName = "Btn_button01_n";
        }
        GetButton((int)Buttons.Skin_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.Skin_Btn).enabled = false;
        GetButton((int)Buttons.Skin_Btn).GetComponent<UISprite>().spriteName = "Btn_button01_p";


        for (int i = (int)ScrollViews.Package_ScrollView; i <= (int)ScrollViews.Gold_ScrollView; ++i)
        {
            GetScrollView(i).gameObject.SetActive(false);
        }

        for (int i = (int)GameObjects.RobotSkin; i <= (int)GameObjects.Emoticon; ++i)
        {
            GetGameObject(i).SetActive(false);
        }

        GetGameObject((int)GameObjects.RobotSkin).SetActive(true);
        OnClickSkinMercuryTap();
    }

    private void OnClickPackageTapButton()
    {
        for (int i = (int)Buttons.Package_Btn; i <= (int)Buttons.BatteryAdd_Btn; ++i)
        {
            GetButton(i).GetComponent<UISprite>().depth = 1;
            GetButton(i).enabled = true;
            GetButton(i).GetComponent<UISprite>().spriteName = "Btn_button01_n";
        }
        GetButton((int)Buttons.Package_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.Package_Btn).enabled = false;
        GetButton((int)Buttons.Package_Btn).GetComponent<UISprite>().spriteName = "Btn_button01_p";


        for (int i = (int)ScrollViews.Package_ScrollView; i <= (int)ScrollViews.Gold_ScrollView; ++i)
        {
            GetScrollView(i).gameObject.SetActive(false);
        }

        for (int i = (int)GameObjects.RobotSkin; i <= (int)GameObjects.Emoticon; ++i)
        {
            GetGameObject(i).SetActive(false);
        }

        GetScrollView((int)ScrollViews.Package_ScrollView).gameObject.SetActive(true);
    }

    private void OnClickBatteryTapButton()
    {
        for (int i = (int)Buttons.Package_Btn; i <= (int)Buttons.BatteryAdd_Btn; ++i)
        {
            GetButton(i).GetComponent<UISprite>().depth = 1;
            GetButton(i).enabled = true;
            GetButton(i).GetComponent<UISprite>().spriteName = "Btn_button01_n";
        }
        GetButton((int)Buttons.Battery_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.Battery_Btn).enabled = false;
        GetButton((int)Buttons.BatteryAdd_Btn).enabled = false;
        GetButton((int)Buttons.Battery_Btn).GetComponent<UISprite>().spriteName = "Btn_button01_p";


        for (int i = (int)ScrollViews.Package_ScrollView; i <= (int)ScrollViews.Gold_ScrollView; ++i)
        {
            GetScrollView(i).gameObject.SetActive(false);
        }

        for (int i = (int)GameObjects.RobotSkin; i <= (int)GameObjects.Emoticon; ++i)
        {
            GetGameObject(i).SetActive(false);
        }

        GetScrollView((int)ScrollViews.Battery_ScrollView).gameObject.SetActive(true);
    }

    public void ShowItemList(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Battery:
                OnClickBatteryTapButton();
                break;
            case ObjectType.Diamond:
                OnClickDiamonTapButton();
                break;
            case ObjectType.Emoticon:
                OnClickEmotionTapButton();
                break;
            case ObjectType.Gold:
                OnClickGoldTapButton();
                break;
            case ObjectType.Skin:
                OnClickSkinTapButton();
                break;
            case ObjectType.Package:
                OnClickPackageTapButton();
                break;
            default:
                break;
        }
    }

    public void OnPurchasedRobotSkin(int id)
    {
        GetScrollView((int)ScrollViews.Skin_ScrollView).GetComponent<RobotSkinItemScrollView>().SetDisabledItemByID(id);
        InActiveBlock();
    }
    public void OnPurchasedEmoticon(int id)
    {
        GetScrollView((int)ScrollViews.Emoticon_ScrollView).GetComponent<EmoticonItemScrollView>().SetDisabledItemByID(id);
        InActiveBlock();
    }
    public void OnPurchasedPackage(int id)
    {
        GetScrollView((int)ScrollViews.Package_ScrollView).GetComponent<PackageItemScrollView>().SetDisabledItemByID(id);
        InActiveBlock();
    }
    public void ActiveBlock()
    {
        GetGameObject((int)GameObjects.BlockPanel).SetActive(true);
    }

    public void InActiveBlock()
    {
        GetGameObject((int)GameObjects.BlockPanel).SetActive(false);
    }

    public void SetDiamondCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000) / 100000;
        GetLabel((int)Labels.DiamondCount_Label).text = count.ToString() + Volt_Utils.GetUnitString(unit);
    }

    public void SetGoldCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000) / 100000;
        GetLabel((int)Labels.GoldCount_Label).text = count.ToString() + Volt_Utils.GetUnitString(unit);
    }

    public void SetBatteryCountLabel(float count)
    {
        GetLabel((int)Labels.BatteryCount_Label).text = $"{count}/{Volt_PlayerData.instance.MaxBetteryCount}";
    }

    private void ShowBatteryChargeTime()
    {
        if (!GetGameObject((int)GameObjects.Battery_Time).activeSelf)
        {
            GetGameObject((int)GameObjects.Battery_Time).SetActive(true);
        }
        else
        {
            GetGameObject((int)GameObjects.Battery_Time).SetActive(false);
        }
    }

    private void Update()
    {
        if (BatteryCharge.instance.Timer.IsStartTimer)
        {
            GetLabel((int)Labels.BatteryTime_Label).text = BatteryCharge.instance.Timer.ToString();
        }
        else
        {
            GetLabel((int)Labels.BatteryTime_Label).text = "20:00";
        }
    }

    public void Clear()
    {
        for(int i = 0; i <= (int)ScrollViews.SelectRobotType_ScrollView; ++i)
        {
            GetScrollView(i).SendMessage("Clear", SendMessageOptions.DontRequireReceiver);
        }
    }
}
