using UnityEngine;
using UnityEditor;

public class DiamondShopInfoPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("DiamondShopInfoPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int rowCount = ByteConverter.ToInt(buffer, ref startIndex);

        int id;
        int assetType;
        int price;
        int diamond;

        for (int i = 0; i < rowCount; i++)
        {
            id = ByteConverter.ToInt(buffer, ref startIndex);
            assetType = ByteConverter.ToInt(buffer, ref startIndex);
            price = ByteConverter.ToInt(buffer, ref startIndex);
            diamond = ByteConverter.ToInt(buffer, ref startIndex);

            DBManager.instance.diamondShopInfos.Add(new InfoShop(id, assetType, price, diamond));
            //Debug.Log("ID : " + id);
            //Debug.Log("AssetType : " + assetType.ToString());
            //Debug.Log("Price : " + price);
            //Debug.Log("Battery: " + diamond);

        }

        DBManager.instance.OnLoadedDiamondShopInfo();
    }

}