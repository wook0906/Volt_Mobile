using UnityEngine;

public class BatteryTimeResponsePacket : Packet
{
    //BatteryTimeRequestPacket으로 인해 회신되어짐.
    public override void UnPack(byte[] buffer)
    {
        //Debug.LogError("BatteryTimeResponsePacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int year = ByteConverter.ToInt(buffer, ref startIndex);
        int month = ByteConverter.ToInt(buffer, ref startIndex);
        int day = ByteConverter.ToInt(buffer, ref startIndex);
        int hour = ByteConverter.ToInt(buffer, ref startIndex);
        int minute = ByteConverter.ToInt(buffer, ref startIndex);
        int second = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log("Year : "+ year);
        //Debug.Log("month : "+ month);
        //Debug.Log("day : "+ day);
        //Debug.Log("hour : "+ hour);
        //Debug.Log("minute : "+ minute);
        //Debug.Log("second : "+ second);

        Volt_PlayerData.instance.lastDateTime = new System.DateTime(year, month, day, hour, minute, second);
        if (BatteryCharge.instance)
            BatteryCharge.instance.OnLoadedLastTimerStartTime();
        //else
        //    Debug.LogError("인스턴스가 없는데...?");
    }
}