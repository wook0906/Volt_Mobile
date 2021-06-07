using UnityEngine;

public class DiamondPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("DiamondPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int diamond = ByteConverter.ToInt(buffer, ref startIndex);

        Debug.Log("현재 DB 다이아 값 : "+ diamond);

        Volt_PlayerData.instance.DiamondCount = diamond;
        Volt_PlayerData.instance.RenewDiamondText(diamond);
    }
}