using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class IAPPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("IAPPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        
    }
}