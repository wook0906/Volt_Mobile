using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class UserDataForMovingLobbyPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserDataForMovingLobbyPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        
    }
}