using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class UserAchConditionStatePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserAchConditionStatePacket Unpack");
        //int startIndex = PacketInfo.FromServerPacketSettingIndex;

        //int rowCount = ByteConverter.ToInt(buffer, ref startIndex);
        //int gamePlay = ByteConverter.ToInt(buffer, ref startIndex);
        //int kill = ByteConverter.ToInt(buffer, ref startIndex);
        //int victoryCoin = ByteConverter.ToInt(buffer, ref startIndex);
        //int dead = ByteConverter.ToInt(buffer, ref startIndex);
        //int attackTry = ByteConverter.ToInt(buffer, ref startIndex);
        //int attackSuccess = ByteConverter.ToInt(buffer, ref startIndex);
        //int victoryCount = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log(rowCount);
        //Debug.Log("Cumulative Game Play count : " + gamePlay);
        //Debug.Log("Cumulative kill count : " + kill);
        //Debug.Log("Cumulative victory Coin : " + victoryCoin);
        //Debug.Log("Cumulative Dead count : " + dead);
        //Debug.Log("Cumulative Attack Try count : " + attackTry);
        //Debug.Log("Cumulative Attack Success count : " + attackSuccess);
        //Debug.Log("Cumulative Victory count : " + victoryCount);

        //DBManager.instance.userExtraData = new UserExtraData(gamePlay, kill, victoryCoin, dead,
        //    attackTry, attackSuccess, victoryCount);

        //DBManager.instance.OnLoadedUserACHConditionState();
    }
}