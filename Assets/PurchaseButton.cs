using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public int targetProductId;
    //EAssetsType obtainAssetType;
    public Volt.Shop.ObjectType objectType;
    [SerializeField]
    InfoShop thisItemInfo;
    public void Init(int ID)
    {
        targetProductId = ID;
        thisItemInfo = GetItemFromDBManager();
    }
    public InfoShop GetItemInfo()
    {
        if (thisItemInfo == null) return null;

        return thisItemInfo;
    }
    //public void Purchase()
    //{
    //    if (targetProductId == 3000005)
    //    {
    //        //Volt_RewardedAds.S.ShowRewardBasedAd();
    //        return;
    //    }

    //    if (objectType == Volt.Shop.ObjectType.Diamond)
    //    {
    //        DiamondPurchase();
    //        return;
    //    }

    //    if (IsThisAssetCanBeBought())
    //    {
    //        switch (objectType)
    //        {
    //            case Volt.Shop.ObjectType.Battery:
    //                BatteryPurchase();
    //                break;
    //            case Volt.Shop.ObjectType.Skin:
    //                SkinPurchase();
    //                break;
    //            default:
    //                //Debug.LogError("구매 에러! 알 수 없는 ObjectType");
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        Volt_ShopUIManager.S.OnPressdownPopupWindowButton(Volt_ShopUIManager.S.notEnoughAssetNoticePanel);
    //    }

        
    //    //foreach (var item in IAPManager.productCharacterSkins)
    //    //{
    //    //    if(item == targetProductId) //비소모성 아이템이라면 중복구매검사를 해야함.
    //    //    {
    //    //        if (IAPManager.Instance.HadPurchased(targetProductId))
    //    //        {
    //    //            Debug.Log("이미 구매했음");
    //    //            return;
    //    //        }
    //    //    }
    //    //}



    //}
    InfoShop GetItemFromDBManager()
    {
        foreach (var item in DBManager.instance.batteryShopInfos)
        {
            if (item.ID == targetProductId)
            {
                //Debug.Log("배터리 : " + item.ID);
                return item;
            }
        }
        foreach (var item in DBManager.instance.diamondShopInfos)
        {
            if (item.ID == targetProductId)
            {
                //Debug.Log("다이아 : " + item.ID);
                return item;
            }
        }
        foreach (var item in DBManager.instance.robotSkinShopInfos)
        {
            if (item.ID == targetProductId)
            {
                //Debug.Log("스킨 : " + item.ID);
                return item;
            }
        }
        foreach (var item in DBManager.instance.goldShopInfos)
        {
            if(item.ID == targetProductId)
            {
                return item;
            }
        }
        foreach (var item in DBManager.instance.packageShopInfos)
        {
            if(item.ID == targetProductId)
            {
                return item;
            }
        }
        foreach (var item in DBManager.instance.emoticonShopInfos)
        {
            if(item.ID == targetProductId)
            {
                return item;
            }
        }
        //Debug.LogError("없는데?");
        return null;
    }
    bool IsThisAssetCanBeBought()
    {
        switch ((EAssetsType)thisItemInfo.priceAssetType)
        {
            case EAssetsType.Battery:
                if (Volt_PlayerData.instance.BatteryCount >= thisItemInfo.price)
                    return true;
                return false;
            case EAssetsType.Gold:
                if (Volt_PlayerData.instance.GoldCount >= thisItemInfo.price)
                    return true;
                return false;
            default:
                return false;
        }
    }
    //void BatteryPurchase()
    //{
    //    //Debug.LogError("구매하는 에셋타입 : " + thisItem.ID);
    //    //해당패킷은 서버에서 자산 검증을 한다...
    //    PacketTransmission.SendBatteryPurchasePacket(thisItemInfo.price, thisItemInfo.count);
    //}
    //void DiamondPurchase()
    //{
    //    IAPManager.Instance.Purchase(IAPManager.Instance.ChangeDiamondIDToStoreID(targetProductId.ToString()));
    //}
    //void SkinPurchase()
    //{
    //    PacketTransmission.SendShopPurchasePacket(EShopPurchase.Skin, targetProductId);
    //}
    public void OnClickButton()
    {
        if(targetProductId == 3000005)
        {
            Managers.UI.ShowPopupUIAsync<ShowAdConfirm_Popup>();
            return;
        }

        Managers.Data.SetPurchaseProductInfo(thisItemInfo, Volt_Utils.GetEShopPurchaseWithId(thisItemInfo.ID));
        Managers.UI.ShowPopupUIAsync<BuyProduct_Popup>();

        //Volt_ShopUIManager.S.OnPressdownPopupWindowButton(Volt_ShopUIManager.S.purchaseConfirmPopupPanel.gameObject);
        //Volt_ShopUIManager.S.purchaseConfirmPopupPanel.RenewCurrentItem(this);
    }

}
