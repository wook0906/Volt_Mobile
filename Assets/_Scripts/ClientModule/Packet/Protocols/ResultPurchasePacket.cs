
using UnityEngine;

//ResultPurchasePacket : 패키지purchase 성공시에 패키지id값과 결과값 (0:성공 1실패)날라옵니다.
public class ResultPurchasePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("ResultPurchasePacket Unpack");
        int startindex = PacketInfo.FromServerPacketSettingIndex;

        int id = ByteConverter.ToInt(buffer, ref startindex);//남은 초
        int result = ByteConverter.ToInt(buffer, ref startindex);


        if(result == 1)
        {
            Managers.UI.ShowPopupUIAsync<PurchaseFail_Popup>();
            //fail( because not enough money)
            return;
        }

        if(id == 8000001)//package1
        {
            Volt_PlayerData.instance.RenewPackageData(Managers.Data.CurrentProductInfoShop.ID);
            ShopScene_UI shopSceneUI = Managers.UI.GetSceneUI<ShopScene_UI>();
            shopSceneUI.OnPurchasedPackage(Managers.Data.CurrentProductInfoShop.ID);
            Volt_PlayerData.instance.OnPurchasedPackage(8000001);
        }
    }
}
