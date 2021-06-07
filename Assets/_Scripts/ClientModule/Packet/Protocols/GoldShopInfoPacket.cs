using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GoldShopInfoPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("GoldShopInfoPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int rowCount = ByteConverter.ToInt(buffer, ref startIndex);

        int id;
        int assetType;
        int price;
        int gold;

        for (int i = 0; i < rowCount; i++)
        {
            id = ByteConverter.ToInt(buffer, ref startIndex);
            assetType = ByteConverter.ToInt(buffer, ref startIndex);
            price = ByteConverter.ToInt(buffer, ref startIndex);
            gold = ByteConverter.ToInt(buffer, ref startIndex);

            DBManager.instance.goldShopInfos.Add(new InfoShop(id, assetType, price, gold));

        }
        DBManager.instance.OnLoadedGoldShopInfo();
    }
}