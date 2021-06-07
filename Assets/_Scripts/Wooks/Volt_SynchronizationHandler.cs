using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_SynchronizationHandler : MonoBehaviour
{
    public static Volt_SynchronizationHandler S;
    public bool isSyncDone = false;

    private void Awake()
    {
        S = this;
    }
   
    //public void SyncPostProcessing(int numOfPlayer, byte[] buffer, int startIndex, RobotData[] robotDatas,
    //    int roundNumber, int armageddonCount, int armageddonPlayer)
    //{
    //    Debug.Log("SysPostProcessing");
    //    StartCoroutine(CoSyncPostProcessing(numOfPlayer, buffer, startIndex, robotDatas, roundNumber, armageddonCount, armageddonPlayer));
    //}

    //public IEnumerator CoSyncPostProcessing(int numOfPlayer, byte[] buffer, int startIndex, RobotData[] robotDatas,
    //    int roundNumber, int armageddonCount, int armageddonPlayer)
    //{
    //    Debug.Log("SysPostProcessing");
    //    for (int index = 0; index < numOfPlayer; index++)
    //    {
    //        int number = ByteConverter.ToInt(buffer, ref startIndex);
    //        int numOfVP = ByteConverter.ToInt(buffer, ref startIndex);
    //        PlayerType type = (PlayerType)ByteConverter.ToInt(buffer, ref startIndex);
    //        bool isRobotAlive = ByteConverter.ToBool(buffer, ref startIndex);

    //        Debug.LogFormat("Number : {0}, NumOfVP : {1}, Type : {2}, isRobotAlive : {3}", number, numOfVP, (PlayerType)type, isRobotAlive);
    //        PlayerData playerData;
    //        playerData.playerNumber = number;
    //        playerData.numOfhasVP = numOfVP;
    //        playerData.playerType = type;
    //        playerData.isRobotAlive = isRobotAlive;
    //        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(number);
    //        player.Synchronization(playerData);

    //        if (playerData.isRobotAlive)
    //        {
    //            player.playerCamRoot.SetSaturationDown(false);
    //            if(player.GetRobot() == null)
    //            {
    //                player.playerRobot = Volt_PrefabFactory.S.CreateRobot(number, player.CharacterNumber).gameObject;
    //            }
    //            yield return StartCoroutine(player.GetRobot().Synchronization(robotDatas[index]));
    //        }
    //        else
    //        {
    //            if (player.GetRobot())
    //                player.GetRobot().ForcedKillRobot();
    //        }
    //        if (player.IsMobileActivated)
    //            player.isNeedSynchronization = false;

    //    }

    //    Volt_GMUI.S.Synchronization(roundNumber);

    //    if (Volt_GameManager.S.mapType == MapType.Ruhrgebiet)
    //    {
    //        Volt_GameManager.S.isOnSuddenDeath = true;
    //        //SuddenDeathOn 시켜야함.
    //        if (roundNumber >= 10 && roundNumber < 13)
    //        {
    //            foreach (var item in Volt_ArenaSetter.S.fallTiles1)
    //            {
    //                item.Fall();
    //            }
    //            Volt_PlayerManager.S.ChangeStartingTiles(1, Volt_ArenaSetter.S.GetTiles(11, 14));
    //            Volt_PlayerManager.S.ChangeStartingTiles(2, Volt_ArenaSetter.S.GetTiles(55, 28));
    //            Volt_PlayerManager.S.ChangeStartingTiles(3, Volt_ArenaSetter.S.GetTiles(69, 75));
    //            Volt_PlayerManager.S.ChangeStartingTiles(4, Volt_ArenaSetter.S.GetTiles(25, 52));
    //        }
    //        if (roundNumber >= 13)
    //        {
    //            foreach (var item in Volt_ArenaSetter.S.fallTiles2)
    //            {
    //                item.Fall();
    //            }

    //            Volt_PlayerManager.S.ChangeStartingTiles(1, Volt_ArenaSetter.S.GetTiles(20, 23));
    //            Volt_PlayerManager.S.ChangeStartingTiles(2, Volt_ArenaSetter.S.GetTiles(56, 29));
    //            Volt_PlayerManager.S.ChangeStartingTiles(3, Volt_ArenaSetter.S.GetTiles(60, 57));
    //            Volt_PlayerManager.S.ChangeStartingTiles(4, Volt_ArenaSetter.S.GetTiles(24, 51));
    //        }
    //    }

    //    yield return StartCoroutine(DelayedSync(armageddonCount, armageddonPlayer));

    //    
    //}

    public IEnumerator DelayedSync(int armageddonCount, int armageddonPlayer)
    {
        //Debug.Log("SysPostProcessing");
        isSyncDone = false;
        yield return new WaitForSeconds(1f);
        Volt_GameManager.S.AmargeddonCount = armageddonCount;
        //Debug.Log(Volt_GameManager.S.AmargeddonCount);
        if (armageddonCount != 0)
            Volt_GameManager.S.AmargeddonPlayer = armageddonPlayer;
        isSyncDone = true;
    }
}
