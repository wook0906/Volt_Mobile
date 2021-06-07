using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyProduct_Popup : UI_Popup
{
    enum Buttons
    {
        Yes_Btn,
        No_Btn,
        ShowWithdrawal_Btn
    }

    enum Labels
    {
        ProductName_Label,
        SemiWithdrawl_Label
    }

    enum Sprites
    {
        Product_Image,
        BG,
        Warning_Icon
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
        GetButton((int)Buttons.ShowWithdrawal_Btn).onClick.Add(new EventDelegate(() =>
        {
            Managers.UI.ShowPopupUIAsync<WithdrawalOfSubscription_Popup>();
        }));

        if(Managers.Data.CurrentProductType == EShopPurchase.Diamond ||
           Managers.Data.CurrentProductType == EShopPurchase.Package)
        {
            GetSprite((int)Sprites.BG).spriteName = "middle panel03_Opaque";
            GetSprite((int)Sprites.BG).width = 1320;
            GetSprite((int)Sprites.BG).height = 909;
            GetLabel((int)Labels.SemiWithdrawl_Label).gameObject.SetActive(true);
            GetSprite((int)Sprites.Warning_Icon).gameObject.SetActive(true);
            GetButton((int)Buttons.ShowWithdrawal_Btn).gameObject.SetActive(true);
        }
        else
        {
            GetSprite((int)Sprites.BG).spriteName = "textbox02_Opaque";
            GetSprite((int)Sprites.BG).width = 732;
            GetSprite((int)Sprites.BG).height = 966;
            GetLabel((int)Labels.SemiWithdrawl_Label).gameObject.SetActive(false);
            GetSprite((int)Sprites.Warning_Icon).gameObject.SetActive(false);
            GetButton((int)Buttons.ShowWithdrawal_Btn).gameObject.SetActive(false);
        }
        string itemName = string.Empty;
        switch (Managers.Data.CurrentProductType)
        {
            case EShopPurchase.Package:
                GetSprite((int)Sprites.Product_Image).width = 300;
                GetSprite((int)Sprites.Product_Image).height = 300; ;
                Dictionary<int, Volt.Shop.PackageShopModel> packageTable = Volt.Shop.ShopDataManager.instance.GetPackageShopItemTable();

                GetSprite((int)Sprites.Product_Image).spriteName = packageTable[Managers.Data.CurrentProductInfoShop.ID].objectICON;
                itemName = Volt_Utils.GetItemNameByLanguage(Managers.Data.CurrentProductType);
                GetLabel((int)Labels.ProductName_Label).text = $"{itemName}";

                break;
            case EShopPurchase.Skin:
                Volt.Shop.RobotSkinShopModel item = Volt.Shop.ShopDataManager.instance.SearchRobotSkinShopModel(Managers.Data.CurrentProductInfoShop.ID);

                if(item == null)
                {
                    Debug.LogError($"Error Not find item [itemID:{Managers.Data.CurrentProductInfoShop.ID}]");
                    break;
                }
                GetSprite((int)Sprites.Product_Image).width = 324;
                GetSprite((int)Sprites.Product_Image).height = 395;
                GetSprite((int)Sprites.Product_Image).spriteName = item.skinSprite;
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.French:
                        GetLabel((int)Labels.ProductName_Label).text = item.skinName_Fren;
                        break;
                    case SystemLanguage.German:
                        GetLabel((int)Labels.ProductName_Label).text = item.skinName_GER;
                        break;
                    case SystemLanguage.Korean:
                        GetLabel((int)Labels.ProductName_Label).text = item.skinName_KOR;
                        break;
                    default:
                        GetLabel((int)Labels.ProductName_Label).text = item.skinName_EN;
                        break;
                }
                break;
            case EShopPurchase.Emoticon:
                GetSprite((int)Sprites.Product_Image).width = 300;
                GetSprite((int)Sprites.Product_Image).height = 300;
                Dictionary<Volt.Shop.ShopRobotSelectType, Volt.Shop.EmoticonShopModel[]> emoticonTables = Volt.Shop.ShopDataManager.instance.GetEmoticonShopItemTable();
                foreach (var emoticonModels in emoticonTables.Values)
                {
                    foreach (var emoticonModel in emoticonModels)
                    {
                        if (emoticonModel.ID == Managers.Data.CurrentProductInfoShop.ID)
                        {
                            GetSprite((int)Sprites.Product_Image).spriteName = emoticonModel.emoticonSprite;
                            return;
                        }
                    }
                }
                itemName = Volt_Utils.GetItemNameByLanguage(Managers.Data.CurrentProductType);
                GetLabel((int)Labels.ProductName_Label).text = $"{itemName} {Managers.Data.CurrentProductInfoShop.count} + 개";
              
                break;
            case EShopPurchase.Gold:
                GetSprite((int)Sprites.Product_Image).width = 300;
                GetSprite((int)Sprites.Product_Image).height = 300;
                Dictionary<int, Volt.Shop.GoldShopModel> goldTable = Volt.Shop.ShopDataManager.instance.GetGoldShopItemTable();
                GetSprite((int)Sprites.Product_Image).spriteName = goldTable[Managers.Data.CurrentProductInfoShop.ID].objectICON;
                itemName = Volt_Utils.GetItemNameByLanguage(Managers.Data.CurrentProductType);

                GetLabel((int)Labels.ProductName_Label).text = $"{itemName} {Managers.Data.CurrentProductInfoShop.count} + 개";
                break;
            case EShopPurchase.Diamond:
                {
                    GetSprite((int)Sprites.Product_Image).width = 300;
                    GetSprite((int)Sprites.Product_Image).height = 300;;
                    Dictionary<int, Volt.Shop.DiamondShopModel> diamondTable = Volt.Shop.ShopDataManager.instance.GetDiamondShopItemTable();

                    GetSprite((int)Sprites.Product_Image).spriteName = diamondTable[Managers.Data.CurrentProductInfoShop.ID].objectICON;
                    itemName = Volt_Utils.GetItemNameByLanguage(Managers.Data.CurrentProductType);
                    GetLabel((int)Labels.ProductName_Label).text = $"{itemName} {Managers.Data.CurrentProductInfoShop.count} + 개";
                }
                break;
            case EShopPurchase.Battery:
                {
                    GetSprite((int)Sprites.Product_Image).width = 300;
                    GetSprite((int)Sprites.Product_Image).height = 300;
                    Dictionary<int, Volt.Shop.BatteryShopModel> batteryTable = Volt.Shop.ShopDataManager.instance.GetBatteryShopItemTable();
                    GetSprite((int)Sprites.Product_Image).spriteName = batteryTable[Managers.Data.CurrentProductInfoShop.ID].objectICON;
                    itemName = Volt_Utils.GetItemNameByLanguage(Managers.Data.CurrentProductType);
                    
                    GetLabel((int)Labels.ProductName_Label).text = $"{itemName} {Managers.Data.CurrentProductInfoShop.count} + 개";
                }
                break;
            default:
                break;
        }
    }

    private void OnClickConfirm()
    {
        if (IsThisAssetCanBeBought())
        {
            switch (Managers.Data.CurrentProductType)
            {
                case EShopPurchase.Battery:
                    PacketTransmission.SendBatteryPurchasePacket(Managers.Data.CurrentProductInfoShop.price,
                        Managers.Data.CurrentProductInfoShop.count);
                    //PacketTransmission.SendShopPurchasePacket(EShopPurchase.Battery, Managers.Data.CurrentProductInfoShop.ID);
                    break;
                case EShopPurchase.Skin:
                    PacketTransmission.SendShopPurchasePacket(EShopPurchase.Skin, Managers.Data.CurrentProductInfoShop.ID);
                    break;
                case EShopPurchase.Gold:
                    PacketTransmission.SendShopPurchasePacket(EShopPurchase.Gold, Managers.Data.CurrentProductInfoShop.ID);
                    break;
                case EShopPurchase.Emoticon:
                    PacketTransmission.SendShopPurchasePacket(EShopPurchase.Emoticon, Managers.Data.CurrentProductInfoShop.ID);
                    break;
                case EShopPurchase.Diamond:
                    IAPManager.Instance.Purchase(IAPManager.Instance.ChangeProjectItemIDToStoreID(Managers.Data.CurrentProductInfoShop.ID.ToString()));
                    break;
                case EShopPurchase.Package:
                    IAPManager.Instance.Purchase(IAPManager.Instance.ChangeProjectItemIDToStoreID(Managers.Data.CurrentProductInfoShop.ID.ToString()));
                    break;
                default:
                    //Debug.LogError("구매 에러! 알 수 없는 ObjectType");
                    break;
            }
        }
        else
        {
            FindObjectOfType<ShopScene_UI>().InActiveBlock();
            ClosePopupUI();
            Managers.UI.ShowPopupUIAsync<PurchaseFail_Popup>();
            return;
        }

        ShopScene_UI scene_UI = Managers.UI.GetSceneUI<ShopScene_UI>();
        scene_UI.ActiveBlock();
        ClosePopupUI();
    }

    private bool IsThisAssetCanBeBought()
    {
        switch ((EAssetsType)Managers.Data.CurrentProductInfoShop.priceAssetType)
        {
            case EAssetsType.Battery:
                if (Volt_PlayerData.instance.BatteryCount >= Managers.Data.CurrentProductInfoShop.price)
                    return true;
                return false;
            case EAssetsType.Diamond:
                if (Volt_PlayerData.instance.DiamondCount >= Managers.Data.CurrentProductInfoShop.price)
                    return true;
                return false;
            case EAssetsType.Gold:
                if (Volt_PlayerData.instance.GoldCount >= Managers.Data.CurrentProductInfoShop.price)
                    return true;
                return false;
            case EAssetsType.Money:
                return true;
            default:
                return false;
        }
    }
}
