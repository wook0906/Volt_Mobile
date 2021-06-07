using UnityEngine;

public class BatteryPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("BatteryPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int battery = ByteConverter.ToInt(buffer, ref startIndex);

        Debug.Log("현재 DB 배터리 값 : "+battery);

        Volt_PlayerData.instance.BatteryCount = battery;
        Volt_PlayerData.instance.RenewBatteryText(battery);

        LobbyScene_AssetUI assetUI = GameObject.FindObjectOfType<LobbyScene_AssetUI>();
        //LobbyScene_UI lobbyScene_UI = Managers.UI.GetSceneUI<LobbyScene_UI>();
        if (assetUI)
        {
            assetUI.SetBatteryCountLabel(battery);
        }
        else
        {
            Debug.LogError("Not Find asset UI");
        }
        ShopScene_UI shopSceneUI = GameObject.FindObjectOfType<ShopScene_UI>();
        if (shopSceneUI)
        {
            shopSceneUI.SetBatteryCountLabel(battery);
        }
        else
        {
            Debug.LogError("Not Find ShopScene UI");
        }
    }
}