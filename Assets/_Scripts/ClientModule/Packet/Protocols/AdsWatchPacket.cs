using UnityEngine;

public class AdsWatchPacket : Packet
{
    enum StateAdsWatch { Success=1, FailNotCount, FailNotTime}
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("AdsWatchPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;
        int state = ByteConverter.ToInt(buffer, ref startIndex);
        int remainTime = ByteConverter.ToInt(buffer, ref startIndex);//≥≤¿∫ √ 
        int remainCount = ByteConverter.ToInt(buffer, ref startIndex);

        Debug.Log(((StateAdsWatch)state).ToString());
        Debug.Log(remainTime);
        Debug.Log(remainCount);

        switch ((StateAdsWatch)state)
        {
            case StateAdsWatch.Success:
                Volt_PlayerData.instance.RemainAdCnt = remainCount;
                if (remainCount <= 0) return;
                AdCharge.instance.SetTimer(remainTime);
                break;
            case StateAdsWatch.FailNotCount:
                break;
            case StateAdsWatch.FailNotTime:
                break;
        }
    }
}