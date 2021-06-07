using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class UserDailyAchSuccessPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserDailyAchSuccessPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int rowCount = ByteConverter.ToInt(buffer, ref startIndex);
        //Debug.Log(rowCount);
        bool isDone;

        int id = 1000001;
        for (int i = 0; i < rowCount; i++)
        {
            isDone = ByteConverter.ToBool(buffer, ref startIndex);
            //DBManager.instance.daliyACHSuccessDatas.Add(id + i, isDone);
            
            if (isDone)
            {
                if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id + i))
                {
                    Volt_PlayerData.instance.AchievementProgresses.Add(id + i, new ACHProgress());
                }
                Volt_PlayerData.instance.AchievementProgresses[id + i].OnAccomplish();
            }
            //Debug.Log(id+i + " isDone? : " + isDone);
        }
        DBManager.instance.OnLoadedUserDaliyACHSuccess();
    }
}