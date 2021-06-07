using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class UserDailyAchConditionStatePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserDailyAchConditionStatePacket Unpack");
        
        int startIndex = PacketInfo.FromServerPacketSettingIndex;
        
        int id = 1000001;
        for (int i = 0; i < (int)EDailyConditionType.End; i++)
        {
            id = 1000001 + i;
            int value = ByteConverter.ToInt(buffer, ref startIndex);
            //Debug.Log("[" + (id + i) + "]daily ach value : " + value);
            
            if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id))
            {
                Volt_PlayerData.instance.AchievementProgresses.Add(id, new ACHProgress());
            }
            Volt_PlayerData.instance.AchievementProgresses[id].SetAchievementProgress(value);
        }
        DBManager.instance.OnLoadedUserDaliyACHCondition();
    }
}