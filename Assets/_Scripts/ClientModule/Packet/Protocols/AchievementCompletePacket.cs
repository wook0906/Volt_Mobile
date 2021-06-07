using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class AchievementCompletePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("AchievementCompletePacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int achId = ByteConverter.ToInt(buffer, ref startIndex);
        bool isDone = ByteConverter.ToBool(buffer, ref startIndex);

        Debug.Log("achId : " + achId);
        Debug.Log(achId +" Done? : " + isDone);

        Debug.Log("이 로그가찍히면 뭔가이상한거임");

        //ACHItem completedACH;
        //if (achId > 2000000)
        //{
        //    completedACH = ACHUI.instance.normalScrollViewItemCreator.GetACHItemByID(achId);
        //    completedACH.ACHComplete();
        //}
        //else
        //{
        //    completedACH = ACHUI.instance.dailyScrollViewItemCreator.GetACHItemByID(achId);
        //    completedACH.ACHComplete();
        //}
    }
}