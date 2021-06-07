using UnityEngine;

public class AdsCheckPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("AdsCheckPacket Unpack");
        int startindex = PacketInfo.FromServerPacketSettingIndex;
        int remaintime = ByteConverter.ToInt(buffer, ref startindex);//≥≤¿∫ √ 
        int remaincount = ByteConverter.ToInt(buffer, ref startindex);


        //Debug.Log(remaintime);
        //Debug.Log(remaincount);
        Volt_PlayerData.instance.RemainAdCnt = remaincount;
        AdCharge.instance.SetTimer(remaintime);

    }
}