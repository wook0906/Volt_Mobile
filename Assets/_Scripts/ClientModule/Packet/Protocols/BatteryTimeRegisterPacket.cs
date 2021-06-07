using UnityEngine;

public class BatteryTimeRegisterPacket : Packet
{
    //서버에다가 현재 시간을 업데이트함.
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("BatteryTimeRegisterPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;   
    }
}