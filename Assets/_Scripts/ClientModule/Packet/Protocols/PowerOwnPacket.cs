using UnityEngine;

public class PowerOwnPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("PowerOwnPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        bool skipAds = ByteConverter.ToBool(buffer, ref startIndex);

        DBManager.instance.userPackageCondition.Add(8000001, new Define.PackageProductState(skipAds));
        DBManager.instance.OnLoadedUserPackageCondition();
    }
}

