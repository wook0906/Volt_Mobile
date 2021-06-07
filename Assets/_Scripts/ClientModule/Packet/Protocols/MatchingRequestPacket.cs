using UnityEngine;

public class MatchingRequestPacket : Packet
{    
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("MatchingRequestPacket UnPack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        // 내가 몇 번째 플레이어인지 설정
        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        Volt_PlayerManager.S.GetMyPlayerNumberFromServer(playerNumber);
    }
}
