using UnityEngine;
using UnityEditor;

public class DailyAchievementInfoPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("DailyAchievementInfoPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int rowCount = ByteConverter.ToInt(buffer, ref startIndex);

        int id;
        int conditionType;
        int condition;
        int rewardType;
        int reward;

        for (int i = 0; i < rowCount; i++)
        {
            id = ByteConverter.ToInt(buffer, ref startIndex);
            conditionType = ByteConverter.ToInt(buffer, ref startIndex);
            condition = ByteConverter.ToInt(buffer, ref startIndex);
            rewardType = ByteConverter.ToInt(buffer, ref startIndex);
            reward = ByteConverter.ToInt(buffer, ref startIndex);

            //Debug.Log("ID : " + id);
            //Debug.Log("Condition Type : " + conditionType.ToString());
            //Debug.Log("Condition : " + condition);
            //Debug.Log("Reward Type: " + rewardType.ToString());
            //Debug.Log("Reward : " + reward);

            DBManager.instance.daliyACHConditionInfos.Add(new InfoACHCondition(id, conditionType, condition,
                rewardType, reward));
        }

        DBManager.instance.OnLoadedDaliyAchievementInfo();
    }

}