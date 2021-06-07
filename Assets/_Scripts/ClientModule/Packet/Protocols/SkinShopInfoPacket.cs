using UnityEngine;
using UnityEditor;

public class SkinShopInfoPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("SkinShopInfoPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int rowCount = ByteConverter.ToInt(buffer, ref startIndex);

        int id;
        int assetType;
        int price;

        for (int i = 0; i < rowCount; i++)
        {
            id = ByteConverter.ToInt(buffer, ref startIndex);
            assetType = ByteConverter.ToInt(buffer, ref startIndex);
            price = ByteConverter.ToInt(buffer, ref startIndex);
            
            //Debug.Log("ID : " + id);
            //Debug.Log("AssetType : " + assetType.ToString());
            //Debug.Log("Price : " + price);

            DBManager.instance.robotSkinShopInfos.Add(new InfoShop(id, assetType, price, 1));
        }
        DBManager.instance.OnLoadedRobotSkinShopInfo();
    }

}