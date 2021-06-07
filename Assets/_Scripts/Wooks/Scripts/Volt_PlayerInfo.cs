using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    NONE, AI,PLAYER, HOSTPLAYER
}
public class Volt_PlayerInfo : MonoBehaviour
{
    [Header("Set in Inspector")]
    //public PlayerDirection playerDirection;
    public List<Volt_Tile> startingTiles;
    
    

    [Header("Set in Scripts")]
    public Camera playerCam;
    public CameraMovement playerCamRoot;
    public Volt_ScreenRaycaster raycaster;
    public string nickName;
    public string NickName
    {
        set { nickName = value; }
        get { return nickName; }
    }

    public int playerNumber;
    
    public Volt_PlayerPanel playerPanel;
    [SerializeField]
    private GameObject _playerRobot;
    public GameObject playerRobot
    {
        get
        {
            return _playerRobot;
        }
        set
        {
            _playerRobot = value;
                
        }
    }
    public bool isRobotAlive;

    private int vp = 0;
    public int VictoryPoint
    {
        get { return vp; }
        set
        {
            if (value >= 0)
            {
                vp = value;
                playerPanel.RenewPoint(vp);
                if (vp >= 3)
                {
                    Volt_GameManager.S.SendAchievementProgressPacketBeforeGameOver(playerNumber);
                }
            }
        }
    }
    [SerializeField]
    private PlayerType playerType;
    public PlayerType PlayerType
    {
        get { return playerType; }
        set
        {
            playerType = value;
            playerPanel.SetPicture(playerNumber, playerType);
        }
    }

    [SerializeField]
    private bool isMobileActivated = true;
    public bool IsMobileActivated
    {
        get { return isMobileActivated; }
        set { isMobileActivated = value; }
    }
    public bool isNeedSynchronization;
    [HideInInspector]
    private RobotType robotType;
    public RobotType RobotType
    {
        get { return robotType; }
        set
        {
            robotType = value;
            //Debug.Log("robotType Number : " + robotType);
            if (Volt_PlayerManager.S.I)
            {
                Volt_GamePlayData.S.RobotType = Volt_PlayerManager.S.I.RobotType;
                //Debug.Log("My robotType Number : " + robotType);
            }
        }
    }
    public SkinType skinType;
    
    private void Awake()
    {
       
    }
    public Volt_Robot GetRobot()
    {
        if (playerRobot)
        {
            return playerRobot.GetComponent<Volt_Robot>();
        }
        return null;
    }
    /*
    // Start is called before the first frame update
    public void Init()
    {
        if (!isI)
        {
            this.GetComponent<Volt_ScreenRaycaster>().enabled = false;
            this.GetComponentInChildren<Camera>().enabled = false;
            this.GetComponentInChildren<CameraMovement>().enabled = false;
        }
        else
        {
            this.GetComponent<Volt_ScreenRaycaster>().enabled = true;
            this.GetComponentInChildren<Camera>().enabled = true;
            this.GetComponentInChildren<CameraMovement>().enabled = true;
        }
        
    }
    */
    public void Synchronization(PlayerData data)
    {
        VictoryPoint = data.numOfhasVP;
        PlayerType = data.playerType;
    }
    //public void ForcedKillRobot()
    //{
    //    if (playerRobot)
    //    {
    //        Debug.Log(playerNumber + " player robot Forced Destroy");
    //        Volt_Robot robot = playerRobot.GetComponent<Volt_Robot>();
          
    //        robot.moduleCardExcutor.DestroyCardAll();
    //        Volt_ArenaSetter.S.GetTile(playerRobot.transform.position).SetRobotInTile(null);
    //        Volt_ArenaSetter.S.robotsInArena.Remove(robot);
    //        Destroy(playerRobot);
    //    }
       
    //    playerRobot = null;
    //}

    public void AutoRobotPlace()
    {
        Volt_GameManager.S.AutoRobotSetup(playerNumber);
        //Volt_Tile placeTile = Volt_ArenaSetter.S.GetRandomTileToPlace(startingTiles);//[(int)startingTiles.Count / 2];
        //if (placeTile.GetRobotInTile() == null)
        //{
        //    if (Volt_GameManager.S.isTrainingMode)
        //        Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, placeTile.tilePosInArray.x, placeTile.tilePosInArray.y);
        //    else
        //        Volt_PlayerManager.S.SendCharacterPositionPacket(playerNumber, placeTile.tilePosInArray.x, placeTile.tilePosInArray.y);
        //}
        //else
        //{
        //    foreach (var item in startingTiles)
        //    {
        //        if (item.GetRobotInTile() == null)
        //        {
        //            if (Volt_GameManager.S.isTrainingMode)
        //                Volt_GameManager.S.PlayerRobotPlaceRequest(playerNumber, placeTile.tilePosInArray.x, placeTile.tilePosInArray.y);
        //            else
        //                Volt_PlayerManager.S.SendCharacterPositionPacket(playerNumber, placeTile.tilePosInArray.x, placeTile.tilePosInArray.y);
        //            return;
        //        }
        //    }
        //}
    }
   
