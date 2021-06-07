using UnityEngine;
using UnityEditor;

public class DailyRewardPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("DailyRewardPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;
        int signInCount = ByteConverter.ToInt(buffer, ref startIndex);
        int playCount = ByteConverter.ToInt(buffer, ref startIndex);
        int winCount = ByteConverter.ToInt(buffer, ref startIndex);
        int vpCount = ByteConverter.ToInt(buffer, ref startIndex);
        int killCount = ByteConverter.ToInt(buffer, ref startIndex);
        bool signInReward = ByteConverter.ToBool(buffer, ref startIndex);
        bool playReward = ByteConverter.ToBool(buffer, ref startIndex);
        bool winReward = ByteConverter.ToBool(buffer, ref startIndex);
        bool vpReward = ByteConverter.ToBool(buffer, ref startIndex);
        bool killReward = ByteConverter.ToBool(buffer, ref startIndex);

        //Debug.Log("signInCount : " + signInCount+", "+
        //           "playCount : " + playCount + ", " +
        //           "winCount : " + winCount + ", " +
        //           "vpCount : " + vpCount + ", " +
        //           "killCount : " + killCount
        //           );
        //Debug.Log("signInReward : " + signInReward + ", " +
        //           "playReward : " + playReward + ", " +
        //           "winReward : " + winReward + ", " +
        //           "vpReward : " + vpReward + ", " +
        //           "killReward : " + killReward
        //           );
        return;
    }

}