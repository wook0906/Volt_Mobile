using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseComplete_Popup : UI_Popup
{
    enum Labels
    {
        ResultMsg_Label
    }

    enum Buttons
    {
        Ok_Btn
    }

    public override void Init()
    {
        base.Init();
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));

        ShopScene_UI shopSceneUI = null;
        GetButton((int)Buttons.Ok_Btn).onClick.Add(new EventDelegate(() =>
        {
            shopSceneUI = Managers.UI.GetSceneUI<ShopScene_UI>();
            shopSceneUI.InActiveBlock();
            ClosePopupUI();
        }));

        Define.PurchaseProductResult result = Managers.Data.PurchaseProductResult;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                if (result.isSuccess)
                    GetLabel((int)Labels.ResultMsg_Label).text = "Réussite des achats";
                else
                    GetLabel((int)Labels.ResultMsg_Label).text = "L'achat a échoué";
                break;
            case SystemLanguage.German:
                if (result.isSuccess)
                    GetLabel((int)Labels.ResultMsg_Label).text = "Kauf erfolgreich";
                else
                    GetLabel((int)Labels.ResultMsg_Label).text = "Kauf fehlgeschlagen";
                break;
            case SystemLanguage.Korean:
                if (result.isSuccess)
                    GetLabel((int)Labels.ResultMsg_Label).text = "구매 성공";
                else
                    GetLabel((int)Labels.ResultMsg_Label).text = "구매 실패";
                break;
            default:
                if (result.isSuccess)
                    GetLabel((int)Labels.ResultMsg_Label).text = "Purchase Success";
                else
                    GetLabel((int)Labels.ResultMsg_Label).text = "Purchase Failed";
                break;
        }
       
        switch (Managers.Data.CurrentProductType)
        {
            //상점정보가 갱신되어야하는놈들만 여기서 갱신
            case EShopPurchase.Package:
                Volt_PlayerData.instance.RenewPackageData(Managers.Data.CurrentProductInfoShop.ID);
                shopSceneUI = FindObjectOfType<ShopScene_UI>();
                shopSceneUI.OnPurchasedPackage(Managers.Data.CurrentProductInfoShop.ID);
                break;
            case EShopPurchase.Skin:
                Volt_PlayerData.instance.RenewRobotSkinData(Managers.Data.CurrentProductInfoShop.ID);
                shopSceneUI = FindObjectOfType<ShopScene_UI>();
                shopSceneUI.OnPurchasedRobotSkin(Managers.Data.CurrentProductInfoShop.ID);
                //플레이어 데이터
                break;
            case EShopPurchase.Emoticon:
                Volt_PlayerData.instance.RenewEmoticonData(Managers.Data.CurrentProductInfoShop.ID);
                shopSceneUI = FindObjectOfType<ShopScene_UI>();
                shopSceneUI.OnPurchasedEmoticon(Managers.Data.CurrentProductInfoShop.ID);
                break;
            default:
                break;
        }
    }
}
