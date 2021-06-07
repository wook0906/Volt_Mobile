using UnityEngine;
using UnityEditor;

public class UseEmoticonPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UseEmoticonPacket Unpack");
        int startIndex = PacketInfo.FromServerPacketDataStartIndex;

        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        int id = ByteConverter.ToInt(buffer, ref startIndex);

        //플레이어 번호와 사용한 이모티콘의ID값이 날아옵니다.
        //**클라이언트에서 Send한 값을 그대로 4명이 나눠받습니다.

        Debug.Log($"EmoticonUsePlayer : {playerNumber}");
        Debug.Log($"Emoticon ID : {id}");

        Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber).GetRobot().panel.EmoticonPlay((Define.EmoticonType)id);

    }

}