using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class SignUpSuccessPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("SignUpSuccessPacket Unpack");
        Debug.Log("계정 생성에 성공하였음");
        DBManager.instance.ClearDB();

        int startIndex = PacketInfo.FromServerPacketSettingIndex;
        int nicknameLength = ByteConverter.ToInt(buffer, ref startIndex);
        string nickname = ByteConverter.ToString(buffer, ref startIndex, nicknameLength);
        int battery = ByteConverter.ToInt(buffer, ref startIndex);
        int gold = ByteConverter.ToInt(buffer, ref startIndex);
        int diamond = ByteConverter.ToInt(buffer, ref startIndex);
        //int rankPoint = ByteConverter.ToInt(buffer, ref startIndex);

        Debug.Log(nicknameLength);
        Debug.Log("Nickname : " + nickname);
        Debug.Log("Battery : " + battery);
        Debug.Log("Gold : " + gold);
        Debug.Log("Diamond : " + diamond);
        //Debug.Log("RankPoint : " + rankPoint);
        DBManager.instance.userData = new UserData(nickname, battery, gold, diamond);

        DBManager.instance.OnLoadedUserData();

        LoginScene_UI loginSceneUI = GameObject.FindObjectOfType<LoginScene_UI>();
        if (!loginSceneUI)
        {
            Debug.LogError("Error LoginScene_UI 없음");
        }
        loginSceneUI.OnSuccessSignIn();
    }

}