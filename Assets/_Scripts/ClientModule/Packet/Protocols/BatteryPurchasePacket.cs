using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class BatteryPurchasePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("ShopPurchasePacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int neededGoldPrice = ByteConverter.ToInt(buffer, ref startIndex);
        int purchasedBatteryCount = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log($"neeededGoldPrice : {neededGoldPrice.ToString()} purchasedBatteryCount : {purchasedBatteryCount.ToString()}");

        Volt_PlayerData.instance.GoldCount -= neededGoldPrice;
        Volt_PlayerData.instance.BatteryCount += purchasedBatteryCount;

        Volt_PlayerData.instance.RenewGoldText(Volt_PlayerData.instance.GoldCount);
        Volt_PlayerData.instance.RenewBatteryText(Volt_PlayerData.instance.BatteryCount);


        Managers.UI.ShowPopupUIAsync<PurchaseComplete_Popup>();
        //Volt_ShopUIManager.S.BoughtItemConfirmPopup(EShopPurchase.Battery, true);
    }
}