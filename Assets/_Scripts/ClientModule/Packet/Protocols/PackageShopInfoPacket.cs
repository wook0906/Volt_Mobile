using UnityEngine;

public class PackageShopInfoPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("PackageShopInfoPacket Unpack");
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

            DBManager.instance.packageShopInfos.Add(new InfoShop(id, assetType, price, 1));
        }
        DBManager.instance.OnLoadedPackageShopInfo();
    }
}

