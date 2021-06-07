using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class AchievementProgressPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("AchievementProgressPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int achId = ByteConverter.ToInt(buffer, ref startIndex);
        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);

        Debug.Log($"{playerNumber} player achId : {achId} renewed");
    }
}