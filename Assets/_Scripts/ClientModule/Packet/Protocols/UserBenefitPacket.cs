using System;
using UnityEngine;


public class UserBenefitPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserBenefitPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        //user 유저가 이득을 받을 '권한'을 가지고 있는가 여부
        bool pack1battery = ByteConverter.ToBool(buffer, ref startIndex);

    }
    
}