    public void PlayerExit(bool isSocketError)
    {
        //if (isSocketError)
        //{
            PlayerType = PlayerType.AI;
            //StartCoroutine(RobotChangeToAI());
            //Debug.Log("소켓 에러네");
            return;
        //}
        //Debug.Log("소켓에러 아니네");
    }
    //IEnumerator RobotChangeToAI()
    //{
    //    yield return new WaitUntil(() => Volt_GameManager.S.pCurPhase != Phase.simulation);
    //    Volt_Tile tile = Volt_ArenaSetter.S.GetTile(playerRobot.transform.position);
    //    Volt_ArenaSetter.S.robotsInArena.Remove(playerRobot.GetComponent<Volt_Robot>());
    //    RobotBehaviourObserver.Instance.OffBehaviorFlag(playerNumber);
    //    Destroy(playerRobot);
    //    playerRobot = null;
    //    Volt_PlayerManager.S.SendCharacterPositionPacket(playerNumber, tile.tilePosInArray.x, tile.tilePosInArray.y);

    //}
    public void ChangeToHostPlayer()
    {
        PlayerType = PlayerType.HOSTPLAYER;
        
        
        switch (Volt_GameManager.S.pCurPhase)
        {
            case Phase.robotSetup:
                break;
            case Phase.behavoiurSelect:
                break;
            case Phase.rangeSelect:
                if (PlayerType == PlayerType.HOSTPLAYER)
                    Volt_GameManager.S.DoAllKillbotsDetection();
                break;
            default:
                break;
        }
    }
    public Define.Effects GetHitEffect()
    {
        switch (RobotType)
        {
            case RobotType.Volt:
                return Define.Effects.VFX_VoltHit;
            case RobotType.Mercury:
                return Define.Effects.VFX_MercuryHit;
            case RobotType.Hound:
                return Define.Effects.VFX_HoundHit;
            case RobotType.Reaper:
                return Define.Effects.VFX_ReaperHit;
            default:
                break;
        }
        return Define.Effects.None;
    }

    public void OnKillRobot(Volt_Robot other)
    {
        //DB 적 처치수 상승
        if (Volt_PlayerManager.S.I == this)
        {
            Volt_GamePlayData.S.Kill++;
            Volt_GMUI.S.Create2DMsg(MSG2DEventType.Kill, other.playerInfo.playerNumber);
        }

        if (other.AddOnsMgr.IsDummyGearOn)
        {
            Volt_GMUI.S.Create3DMsg(MSG3DEventType.NoPoint, other.playerInfo);
            return;
        }

        if(other.playerInfo.playerType == PlayerType.AI)
        {
            if (Volt_GameManager.S.IsTutorialMode)
            {
                VictoryPoint++;
                Volt_GMUI.S.Create3DMsg(MSG3DEventType.PointUp, this);
            }
            else
            {
                if (other.playerInfo.VictoryPoint > 0)
                {
                    VictoryPoint++;
                    //Debug.Log($"{NickName}이 {other.playerInfo.NickName}을 죽이고 점수 획득");
                    other.playerInfo.VictoryPoint--;
                    PacketTransmission.SendVictoryPointPacket(other.playerInfo.playerNumber, other.playerInfo.VictoryPoint);
                    //Debug.Log($"{other.playerInfo.NickName}이 {NickName}에게 점수 뺏김");
                    PacketTransmission.SendKillPacket(Volt_GMUI.S.RoundNumber, playerNumber, other.playerInfo.playerNumber); //DB
                    Volt_GMUI.S.Create3DMsg(MSG3DEventType.PointUp, this);
                }
                else
                {
                    Volt_GMUI.S.Create3DMsg(MSG3DEventType.NoPoint, this);
                }
            }
        }
        else
        {
            VictoryPoint++;
            Debug.Log($"{NickName}이 {other.playerInfo.NickName}을 죽이고 점수 획득");
            PacketTransmission.SendKillPacket(Volt_GMUI.S.RoundNumber, playerNumber, other.playerInfo.playerNumber); //DB
            Volt_GMUI.S.Create3DMsg(MSG3DEventType.PointUp, this);
        }
    }

    public bool CompareTo(Volt_PlayerInfo info)
    {
        return this.playerNumber == info.playerNumber;
    }
}