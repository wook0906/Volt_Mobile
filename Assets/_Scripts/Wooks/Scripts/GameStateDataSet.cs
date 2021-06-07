using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileData
{
    public int tileIdx;
    public Volt_Tile.TileType tileType;
    public Card tileInModule; //현재 타일위에 배치된 모듈의 종류(없으면 Card.None)
    public int numOfVP;
    public bool isHaveRepairKit;
    public bool isHaveTimeBomb;
    public bool isOnVoltage;
    public int timeBombOwnerPlayerNumber;
    public int timeBombCount;
}
public struct RobotData
{
    public int tileIdx;
    public int ownerPlayerNumber;
    public Card slot1Module; 
    public int module1State; //현재 해당 모듈이 어떤 상태인지?
    public Card slot2Module;
    public int module2State;
    public int hitCount;
    public int lookDirectionLength; // Direction String을 읽을 길이
    public string lookDirection;
    public bool isHaveTimeBomb;
    public int timeBombCount;
    public int timeBombOwnerNumber;
    public int shieldPoint;
    public RobotType robotType;
    public SkinType skinType;
}
public struct PlayerData
{
    public int playerNumber;
    public int numOfhasVP;
    public PlayerType playerType;
    public bool isRobotAlive;
    public string nickName;
}
public class GameStateDataSet : MonoBehaviour
{
    public static void SendTotalTurnOverGameDatas()
    {
        //foreach (var item in Volt_PlayerManager.S.GetPlayers())
        //{
        //    if (item.VictoryPoint >= 3)
        //        return;
        //}
        int numOfNeedSyncTiles = Volt_ArenaSetter.S.needSyncTiles.Count;
        TileData[] newStateTileDatas = new TileData[numOfNeedSyncTiles];
        int cnt = 0;
        foreach (var tile in Volt_ArenaSetter.S.needSyncTiles)
        {
            newStateTileDatas[cnt].tileIdx = tile.tileIndex;
            newStateTileDatas[cnt].tileType = tile.pTileType;
            if (tile.tileInModuleType != Card.NONE)
                newStateTileDatas[cnt].tileInModule = tile.tileInModuleType;
            else
                newStateTileDatas[cnt].tileInModule = Card.NONE;
            newStateTileDatas[cnt].numOfVP = tile.numOfCoins;
            newStateTileDatas[cnt].isHaveRepairKit = tile.isHaveRepairKit;
            newStateTileDatas[cnt].isHaveTimeBomb = tile.IsHaveTimeBomb;
            newStateTileDatas[cnt].isOnVoltage = tile.IsOnVoltage;
            if (tile.TimeBombInstance == null)
            {
                newStateTileDatas[cnt].timeBombOwnerPlayerNumber = 0;
                newStateTileDatas[cnt].timeBombCount = 0;
            }
            else
            {
                newStateTileDatas[cnt].timeBombOwnerPlayerNumber = tile.TimeBombInstance.ownerPlayerNumber;
                newStateTileDatas[cnt].timeBombCount = tile.TimeBombInstance.count;
            }


            Debug.Log($"idx : {newStateTileDatas[cnt].tileIdx}, voltage : {newStateTileDatas[cnt].isOnVoltage}");
            //Debug.Log("tileIdx : " + newStateTileDatas[cnt].tileIdx +
            //    " tileType : " + newStateTileDatas[cnt].tileType.ToString() +
            //    " tileInModule : " + newStateTileDatas[cnt].tileInModule.ToString() +
            //    " numOfVP : " + newStateTileDatas[cnt].numOfVP +
            //    " isHaveRepairKit : " + newStateTileDatas[cnt].isHaveRepairKit +
            //    " isHaveTimeBomb : " + newStateTileDatas[cnt].isHaveTimeBomb +
            //    " timeBombOwner : " + newStateTileDatas[cnt].timeBombOwnerPlayerNumber + " timeBombCount : " + newStateTileDatas[cnt].timeBombCount);
            cnt++;
        }

        Volt_ArenaSetter.S.needSyncTiles.Clear();

        int numOfRobot = Volt_ArenaSetter.S.robotsInArena.Count;
        RobotData[] robotDatas = new RobotData[numOfRobot];
        cnt = 0;
     

        foreach (var robot in Volt_ArenaSetter.S.robotsInArena)
        {
            robotDatas[cnt].tileIdx = Volt_ArenaSetter.S.GetTile(robot.transform.position).tileIndex;
            robotDatas[cnt].ownerPlayerNumber = robot.playerInfo.playerNumber;
            Volt_ModuleCardBase[] cards = robot.moduleCardExcutor.GetCurEquipCards();
            if (cards[0] != null)
            {
                robotDatas[cnt].slot1Module = cards[0].card;
                robotDatas[cnt].module1State = cards[0].ActiveType;
            }
            else
            {
                robotDatas[cnt].slot1Module = Card.NONE;
                robotDatas[cnt].module1State = 0;
            }

            if (cards[1] != null)
            {
                robotDatas[cnt].slot2Module = cards[1].card;
                robotDatas[cnt].module2State = cards[1].ActiveType;
            }
            else
            {
                robotDatas[cnt].slot2Module = Card.NONE;
                robotDatas[cnt].module2State = 0;
            }
            robotDatas[cnt].hitCount = robot.HitCount;
            robotDatas[cnt].shieldPoint = robot.AddOnsMgr.ShieldPoints;
            robotDatas[cnt].lookDirectionLength = robot.transform.forward.ToString().Length;
            robotDatas[cnt].lookDirection = robot.transform.forward.ToString();
            robotDatas[cnt].isHaveTimeBomb = robot.IsTimeBombOn;
            if (robot.GetTimeBomb() == null)
            {
                robotDatas[cnt].timeBombCount = 0;
                robotDatas[cnt].timeBombOwnerNumber = 0;
            }
            else
            {
                robotDatas[cnt].timeBombCount = robot.GetTimeBomb().count;
                robotDatas[cnt].timeBombOwnerNumber = robot.GetTimeBomb().ownerPlayerNumber;
            }
            Debug.Log("robotTileIdx : " + robotDatas[cnt].tileIdx +
                " owner : " + robotDatas[cnt].ownerPlayerNumber +
                " slot1Module : " + robotDatas[cnt].slot1Module.ToString() +
                " slot1State : " + robotDatas[cnt].module1State +
                " slot2Module : " + robotDatas[cnt].slot2Module.ToString() +
                " slot2State : " + robotDatas[cnt].module2State +
                " hitCount : " + robotDatas[cnt].hitCount +
                " shieldPoint : " + robotDatas[cnt].shieldPoint +
                " lookDirection Length : " + robotDatas[cnt].lookDirectionLength +
                " lookDirection : " + robotDatas[cnt].lookDirection +
                " isHaveTimeBomb : " + robotDatas[cnt].isHaveTimeBomb +
                " timeBombCount : " + robotDatas[cnt].timeBombCount +
                " timeBombOwner : " + robotDatas[cnt].timeBombOwnerNumber);
            cnt++;
        }
        PlayerData[] playerDatas = new PlayerData[4];
        cnt = 0;
        foreach (var player in Volt_PlayerManager.S.GetPlayers())
        {
            playerDatas[cnt].playerNumber = player.playerNumber;
            if(Volt_GameManager.S.isEndlessGame)
                playerDatas[cnt].numOfhasVP = 0;
            else 
                playerDatas[cnt].numOfhasVP = player.VictoryPoint;
            playerDatas[cnt].playerType = player.PlayerType;
            playerDatas[cnt].isRobotAlive = player.isRobotAlive;
            cnt++;
        }

        PacketTransmission.SendTotalTurnOverPacket(newStateTileDatas, robotDatas, playerDatas);
    }


}
