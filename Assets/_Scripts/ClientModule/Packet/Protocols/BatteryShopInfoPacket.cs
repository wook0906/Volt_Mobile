using UnityEngine;
using UnityEditor;
using System;

public class BatteryShopInfoPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("BatterShopInfoPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int rowCount = ByteConverter.ToInt(buffer, ref startIndex);

        int id;
        int assetType;
        int price;
        int battery;

        for (int i = 0; i < rowCount; i++)
        {
            id = ByteConverter.ToInt(buffer, ref startIndex);
            assetType = ByteConverter.ToInt(buffer, ref startIndex);
            price = ByteConverter.ToInt(buffer, ref startIndex);
            battery = ByteConverter.ToInt(buffer, ref startIndex);
            
            //Debug.Log("ID : " + id);
            //Debug.Log("AssetType : " + assetType);
            //Debug.Log("Price : " + price);
            //Debug.Log("Battery: " + battery);

            DBManager.instance.batteryShopInfos.Add(new InfoShop(id, assetType, price, battery));
        }

        DBManager.instance.OnLoadedBatteryShopInfo();
    }
}