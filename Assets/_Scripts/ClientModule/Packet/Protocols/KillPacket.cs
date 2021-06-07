using UnityEngine;
using UnityEditor;

public class KillPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("KillPacket Unpack");
        //int startIndex = PacketInfo.FromServerPacketSettingIndex;

        
        return;
    }

}