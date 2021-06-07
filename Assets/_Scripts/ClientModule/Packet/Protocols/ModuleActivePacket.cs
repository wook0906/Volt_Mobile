
using UnityEngine;

public class ModuleActivePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        int startIndex = PacketInfo.FromServerPacketDataStartIndex;

        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        int slotNumber = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log("Active  playerNumber : " + playerNumber + "  slotNumber : " + slotNumber);
        Volt_Robot robot = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber).playerRobot.GetComponent<Volt_Robot>();
        robot.moduleCardExcutor.SetOnActiveCard(robot.moduleCardExcutor.GetCurEquipCards()[slotNumber], slotNumber);
    }
}