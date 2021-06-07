using UnityEngine;

public class GoldPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("GoldPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int gold = ByteConverter.ToInt(buffer, ref startIndex);

        Debug.Log("현재 DB 골드 값 : "+gold);

        Volt_PlayerData.instance.GoldCount = gold;
        Volt_PlayerData.instance.RenewGoldText(gold);

      
    }
}