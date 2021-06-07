using UnityEngine;
using UnityEditor;

public class ReConnectionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        int startIndex = PacketInfo.FromServerPacketDataStartIndex;
        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        //끊겼던 플레이어.

        // AI -> Player
        Volt_PlayerManager.S.ReturnToPlayer(playerNumber);
        Volt_PlayerManager.S.ChangeHostPlayerUnActive(playerNumber);
    }
}