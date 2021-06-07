using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class AchievementCompletionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("AchievementCompletionPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int achId = ByteConverter.ToInt(buffer, ref startIndex);
        bool isDone = ByteConverter.ToBool(buffer, ref startIndex);

        //Debug.Log("achId : " + achId);
        //Debug.Log(achId +" Done? : " + isDone);


        ACHItem completedACH;
        if (achId > 2000000)
        {
            AchScene_UI achScene_UI = GameObject.FindObjectOfType<AchScene_UI>();

            completedACH = achScene_UI.GetNormalAchScrollViewItemCreator().GetACHItemByID(achId);
            completedACH.ACHComplete();
            
        }
        else
        {
            AchScene_UI achScene_UI = GameObject.FindObjectOfType<AchScene_UI>();

            completedACH = achScene_UI.GetDailyAchScrollViewItemCreator().GetACHItemByID(achId);
            completedACH.ACHComplete();
        }
    }
}