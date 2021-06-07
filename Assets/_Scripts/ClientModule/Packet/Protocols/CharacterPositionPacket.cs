
using UnityEngine;

public class CharacterPositionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        int startIndex = PacketInfo.FromServerPacketDataStartIndex;

        int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
        int xPos = ByteConverter.ToInt(buffer, ref startIndex);
        int yPos = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log("CharacterPositionPacket");
        //Debug.Log("PlayerNumber : " + playerNumber);
        //Debug.Log("x : " + xPos);
        //Debug.Log("y : " + yPos);

        if (Volt_GameManager.S.useCustomPosition)
        {
            switch (playerNumber)
            {
                case 1:
                    Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, (int)Volt_GameManager.S.playerInitPoints[0].x, (int)Volt_GameManager.S.playerInitPoints[0].y);
                    return;
                case 2:
                    Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, (int)Volt_GameManager.S.playerInitPoints[1].x, (int)Volt_GameManager.S.playerInitPoints[1].y);
                    return;
                case 3:
                    Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, (int)Volt_GameManager.S.playerInitPoints[2].x, (int)Volt_GameManager.S.playerInitPoints[2].y);
                    return;
                case 4:
                    Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, (int)Volt_GameManager.S.playerInitPoints[3].x, (int)Volt_GameManager.S.playerInitPoints[3].y);
                    return;
                default:
                    Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, xPos, yPos);
                    return;

            }
        }
        else
            Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, xPos, yPos);
    }
}