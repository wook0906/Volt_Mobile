using System;
using UnityEngine;

public class SynchronizationElsePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int numOfPlayer = 4;
        int order = ByteConverter.ToInt(buffer, ref startIndex);

        int roundNumber = ByteConverter.ToInt(buffer, ref startIndex);
        int armageddonCount = ByteConverter.ToInt(buffer, ref startIndex);
        int armageddonPlayer = ByteConverter.ToInt(buffer, ref startIndex);
        int remainVpSetupCount = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log("roundNumber : " + roundNumber);
        //Debug.Log("armageddonCount : " + armageddonCount);
        //Debug.Log("armageddonPlayer : " + armageddonPlayer);
        //Debug.Log("remainVpSetupCount : " + remainVpSetupCount);

        //CommunicationWaitQueue.Instance.SetOrder(order);
        //Debug.Log("Order : " + order);

        if (!CommunicationInfo.IsMobileActive)
            CommunicationWaitQueue.Instance.SetOrder(order);

        //RobotData[] robotDatas = new RobotData[numOfPlayer];
        try
        {
            for (int index = 0; index < numOfPlayer; index++)
            {

                int tileIdx = ByteConverter.ToInt(buffer, ref startIndex);
                int playerNumber = ByteConverter.ToInt(buffer, ref startIndex);
                Card moduleslot1 = (Card)ByteConverter.ToInt(buffer, ref startIndex);
                int moduleState1 = ByteConverter.ToInt(buffer, ref startIndex);
                Card moduleslot2 = (Card)ByteConverter.ToInt(buffer, ref startIndex);
                int moduleState2 = ByteConverter.ToInt(buffer, ref startIndex);
                int hitCount = ByteConverter.ToInt(buffer, ref startIndex);
                int shieldPoint = ByteConverter.ToInt(buffer, ref startIndex);
                int lookDirectionLength = ByteConverter.ToInt(buffer, ref startIndex);
                string lookDirection = ByteConverter.ToString(buffer, ref startIndex, lookDirectionLength);
                bool isHaveTimeBomb = ByteConverter.ToBool(buffer, ref startIndex);
                int timeBombCount = ByteConverter.ToInt(buffer, ref startIndex);
                int TimeBombOwner = ByteConverter.ToInt(buffer, ref startIndex);
                RobotType robotType = (RobotType)ByteConverter.ToInt(buffer, ref startIndex);
                SkinType skinType = (SkinType)ByteConverter.ToInt(buffer, ref startIndex);

                //Debug.LogFormat("TileIdx : {10}, OwnerPlayerNumber : {0}, ModuleSlots1 : {1}, ModuleState1 : {2},  " +
                //    "ModuleSlots2 : {3}, ModuleState2 : {4}, " +
                //    "LookLength : {5}, LookDirect : {6}, " +
                //    "IshaveTimeBomb : {7}, TimeBombCount : {8}, TimeBombOwner : {9}, ShieldPoint : {11}, RobotType : {12}, SkinType : {13}",
                //    playerNumber, moduleslot1, moduleState1, moduleslot2, moduleState2, lookDirectionLength, lookDirection, isHaveTimeBomb, timeBombCount, TimeBombOwner, tileIdx, shieldPoint, robotType, skinType);

                RobotData robotData;

                robotData.tileIdx = tileIdx;
                robotData.ownerPlayerNumber = playerNumber;
                robotData.slot1Module = moduleslot1;
                robotData.module1State = moduleState1;
                robotData.slot2Module = moduleslot2;
                robotData.module2State = moduleState2;
                robotData.hitCount = hitCount;
                robotData.shieldPoint = shieldPoint;
                robotData.lookDirectionLength = lookDirectionLength;
                robotData.lookDirection = lookDirection;
                robotData.isHaveTimeBomb = isHaveTimeBomb;
                robotData.timeBombCount = timeBombCount;
                robotData.timeBombOwnerNumber = TimeBombOwner;
                robotData.robotType = robotType;
                robotData.skinType = skinType;

                Volt_GameManager.S.robotDataForSync.Enqueue(robotData);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }

        for (int index = 0; index < numOfPlayer; index++)
        {
            int number = ByteConverter.ToInt(buffer, ref startIndex);
            int numOfVP = ByteConverter.ToInt(buffer, ref startIndex);
            PlayerType type = (PlayerType)ByteConverter.ToInt(buffer, ref startIndex);
            bool isRobotAlive = ByteConverter.ToBool(buffer, ref startIndex);
            int nickNameLength = ByteConverter.ToInt(buffer, ref startIndex);
            string nickName = ByteConverter.ToString(buffer, ref startIndex, nickNameLength);

            //Debug.LogFormat("Number : {0}, NumOfVP : {1}, Type : {2}, isRobotAlive : {3}, nickNameLength : {4}, nickName : {5}", number, numOfVP, (PlayerType)type, isRobotAlive,nickNameLength, nickName);
            PlayerData playerData;
            playerData.playerNumber = number;
            playerData.numOfhasVP = numOfVP;
            playerData.playerType = type;
            playerData.isRobotAlive = isRobotAlive;
            playerData.nickName = nickName;

            Volt_GameManager.S.playerDataForSync.Enqueue(playerData);
            //Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(number);
        }
        Volt_GameManager.S.ElseDataSynchronizationStart(roundNumber, armageddonCount, armageddonPlayer, remainVpSetupCount);

    }
}