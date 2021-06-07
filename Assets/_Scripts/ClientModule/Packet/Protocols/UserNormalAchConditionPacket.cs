using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class UserNormalAchConditionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("UserNormalAchConditionPacket Unpack");
        //int startIndex = PacketInfo.FromServerPacketSettingIndex;

        //int rowCount = ByteConverter.ToInt(buffer, ref startIndex);
        //Debug.Log(rowCount);
        //int id = 2000001;

        //for (int i = 0; i < rowCount; i++)
        //{
        //    int value = ByteConverter.ToInt(buffer, ref startIndex);
        //    DBManager.instance.userNormalACHCondition.achInfos.Add(new UserACHCondition.InfoACH(id + i, value));
        //    Debug.Log((id + i) + "daily ach count : " + value);
        //}

        //DBManager.instance.OnLoadedUserNormalACHCondition();
    }
}