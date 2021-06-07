using UnityEngine;

public class PlayerExitPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        int startIndex = PacketInfo.FromServerPacketSettingIndex;
        int exitingPlayerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        bool isSocketError = ByteConverter.ToBool(buffer, ref startIndex); //���ͳ� �Ҿ������� �������?

        //Debug.Log("exitingPlayerNumber : " + exitingPlayerNumber);
        //Debug.Log("isSocketError? : " + isSocketError);

        Volt_PlayerManager.S.ReceiveFocusMSG(exitingPlayerNumber, false);
        Volt_PlayerManager.S.PlayerExit(exitingPlayerNumber,isSocketError);
        
    }
}