using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class ShopPurchasePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("ShopPurchasePacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        EShopPurchase itemType = (EShopPurchase)ByteConverter.ToInt(buffer, ref startIndex);
        int itemID = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log($"Purchased item type : {itemType.ToString()} item Id : {itemID}");

        Managers.Data.SetPurchaseProductResult(itemType, true);
        Managers.UI.ShowPopupUIAsync<PurchaseComplete_Popup>();
        //Volt_ShopUIManager.S.BoughtItemConfirmPopup(EShopPurchase.Skin, true);

        //Volt_ShopUIManager.S.RenewShopItemState(itemType, itemID);
        //비 소모성 아이템일 경우 샵버튼 비활성화 -> 이미 구매하였음
    }
}