using UnityEngine;

public class BatteryTimeRequestPacket : Packet
{
    //BatteryTimeResponse 패킷을 회신받음.
    public override void UnPack(byte[] buffer)
    {
        
        Debug.Log("BatteryTimeRequestPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int year = ByteConverter.ToInt(buffer, ref startIndex);
        int month = ByteConverter.ToInt(buffer, ref startIndex);
        int day = ByteConverter.ToInt(buffer, ref startIndex);
        int hour = ByteConverter.ToInt(buffer, ref startIndex);
        int minute = ByteConverter.ToInt(buffer, ref startIndex);
        int second = ByteConverter.ToInt(buffer, ref startIndex);

        Debug.Log("Year : "+ year);
        Debug.Log("month : "+ month);
        Debug.Log("day : "+ day);
        Debug.Log("hour : "+ hour);
        Debug.Log("minute : "+ minute);
        Debug.Log("second : "+ second);

        

    }
}