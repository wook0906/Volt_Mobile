using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class UserDailyAchConditionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("UserDailyAchConditionPacket Unpack");
        //int startIndex = PacketInfo.FromServerPacketSettingIndex;

        //int rowCount = ByteConverter.ToInt(buffer, ref startIndex);
        //Debug.Log(rowCount);

        //int id = 1000001;

        //for (int i = 0; i < rowCount; i++)
        //{
        //    int value = ByteConverter.ToInt(buffer, ref startIndex);
        //    DBManager.instance.userDaliyACHCondition.achInfos.Add(new UserACHCondition.InfoACH(id + i, value));
        //    Debug.Log("[" + (id + i) + "]daily ach value : " + value);
        //}
        //DBManager.instance.OnLoadedUserDaliyACHCondition();
    }
}