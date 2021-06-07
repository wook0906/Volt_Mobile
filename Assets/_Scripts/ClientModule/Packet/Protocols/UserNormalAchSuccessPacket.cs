using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class UserNormalAchSuccessPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserNormalAchSuccessPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int rowCount = ByteConverter.ToInt(buffer, ref startIndex);
        //Debug.Log(rowCount);
        bool isDone;
        
        int id = 2000001;
        for (int i = 0; i < rowCount; i++)
        {
            isDone = ByteConverter.ToBool(buffer, ref startIndex);

            if (isDone)
            {
                if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id + i))
                    Volt_PlayerData.instance.AchievementProgresses.Add(id + i, new ACHProgress());

                Volt_PlayerData.instance.AchievementProgresses[id + i].OnAccomplish();
            }
            //Debug.Log(id+i + " isDone? : " + isDone);
        }

        DBManager.instance.OnLoadedUserNormalACHSuccess();
    }
}