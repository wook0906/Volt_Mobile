using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_PlayerManager : MonoBehaviour
{
    public static Volt_PlayerManager S;
    //private Volt_PlayerInfo[] curPlayerList = new Volt_PlayerInfo[4];
    [SerializeField]
    private List<Volt_PlayerInfo> curPlayerList;
   
    private int activatedPlayersCnt = 0;
    public int ActivatedPlayersCnt
    {
        get { return activatedPlayersCnt; }
        set
        {
            activatedPlayersCnt = value;
            //print("activated Player Count : " + activatedPlayersCnt);
        }
    }
    public int myPlayerNumber;
    public Volt_PlayerInfo I;
    public int[] playerKey = { 1, 1, 1, 1 };
    // Start is called before the first frame update
    private void Awake()
    {
        S = this;
        curPlayerList = new List<Volt_PlayerInfo>();
    }
    void Start()
    {
        FindObjectOfType<Volt_PlayerUI>().Init();
    }

    public bool isCanGetPlayerKey(int playerNumber)
    {
        if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
        {
            return true;
        }
        else
        {
            if (playerKey[playerNumber - 1] == 1)
            {
                
                return true;
            }
            else
            {
                //Debug.Log(playerNumber + "의 키가 없습니다.");
                return false;
            }
        }
    }
    public void TakePlayerKey(int playerNumber)
    {
        //Debug.Log(playerNumber + "의 Key를 가져갑니다~ 유후~");
        playerKey[playerNumber - 1] = 0;
    }
    public void ReturnPlayerKey(int playerNumber)
    {
        //Debug.Log(playerNumber + "의 Key를 반납합니다~ 유유유");
        playerKey[playerNumber - 1] = 1;
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        
    }
    public List<Volt_PlayerInfo> GetPlayers()
    {
        return curPlayerList;
    }
    public List<Volt_PlayerInfo> GetActivatedPlayers()
    {
        List<Volt_PlayerInfo> activatedPlayers = new List<Volt_PlayerInfo>();
        foreach (var item in curPlayerList)
        {
            if (item.IsMobileActivated)
                activatedPlayers.Add(item);
        }
        return activatedPlayers;
    }
    public void GetMyPlayerNumberFromServer(int playerNumber) //매칭시작패킷에 대한 회신으로 내 플레이어 넘버를 받는다.
    {
        myPlayerNumber = playerNumber;
        //print("나는 " + myPlayerNumber + "번");
    }
    public void SetupPlayersInfo(PlayerType playerType1, RobotType char1, SkinType skin1, string nickName1,
                                PlayerType playerType2, RobotType char2, SkinType skin2, string nickName2,
                                PlayerType playerType3, RobotType char3, SkinType skin3, string nickName3,
                                PlayerType playerType4, RobotType char4, SkinType skin4, string nickName4) //매칭완료시 호출. 플레이어들의 정보를 업데이트한다.
    {

        //print("SetupPlayersInfo");
        
        CreatePlayer(playerType1, 1, char1, skin1, nickName1);
        if (playerType1 != PlayerType.AI)
            ActivatedPlayersCnt++;
            
        CreatePlayer(playerType2, 2, char2, skin2, nickName2);
        if (playerType2 != PlayerType.AI)
            ActivatedPlayersCnt++;
        
        CreatePlayer(playerType3, 3, char3, skin3, nickName3);
        if (playerType3 != PlayerType.AI)
            ActivatedPlayersCnt++;

        CreatePlayer(playerType4, 4, char4, skin4, nickName4);
        if (playerType4 != PlayerType.AI)
            ActivatedPlayersCnt++;
    }
    void CreatePlayer(PlayerType newPlayerType,int playerNumber, RobotType robotType, SkinType skinType, string nickName)
    {
        Managers.Resource.InstantiateAsync("Game/Contents/Player.prefab").Completed += (result) =>
        {
            Debug.Log($"Done Create player[{playerNumber}]");
            Volt_PlayerInfo newPlayer = result.Result.GetComponent<Volt_PlayerInfo>();
            curPlayerList.Add(newPlayer);
            newPlayer.playerNumber = playerNumber;
            newPlayer.RobotType = robotType;

            if (myPlayerNumber == playerNumber)
            {
                I = newPlayer;
                Volt_PlayerUI.S.owner = I;
                I.playerPanel = Volt_GMUI.S.playerPanels[0];
                //print("I : " + playerNumber);
            }
            else
            {
                for (int i = Volt_GMUI.S.playerPanels.Length - 1; i > 0; i--)
                {
                    if (Volt_GMUI.S.playerPanels[i].ownerPlayer) continue;
                    newPlayer.playerPanel = Volt_GMUI.S.playerPanels[i];
                }
            }
            newPlayer.NickName = nickName;
            newPlayer.skinType = skinType;
            newPlayer.playerPanel.Init(newPlayer);
            newPlayer.PlayerType = newPlayerType;
        };
        //print("Create :" + playerNumber);
    }

    public void RuhrgebietChangeStartingTiles(int playerNumber, int phase)
    {
        Volt_PlayerInfo player = GetPlayerByPlayerNumber(playerNumber);
        player.startingTiles.Clear();
        switch (playerNumber)
        {
            case 1:
                if (phase == 1)
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(12));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(13));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(14));
                }
                else
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(21));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(22));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(23));
                }
                break;
            case 2:
                if (phase == 1)
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(46));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(37));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(28));
                }
                else
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(47));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(38));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(29));
                }
                break;
            case 3:
                if (phase == 1)
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(68));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(67));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(66));
                }
                else
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(59));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(58));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(57));
                }
                break;
            case 4:
                if (phase == 1)
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(34));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(43));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(52));
                }
                else
                {
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(33));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(42));
                    player.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(51));
                }
                break;
            default:
                break;
        }

    }
    public Volt_PlayerInfo GetPlayerByPlayerNumber(int playerNumber)
    {
        foreach (var player in curPlayerList)
        {
            if (player.playerNumber == playerNumber)
            {
                return player;
            }
        }
        //print("Get Player Err Get Number : " + playerNumber);
        return null;
    }

    private void OnApplicationPause(bool pause)
    {
        if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode) return;
        if (!pause)
        {
            if (I != null)
            {
                //Debug.Log("OnApplication Pause Exit Start1");
                
                PacketTransmission.SendMobileActivePacket(I.playerNumber);
                Volt_GameManager.S.behaviourStack.Clear();
                Volt_GameManager.S.tmpBehaviours.Clear();
                Volt_GameManager.S.StopAllCoroutines();
                //ReturnPlayerKey(I.playerNumber);
                I.playerCamRoot.CamInit();
                I.playerCamRoot.SaveLastInfo();
                Volt_GMUI.S.guidePanel.ShowGuideTextMSG(GuideMSGType.SynchronizationWait, false, Application.systemLanguage);
                
                Volt_GMUI.S.syncWaitblockPanel.SetActive(true);
                //Debug.Log("OnApplication Pause Exit End1");
            }
        }
        else
        {
            if (I != null)
            {
                foreach (var item in FindObjectsOfType<Volt_PitMonster>())
                {
                    item.ForcedCancel();
                }
                Volt_GMUI.S.IsTickOn = false;
                Volt_GMUI.S.TickTimer = 99;
                //Debug.Log("OnApplication Pause Enter Start2");
                foreach (var item in Volt_ArenaSetter.S.GetTileArray())
                {
                    if (item.ModuleInstance)
                        item.DestroyModule();
                }

                switch (Volt_GameManager.S.pCurPhase)
                {
                    case Phase.robotSetup:
                        foreach (var item in I.startingTiles)
                        {
                            item.responseParticle.Stop();
                            item.responseParticle.gameObject.SetActive(false);
                        }
                        break;
                    case Phase.behavoiurSelect:
                        Volt_PlayerUI.S.BehaviourSelectOff();
                        Volt_PlayerUI.S.ShowModuleButton(false);
                        SendBehaviorOrderPacket(I.playerNumber, new Volt_RobotBehavior());
                        break;
                    case Phase.rangeSelect:
                        SendBehaviorOrderPacket(I.playerNumber, new Volt_RobotBehavior());
                        I.playerCamRoot.CamInit();
                        I.playerCamRoot.SaveLastInfo();
                        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
                        {
                            item.SetDefaultBlinkOption();
                            item.BlinkOn = false;
                        }
                        break;
                    
                    case Phase.synchronization:
                        ModuleType moduleType = Volt_ModuleDeck.S.GetRandomModuleType();
                        Volt_ModuleCardBase cardBase = Volt_ModuleDeck.S.DrawRandomCard(moduleType);
                        PacketTransmission.SendFieldReadyCompletionPacket(cardBase.card);
                        break;
                    case Phase.gameOver:
                        Volt_DontDestroyPanel.S.OnDisconnected();
                        break;
                    default:
                        //Debug.Log("Do Nothing UnActive");
                        break;
                }
                
                Volt_GameManager.S.screenBlockPanel.SetActive(true);
                I.isNeedSynchronization = true;
                PacketTransmission.SendMobileUnActivePacket(I.playerNumber);
            }
            Volt_GameManager.S.StopAllCoroutines();
            Volt_GameManager.S.pCurPhase = Phase.waitSync;
            //껐을떄
            //Debug.Log("OnApplication Pause Enter End3");

        }
    }
   
    public void ChangeHostPlayerUnActive(int playerNumber)
    {
        Volt_PlayerInfo player = GetPlayerByPlayerNumber(playerNumber);
        if (player.PlayerType == PlayerType.HOSTPLAYER)
        {
            foreach (var item in curPlayerList)
            {
                ReturnPlayerKey(playerNumber);
                if (item == player)
                {
                    continue;
                }
                if (item.PlayerType == PlayerType.PLAYER)
                {
                    player.PlayerType = PlayerType.PLAYER;
                    item.ChangeToHostPlayer();
                    //print("unactive change");
                    return;
                }
            }
        }
        else
            return;
    }
    public void SendCharacterPositionPacket(int playerNumber, int x, int y)
    {
        if (Volt_GameManager.S.pCurPhase == Phase.robotSetup || Volt_GameManager.S.pCurPhase == Phase.synchronization)
        {
            //Debug.Log("Send " + playerNumber + " Character Position");
            PacketTransmission.SendCharacterPositionPacket(playerNumber, x, y);
        }
    }
    public void SendBehaviorOrderPacket(int playerNumber,Volt_RobotBehavior behaviour)
    {
        if (Volt_GameManager.S.pCurPhase == Phase.behavoiurSelect || Volt_GameManager.S.pCurPhase == Phase.rangeSelect)
        {
            //Debug.Log("Send " + playerNumber + " BehaviourOrder");
            PacketTransmission.SendBehaviorOrderPacket(playerNumber, behaviour);
        }
    }
    public int GetCurrentNumberOfActive()
    {
        int cnt = 0;
        foreach (var item in GetPlayers())
        {
            //Debug.Log(item.playerNumber + " : IsMoblileActivated " + item.IsMobileActivated + " isNeedSync : " + item.isNeedSynchronization);
            if (item.IsMobileActivated && !item.isNeedSynchronization)
                cnt++;
        }
        return cnt;
    }
    
    public void ReceiveFocusMSG(int playerNumber,bool isActivated)
    {
        Volt_PlayerInfo player = GetPlayerByPlayerNumber(playerNumber);
        if (player.IsMobileActivated == isActivated) return;

        player.IsMobileActivated = isActivated;

        if (isActivated)
        {
            ActivatedPlayersCnt++;
        }
        else
        {
            ActivatedPlayersCnt--;
            player.isNeedSynchronization = true;
            ReturnPlayerKey(playerNumber);
        }

    }
    public void ReturnToPlayer(int playerNumber)
    {
        Volt_PlayerInfo player = GetPlayerByPlayerNumber(playerNumber);
        
        player.IsMobileActivated = true;
        player.isNeedSynchronization = true;
        player.PlayerType = PlayerType.PLAYER;
    }
    public void PlayerExit(int playerNumber, bool isSocketError)
    {
        Volt_PlayerInfo player = GetPlayerByPlayerNumber(playerNumber);
        ChangeHostPlayerUnActive(playerNumber);
        ReturnPlayerKey(playerNumber);
        if (player.IsMobileActivated)
        {
            player.IsMobileActivated = false;
            player.PlayerExit(isSocketError);
        }
        else
        {
            //Debug.Log("이미 Activate 상태가 아니었기 때문에 AI로 세팅할 것임.");
            player.PlayerExit(!isSocketError);
        }
        //exitPlayerCnt++;
        
        
        //CommunicationWaitQueue.Instance.ResetOrder();
        //CommunicationInfo.IsBoardGamePlaying = false;

    }
    
    
}
