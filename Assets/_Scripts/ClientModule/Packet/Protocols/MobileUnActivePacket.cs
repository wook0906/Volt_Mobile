using UnityEngine;

public class MobileUnActivePacket : Packet 
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("MobileUnActivePacket Unpack");
        int startIndex = PacketInfo.FromServerPacketDataStartIndex;
        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        Volt_PlayerManager.S.ReceiveFocusMSG(playerNumber, false);
        Volt_PlayerManager.S.ChangeHostPlayerUnActive(playerNumber);
    }
}