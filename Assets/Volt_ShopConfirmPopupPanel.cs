using System.Collections.Generic;
using UnityEngine;

public class Volt_ShopConfirmPopupPanel : MonoBehaviour
{
    private string withoutWithdrawlBGSpriteName = "textbox02_Opaque";
    public int withoutWithdrawlWidth;
    public int withoutWithdrawlHeight;
    private string withdrawlBGSpriteName = "middle panel03_Op";
    public int withdrawlWidth;
    public int withdrawlHeight;

    public int skinSpriteWith;
    public int skinSpriteHeight;

    public int elseItemSpriteWithAndHeight;

    public UISprite itemSprite;
    public UILabel itemNameLabel;
    PurchaseButton currentItemPurchaseBtn;
    public GameObject withdrawlOfSubscriptionPanel;

    public UILabel withdrawlLabel;
    public UIButton showWithdrawlBtn;
    public UISprite warningIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RenewCurrentItem(PurchaseButton newItemBtn)
    {
        Debug.Log("renew!");
        currentItemPurchaseBtn = newItemBtn;

        InfoShop info = currentItemPurchaseBtn.GetItemInfo();
        EShopPurchase itemType = Volt_Utils.GetEShopPurchaseWithId(info.ID);
        string itemName;
        UISprite thisBG = GetComponentInChildren<UISprite>();

        if(itemType == EShopPurchase.Diamond)
        {
            thisBG.spriteName = withdrawlBGSpriteName;
            thisBG.width = withdrawlWidth;
            thisBG.height = withdrawlHeight;
            withdrawlLabel.gameObject.SetActive(true);
            showWithdrawlBtn.gameObject.SetActive(true);
            warningIcon.gameObject.SetActive(true);
        }
        else
        {
            thisBG.spriteName = withoutWithdrawlBGSpriteName;
            thisBG.width = withoutWithdrawlWidth;
            thisBG.height = withoutWithdrawlHeight;
            withdrawlLabel.gameObject.SetActive(false);
            showWithdrawlBtn.gameObject.SetActive(false);
            warningIcon.gameObject.SetActive(false);
        }
        

        //사고자하는 아이템 보여주기 갱신
        switch (itemType)
        {
            case EShopPurchase.Package:
                break;
            case EShopPurchase.Skin:
                itemSprite.width = skinSpriteWith;
                itemSprite.height = skinSpriteHeight;
                Dictionary<Volt.Shop.ShopRobotSelectType, Volt.Shop.RobotSkinShopModel[]> skinTable = Volt.Shop.ShopDataManager.instance.GetRobotSkinShopItemTable();
                foreach (var item in skinTable[Volt_ShopUIManager.S.curTapRobotType])
                {
                    if (item.ID == info.ID)
                    {
                        itemSprite.atlas = AtlasManager.instance.GetAtlas(item.skinAtlas);
                        itemSprite.spriteName = item.skinSprite;
                        switch (Application.systemLanguage)
                        {
                            case SystemLanguage.French:
                                itemNameLabel.text = item.skinName_Fren;
                                break;
                            case SystemLanguage.German:
                                itemNameLabel.text = item.skinName_GER;
                                break;               
                            case SystemLanguage.Korean:
                                itemNameLabel.text = item.skinName_KOR;
                                break;
                            default:
                                itemNameLabel.text = item.skinName_EN;
                                break;
                        }
                        break;
                    }
                }
                break;
            case EShopPurchase.Emoticon:
                break;
            case EShopPurchase.Gold:
                break;
            case EShopPurchase.Diamond:
                itemSprite.width = elseItemSpriteWithAndHeight;
                itemSprite.height = elseItemSpriteWithAndHeight;
                Dictionary<int, Volt.Shop.DiamondShopModel> diamondTable = Volt.Shop.ShopDataManager.instance.GetDiamondShopItemTable();
                itemSprite.atlas = AtlasManager.instance.GetAtlas(diamondTable[info.ID].iconAtlas);
                itemSprite.spriteName = diamondTable[info.ID].objectICON;
                itemName = Volt_Utils.GetItemNameByLanguage(itemType);
                itemNameLabel.text = itemName + " "+ info.count.ToString()+"개";
                break;
            case EShopPurchase.Battery:
                Debug.Log("Battery Renew");
                itemSprite.width = elseItemSpriteWithAndHeight;
                itemSprite.height = elseItemSpriteWithAndHeight;
                Dictionary<int, Volt.Shop.BatteryShopModel> batteryTable = Volt.Shop.ShopDataManager.instance.GetBatteryShopItemTable();
                itemSprite.atlas = AtlasManager.instance.GetAtlas(batteryTable[info.ID].iconAtlas);
                itemSprite.spriteName = batteryTable[info.ID].objectICON;
                itemName = Volt_Utils.GetItemNameByLanguage(itemType);
                Debug.Log(itemName+info.count.ToString());
                itemNameLabel.text = itemName + " "+ info.count.ToString() + "개";
                break;
            default:
                break;
        }
        Debug.Log("renew End!");
    }
    
    public void OnClickPurchaseConfirm()
    {
        //currentItemPurchaseBtn.Purchase();
    }
    public void OnShowWithdrawl()
    {
        Volt_ShopUIManager.S.OnPressdownPopupWindowButton(withdrawlOfSubscriptionPanel);
    }
    public void OffShowWithdrawl()
    {
        Volt_ShopUIManager.S.OnPressdownCloseButton(withdrawlOfSubscriptionPanel);
    }
}
