using UnityEngine;
using UnityEditor;

public class VictoryPointPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("VictoryPointPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        int vp = ByteConverter.ToInt(buffer, ref startIndex);

        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        player.VictoryPoint = vp;
        //Debug.Log($"{playerNumber} player Get/Lost Victory Point cause {getVpType.ToString()}");
    }

}