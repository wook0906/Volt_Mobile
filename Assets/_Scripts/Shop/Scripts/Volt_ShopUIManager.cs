using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_ShopUIManager : MonoBehaviour
{
    public static Volt_ShopUIManager S;

    public GameObject packageShopGO;
    public GameObject diamondShopGO;
    public GameObject batteryShopGO;
    public GameObject goldShopGO;
    public GameObject robotSkinShopGO;
    public GameObject emoticonShopGO;

    public GameObject optionGO;
    public Volt.Shop.RobotSkinItemScrollView skinItemScrollView;
    public Volt.Shop.EmoticonItemScrollView emoticonItemScrollView;
    public Volt_ShopConfirmPopupPanel purchaseConfirmPopupPanel;
    public Volt_PurchaseFeedbackPanel purchasedConfirmPopupPanel;
    public GameObject notEnoughAssetNoticePanel;

    public UIButton mercurySkinTapButton;
    public UIButton voltSkinTapButton;
    public UIButton reaperSkinTapButton;
    public UIButton houndSkinTapButton;

    public UIButton mercuryEmoticonTapButton;
    public UIButton voltEmoticonTapButton;
    public UIButton reaperEmoticonTapButton;
    public UIButton houndEmoticonTapButton;


    private const string normalSkinTapButtonName = "Btn_button03_n";
    private const string pressedSkinTapButtonName = "Btn_button03_p";

    public UISprite packageBtn;
    public UISprite diamondBtn;
    public UISprite batteryBtn;
    public UISprite goldBtn;
    public UISprite skinBtn;
    public UISprite emoticonBtn;


    //플레이어의 데이터
    public UILabel batteryLabel;
    public UILabel diamondLabel;
    public UILabel goldLabel;

    
    private Volt.Shop.ShopRobotSelectType currSelectedSkinType = Volt.Shop.ShopRobotSelectType.Mercury;
    public Volt.Shop.ShopRobotSelectType curTapRobotType;

    bool isControlled = false;

    void Awake()
    {
        S = this;
    }
    private void Start()
    {
        batteryLabel.text = Volt_PlayerData.instance.BatteryCount.ToString() + "/5";
        SetDiamondCountLabel(Volt_PlayerData.instance.DiamondCount);
        SetGoldCountLabel(Volt_PlayerData.instance.GoldCount);
    }
    public void ShopDataLoadDoneCallback()
    {
        if (!PlayerPrefs.HasKey("Volt_ShopEnterKey"))
        {
            OnClickPackageShopTap();
            //OnClickBatteryShopTap();
            return;
        }
        switch (PlayerPrefs.GetString("Volt_ShopEnterKey"))
        {
            case "Default":
                break;
            case "Battery":
                OnClickBatteryShopTap();
                break;
            case "Jewelry":
                OnClickDiamondShopTap();
                break;
            case "Skin":
                OnClickRobotSkinShopTap();
                break;
            case "Package":
                OnClickPackageShopTap();
                break;
            default:
                break;
        }
    }
    public void SetGoldCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000f) / 100000f;
        goldLabel.text = count.ToString() + Volt_Utils.GetUnitString(unit);
    }
    public void SetDiamondCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000f) / 100000f;
        diamondLabel.text = count.ToString() + Volt_Utils.GetUnitString(unit);
    }
    public void OnClickPackageShopTap()
    {
        batteryShopGO.SetActive(false);
        diamondShopGO.SetActive(false);
        goldShopGO.SetActive(false);
        robotSkinShopGO.SetActive(false);
        emoticonShopGO.SetActive(false);

        if (packageShopGO.activeSelf)
            return;

        packageShopGO.SetActive(true);

        packageBtn.depth = 2;
        batteryBtn.depth = 0;
        diamondBtn.depth = 0;
        goldBtn.depth = 0;
        skinBtn.depth = 0;
        emoticonBtn.depth = 0;
    }
    public void OnClickBatteryShopTap()
    {
        packageShopGO.SetActive(false);
        diamondShopGO.SetActive(false);
        goldShopGO.SetActive(false);
        robotSkinShopGO.SetActive(false);
        emoticonShopGO.SetActive(false);

        if (batteryShopGO.activeSelf)
            return;

        batteryShopGO.SetActive(true);

        packageBtn.depth = 0;
        batteryBtn.depth = 2;
        diamondBtn.depth = 0;
        goldBtn.depth = 0;
        skinBtn.depth = 0;
        emoticonBtn.depth = 0;
    }

    public void OnClickDiamondShopTap()
    {
        packageShopGO.SetActive(false);
        batteryShopGO.SetActive(false);
        goldShopGO.SetActive(false);
        robotSkinShopGO.SetActive(false);
        emoticonShopGO.SetActive(false);

        if (diamondShopGO.activeSelf)
            return;

        diamondShopGO.SetActive(true);

        packageBtn.depth = 0;
        batteryBtn.depth = 0;
        diamondBtn.depth = 2;
        goldBtn.depth = 0;
        skinBtn.depth = 0;
        emoticonBtn.depth = 0;
    }

    public void OnClickGoldShopTap()
    {
        packageShopGO.SetActive(false);
        diamondShopGO.SetActive(false);
        batteryShopGO.SetActive(false);
        robotSkinShopGO.SetActive(false);
        emoticonShopGO.SetActive(false);

        if (goldShopGO.activeSelf)
            return;

        goldShopGO.SetActive(true);

        packageBtn.depth = 0;
        batteryBtn.depth = 0;
        diamondBtn.depth = 0;
        goldBtn.depth = 2;
        skinBtn.depth = 0;
        emoticonBtn.depth = 0;
    }

    public void OnClickRobotSkinShopTap()
    {
        packageShopGO.SetActive(false);
        batteryShopGO.SetActive(false);
        goldShopGO.SetActive(false);
        diamondShopGO.SetActive(false);
        emoticonShopGO.SetActive(false);

        if (robotSkinShopGO.activeSelf)
            return;

        robotSkinShopGO.SetActive(true);

        packageBtn.depth = 0;
        batteryBtn.depth = 0;
        diamondBtn.depth = 0;
        goldBtn.depth = 0;
        skinBtn.depth = 2;
        emoticonBtn.depth = 0;

        switch (currSelectedSkinType)
        {
            case Volt.Shop.ShopRobotSelectType.Mercury:
                OnClickMercurySkinTapButton();
                break;
            case Volt.Shop.ShopRobotSelectType.Hound:
                OnClickHoundSkinTapButton();
                break;
            case Volt.Shop.ShopRobotSelectType.Reaper:
                OnClickReaperSkinTapButton();
                break;
            case Volt.Shop.ShopRobotSelectType.Volt:
                OnClickVoltSkinTapButton();
                break;
            default:
                break;
        }
    }

    public void OnClickEmoticonShopTap()
    {
        packageShopGO.SetActive(false);
        batteryShopGO.SetActive(false);
        goldShopGO.SetActive(false);
        diamondShopGO.SetActive(false);
        robotSkinShopGO.SetActive(false);

        if (emoticonShopGO.activeSelf)
            return;

        emoticonShopGO.SetActive(true);

        packageBtn.depth = 0;
        batteryBtn.depth = 0;
        diamondBtn.depth = 0;
        goldBtn.depth = 0;
        skinBtn.depth = 0;
        emoticonBtn.depth = 2;

        switch (currSelectedSkinType)
        {
            case Volt.Shop.ShopRobotSelectType.Mercury:
                OnClickMercuryEmoticonTapButton();
                break;
            case Volt.Shop.ShopRobotSelectType.Hound:
                OnClickHoundEmoticonTapButton();
                break;
            case Volt.Shop.ShopRobotSelectType.Reaper:
                OnClickReaperEmoticonTapButton();
                break;
            case Volt.Shop.ShopRobotSelectType.Volt:
                OnClickVoltEmoticonTapButton();
                break;
            default:
                break;
        }
    }

    public void OnClickVoltSkinTapButton()
    {
        mercurySkinTapButton.normalSprite = normalSkinTapButtonName;
        reaperSkinTapButton.normalSprite = normalSkinTapButtonName;
        houndSkinTapButton.normalSprite = normalSkinTapButtonName;

        voltSkinTapButton.normalSprite = pressedSkinTapButtonName;
        skinItemScrollView.OnClickSkinTapButton(Volt.Shop.ShopRobotSelectType.Volt);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Volt;
    }

    public void OnClickMercurySkinTapButton()
    {
        voltSkinTapButton.normalSprite = normalSkinTapButtonName;
        reaperSkinTapButton.normalSprite = normalSkinTapButtonName;
        houndSkinTapButton.normalSprite = normalSkinTapButtonName;

        mercurySkinTapButton.normalSprite = pressedSkinTapButtonName;
        skinItemScrollView.OnClickSkinTapButton(Volt.Shop.ShopRobotSelectType.Mercury);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Mercury;
    }

    public void OnClickReaperSkinTapButton()
    {
        mercurySkinTapButton.normalSprite = normalSkinTapButtonName;
        voltSkinTapButton.normalSprite = normalSkinTapButtonName;
        houndSkinTapButton.normalSprite = normalSkinTapButtonName;

        reaperSkinTapButton.normalSprite = pressedSkinTapButtonName;
        skinItemScrollView.OnClickSkinTapButton(Volt.Shop.ShopRobotSelectType.Reaper);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Reaper;
    }

    public void OnClickHoundSkinTapButton()
    {
        mercurySkinTapButton.normalSprite = normalSkinTapButtonName;
        reaperSkinTapButton.normalSprite = normalSkinTapButtonName;
        voltSkinTapButton.normalSprite = normalSkinTapButtonName;

        houndSkinTapButton.normalSprite = pressedSkinTapButtonName;
        skinItemScrollView.OnClickSkinTapButton(Volt.Shop.ShopRobotSelectType.Hound);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Hound;
    }

    public void OnClickVoltEmoticonTapButton()
    {
        mercuryEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        reaperEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        houndEmoticonTapButton.normalSprite = normalSkinTapButtonName;

        voltEmoticonTapButton.normalSprite = pressedSkinTapButtonName;
        emoticonItemScrollView.OnClickEmoticonTapButton(Volt.Shop.ShopRobotSelectType.Volt);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Volt;
    }

    public void OnClickMercuryEmoticonTapButton()
    {
        voltEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        reaperEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        houndEmoticonTapButton.normalSprite = normalSkinTapButtonName;

        mercuryEmoticonTapButton.normalSprite = pressedSkinTapButtonName;
        emoticonItemScrollView.OnClickEmoticonTapButton(Volt.Shop.ShopRobotSelectType.Mercury);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Mercury;
    }

    public void OnClickReaperEmoticonTapButton()
    {
        mercuryEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        voltEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        houndEmoticonTapButton.normalSprite = normalSkinTapButtonName;

        reaperEmoticonTapButton.normalSprite = pressedSkinTapButtonName;
        emoticonItemScrollView.OnClickEmoticonTapButton(Volt.Shop.ShopRobotSelectType.Reaper);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Reaper;
    }

    public void OnClickHoundEmoticonTapButton()
    {
        mercuryEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        reaperEmoticonTapButton.normalSprite = normalSkinTapButtonName;
        voltEmoticonTapButton.normalSprite = normalSkinTapButtonName;

        houndEmoticonTapButton.normalSprite = pressedSkinTapButtonName;
        emoticonItemScrollView.OnClickEmoticonTapButton(Volt.Shop.ShopRobotSelectType.Hound);
        curTapRobotType = Volt.Shop.ShopRobotSelectType.Hound;
    }

    public void OnPressdownShowWindowButton(GameObject self)
    {
        self.GetComponent<UIRect>().alpha = 1f;
        Volt_UILayerManager.instance.Enqueue(self);
    }
    public void OnPressdownHideButton(GameObject self)
    {
        self.GetComponent<UIRect>().alpha = 0f;
        Volt_UILayerManager.instance.RemoveUI(self);
    }
    public void OnPressdownPopupWindowButton(GameObject self)
    {
        self.GetComponent<UIRect>().alpha = 1f;
        self.SetActive(true);
        Volt_UILayerManager.instance.Enqueue(self);
    }
    public void OnPressdownCloseButton(GameObject self)
    {
        self.GetComponent<UIRect>().alpha = 0f;
        self.SetActive(false);
        Volt_UILayerManager.instance.RemoveUI(self);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Volt_UILayerManager.instance.Empty())
            {
                //PacketTransmission.SendUserDataForMovingLobbyPacket();
                //Volt_LoadingSceneManager.S.RequestLoadScene("Lobby2");
            }
            else
            {
                GameObject go = Volt_UILayerManager.instance.Dequeue();
                //go.SetActive(false);
                go.GetComponent<UIRect>().alpha = 1f;
            }
        }
    }

    
    public void RenewShopItemState(EShopPurchase itemType, int itemID)
    {
        switch (itemType)
        {
            case EShopPurchase.Package:
                break;
            case EShopPurchase.Skin:
                Volt_PlayerData.instance.RenewRobotSkinData(itemID);
                
                skinItemScrollView.SetDisabledItemByID(itemID);
                //플레이어 데이터
                break;
            case EShopPurchase.Emoticon:
                Volt_PlayerData.instance.RenewEmoticonData(itemID);

                emoticonItemScrollView.SetDisabledItemByID(itemID);
                break;
            default:
                break;
        }
    }
    //public void BoughtItemConfirmPopup(EAssetsType assetType, bool success)
    //{
    //    purchasedConfirmPopupPanel.Feedback(assetType, success);
    //    OnPressdownPopupWindowButton(purchasedConfirmPopupPanel.gameObject);
    //}
    public void BoughtItemConfirmPopup(EShopPurchase assetType, bool success)
    {
        purchasedConfirmPopupPanel.Feedback(assetType, success);
        OnPressdownPopupWindowButton(purchasedConfirmPopupPanel.gameObject);
    }
}
