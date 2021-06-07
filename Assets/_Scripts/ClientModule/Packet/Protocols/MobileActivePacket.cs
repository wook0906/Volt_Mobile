
public class MobileActivePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        int startIndex = PacketInfo.FromServerPacketDataStartIndex;
        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);

        Volt_PlayerManager.S.ReceiveFocusMSG(playerNumber,true);
    }
}