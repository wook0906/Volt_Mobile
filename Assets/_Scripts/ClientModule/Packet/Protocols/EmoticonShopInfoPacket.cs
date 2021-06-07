using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class EmoticonShopInfoPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("EmoticonShopInfoPacket Unpack");
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

            DBManager.instance.emoticonShopInfos.Add(new InfoShop(id, assetType, price, 1));
            //Debug.Log("ID : " + id);
            //Debug.Log("AssetType : " + assetType.ToString());
            //Debug.Log("Price : " + price);
            //Debug.Log("Battery: " + diamond);

        }
        DBManager.instance.OnLoadedEmoticonShopInfo();
    }
}

