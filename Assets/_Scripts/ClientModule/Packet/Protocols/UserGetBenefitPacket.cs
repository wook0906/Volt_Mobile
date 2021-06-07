using System;
using UnityEngine;


public class UserGetBenefitPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserGetBenefitPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        //패키지1 구입시 하루 제공되는 배터리5개를 받았는가 여부
        bool pack1battery = ByteConverter.ToBool(buffer, ref startIndex);
        Debug.Log(pack1battery);

        DBManager.instance.userBenefitCondition.Add(8000001, pack1battery);
        DBManager.instance.OnLoadedUserBenefitCondition();
    }
    
}

