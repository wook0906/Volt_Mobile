using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
//using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
public enum Phase
{
    none, matching, fieldSetup, playerSetup, ItemSetup, robotSetup, behavoiurSelect, rangeSelect, simulation, resolution, synchronization, gameOver, waitSync, suddenDeath,exitGame
}
public enum MapType
{
    TwinCity, Rome, Ruhrgebiet, Tokyo, Tutorial
}
public class Volt_GameManager : MonoBehaviour
{ 
    public static Volt_GameManager S;
    public PostProcessVolume postProcessVolume;
    Vignette vignette;
    public bool pause = false;
    public bool isSingleGame = false;
    public bool IsTrainingMode { private set; get; }
    private bool _isOnSuddenDeath = false;
    public bool isOnSuddenDeath { get { return _isOnSuddenDeath; }
        set
        {
            Debug.Log($"Set isOnSuddenDeath{value}");
            _isOnSuddenDeath = value;
        }
    }

    private bool _isEndlessGame = false;
    public bool isEndlessGame
    {
        get { return _isEndlessGame; }
        set { _isEndlessGame = value; }
    }

    public bool _isKillbotBehaviourOff = false;
    public bool isKillbotBehaviourOff { get { return _isKillbotBehaviourOff; }
        set { _isKillbotBehaviourOff = value; }
    }

    public int RoundNumber { get; set; }
    //패킷 오더 넘버... 패킷보낼때마다 증가
    public GameObject playerPrefab;
    public UILabel noticeText;
    
    public GameObject screenBlockPanel;

    public Volt_RobotBehavior behaviour;
    public List<Volt_Robot> dodgeRobots = new List<Volt_Robot>();
    public Stack<Volt_RobotBehavior> behaviourStack;
    public List<Volt_RobotBehavior> tmpBehaviours;
    public GameObject amargeddonPrefab;
    public GameObject gameOverPanel;

    public GameObject[] ballistaLaunchPoints;

    private bool isModuleSetupDone = false;
    private bool isVpSetupDone = false;
    private bool isRepairkitSetupDone = false;
    private bool isVoltageSpaceSetupDone = false;
    private bool isBehaviourSelectDone = false;
    private bool isRangeSelectDone = false;
    private bool isTileDataSyncDone = false;
    private bool isElseDataSyncDone = false;

    private bool _isRangeSelectCameraMoving = false;
    public bool isRangeSelectCameraMoving
    {
        get { return _isRangeSelectCameraMoving; }
        set { _isRangeSelectCameraMoving = value; }
    }
    private bool _isSynchronizationDone = true;
    public bool isSynchronizationDone
    {
        get { return _isSynchronizationDone; }
        set { _isSynchronizationDone = value; }
    }
    public bool IsTutorialMode { private set; get; }

    public int optionIdx = 0;

    Queue<RobotPlaceData> waitRobotPlaceQueue;

    public Queue<TileData> tileDataForSync;
    public Queue<RobotData> robotDataForSync;
    public Queue<PlayerData> playerDataForSync;

    [SerializeField]
    Phase curPhase;
    public Phase pCurPhase
    {
        get { return curPhase; }
        set { curPhase = value; }
    }
    public MapType mapType;

    //public UILabel amargeddonCountLabel;
    [SerializeField]
    private int amargeddonCount;
    public int AmargeddonCount
    {
        get { return amargeddonCount; }
        set
        {
            if (value <= 8 && value >= 0)
            {
                amargeddonCount = value;
                //amargeddonCountLabel.text = amargeddonCount.ToString();
            }
        }
    }
    [SerializeField]
    private bool isPlayAmargeddon = false;

    [SerializeField]
    private int amargeddonPlayer;
    public int AmargeddonPlayer { get { return amargeddonPlayer; } set { amargeddonPlayer = value; } }
    // Start is called before the first frame update

    public bool useCustomPosition = false;
    public Vector2[] playerInitPoints;


    public int randomBallistaLaunchPoint;

    [HideInInspector]
    AudioSource audio;

    public bool useCameraEffect = true;

    bool isPlayerRobotPlaceRunning = false;

    public int remainRoundCountToVpSetup = 5;

    public bool isGameOverWaiting = false;
    public bool isLocalSimulationDone = false;


    TileData[] SyncTileDatas = new TileData[81];
    private GameObject mapGO;

    //private GameObject sateliteBeam;
    private void Awake()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        S = this;
        audio = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("Volt_TutorialDone") == 0)
            IsTutorialMode = true;
        else
            IsTutorialMode = false;

        if (PlayerPrefs.GetInt("Volt_TrainingMode") == 1)
            IsTrainingMode = true;
        else
            IsTrainingMode = false;

        
    }
    private void FixedUpdate()
    {
        if(pCurPhase == Phase.waitSync)
        {
            StopAllCoroutines();
        }
    }
    public void Init()
    {
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        noticeText = GameObject.Find("GMUI/TopUIs/Notice/Notice").GetComponent<UILabel>();
        noticeText.transform.parent.gameObject.SetActive(false);
        screenBlockPanel = GameObject.Find("GMUI/ReconnectWaitBlock");
        screenBlockPanel.SetActive(false);

        waitRobotPlaceQueue = new Queue<RobotPlaceData>();
        tileDataForSync = new Queue<TileData>();
        robotDataForSync = new Queue<RobotData>();
        playerDataForSync = new Queue<PlayerData>();
        behaviourStack = new Stack<Volt_RobotBehavior>();
        tmpBehaviours = new List<Volt_RobotBehavior>();
        behaviour = new Volt_RobotBehavior();
        dodgeRobots.Clear();

        Volt_GamePlayData.S.OnPlayGame(IsTutorialMode || IsTrainingMode);

        if (postProcessVolume.profile.TryGetSettings(out vignette))
        {
            vignette.active = false;
        }
        if (!IsTutorialMode)
            Destroy(Volt_TutorialManager.S);

        if (!IsTrainingMode)
        {
            Volt_GMUI.S.optionPanel.GetComponent<Volt_GameOptionPanel>().cheatActiveToggle.SetActive(false);
        }
        Volt_PlayerUI.S.ShowModuleButton(false);
        
        //StartMatching();
    }
    
    public Volt_RobotBehavior PopBehaviour()
    {
        return behaviourStack.Pop();
    }

    public void StartMatching()
    {
        //DB 플레이 횟수 증가

        //curPhase = Phase.matching;
        //noticeText.text = "Match waiting...";
        curPhase = Phase.matching;
        RobotType selectedRobotType = (RobotType)PlayerPrefs.GetInt("SELECTED_ROBOT");
        RobotType randomKillbotType2 = (RobotType)Random.Range(0, 4);
        RobotType randomKillbotType3 = (RobotType)Random.Range(0, 4);
        RobotType randomKillbotType4 = (RobotType)Random.Range(0, 4);

        if (IsTutorialMode)
        {
            Volt_PlayerManager.S.GetMyPlayerNumberFromServer(1);
            Volt_PlayerManager.S.SetupPlayersInfo(PlayerType.HOSTPLAYER, selectedRobotType, Volt_PlayerData.instance.selectdRobotSkins[selectedRobotType].SkinType, Volt_PlayerData.instance.NickName,
                                                PlayerType.AI, randomKillbotType2, (SkinType)Random.Range(0, 7), Volt_Utils.GetRandomKillbotName(randomKillbotType2),
                                                PlayerType.AI, randomKillbotType3, (SkinType)Random.Range(0, 7), Volt_Utils.GetRandomKillbotName(randomKillbotType3),
                                                PlayerType.AI, randomKillbotType4, (SkinType)Random.Range(0, 7), Volt_Utils.GetRandomKillbotName(randomKillbotType4));
            ArenaSetupStart(0);
        }
        else if (IsTrainingMode)
        {
            Volt_PlayerManager.S.GetMyPlayerNumberFromServer(1);
            Volt_PlayerManager.S.SetupPlayersInfo(PlayerType.HOSTPLAYER, selectedRobotType, Volt_PlayerData.instance.selectdRobotSkins[selectedRobotType].SkinType, Volt_PlayerData.instance.NickName,
                                                PlayerType.AI, randomKillbotType2, (SkinType)Random.Range(0, 7), Volt_Utils.GetRandomKillbotName(randomKillbotType2),
                                                PlayerType.AI, randomKillbotType3, (SkinType)Random.Range(0, 7), Volt_Utils.GetRandomKillbotName(randomKillbotType3),
                                                PlayerType.AI, randomKillbotType4, (SkinType)Random.Range(0, 7), Volt_Utils.GetRandomKillbotName(randomKillbotType4));
            ArenaSetupStart(PlayerPrefs.GetInt("SELECTED_MAP"));
        }

        PacketTransmission.SendMatchingRequestPacket(PlayerPrefs.GetInt("SELECTED_ROBOT"), (int)Volt_PlayerData.instance.selectdRobotSkins[selectedRobotType].SkinType);
    }
    
    public void ArenaSetupStart(int mapType) //매칭 요청과 동시에 맵 세팅을 시작한다. 
    {
        Volt_ModuleDeck.S.SetModuleDeckSettingData((MapType)mapType);
        StartCoroutine(ArenaSetup(mapType));
    }

    IEnumerator ArenaSetup(int mapType)
    {
        curPhase = Phase.fieldSetup;

        yield return new WaitUntil(() => Volt_PlayerManager.S.I != null);
        
        string sceneName = "";
        if (IsTutorialMode)
        {
            sceneName = "TwinCity";
            this.mapType = MapType.TwinCity;
            if (Volt_SoundManager.S != null)
                Volt_SoundManager.S.ChangeBGM(this.mapType);
        }
        else
        {
            switch (mapType)
            {
                case 0:
                    sceneName = "TwinCity";
                    this.mapType = MapType.TwinCity;
                    break;
                case 1:
                    sceneName = "Rome";
                    this.mapType = MapType.Rome;
                    break;
                case 2:
                    sceneName = "Ruhrgebiet";
                    this.mapType = MapType.Ruhrgebiet;
                    break;
                case 3:
                    sceneName = "Tokyo";
                    this.mapType = MapType.Tokyo;
                    //sateliteBeam = Volt_PrefabFactory.S.Instantiate(Volt_PrefabFactory.S.effect_SateliteBeam,
                    //    Vector3.one * 1000f, Quaternion.identity);
                    //sateliteBeam.SetActive(false);
                    break;
                default:
                    break;
            }
            
            if (Volt_SoundManager.S != null)
                Volt_SoundManager.S.ChangeBGM(this.mapType);
        }

        AsyncOperationHandle handle = Addressables.LoadSceneAsync($"Assets/_Scenes/{sceneName}.unity", LoadSceneMode.Additive, true);
        yield return new WaitUntil(() => { return handle.IsDone; });
        
        yield return new WaitForSeconds(1.5f);
        //noticeText.text = "Field Setup...";
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        StartCoroutine(Volt_ArenaSetter.S.SetupField());
    }
    
    public void PlayerSetupStart() //플레이어 매니저로부터 플레이어 정보를 가져와 맵에 세팅한다.
    {
        curPhase = Phase.playerSetup;
        
        StartCoroutine(PlayerSetup());
    }
    IEnumerator PlayerSetup()
    {
        Debug.Log("PlayerSetup");
        yield return new WaitUntil(() => Volt_PlayerManager.S.GetPlayers().Count == 4);

        List<Volt_PlayerInfo> players = Volt_PlayerManager.S.GetPlayers();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerCam = players[i].GetComponentInChildren<Camera>();
            players[i].playerCamRoot = players[i].GetComponentInChildren<CameraMovement>();
            players[i].raycaster = players[i].GetComponent<Volt_ScreenRaycaster>();
            if (players[i] != Volt_PlayerManager.S.I)
            {
                players[i].playerCam.gameObject.SetActive(false);
                players[i].playerCamRoot.enabled = false;
                players[i].raycaster.enabled = false;
            }
        }
        
        if (IsTutorialMode)
        {
            Volt_PlayerManager.S.I.playerCamRoot.enabled = false;
        }
        foreach (var item in Volt_PlayerManager.S.GetPlayers())
        {
            switch (mapType)
            {
                case MapType.TwinCity:
                    switch (item.playerNumber)
                    {
                        case 1:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(2));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(3));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(4));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(5));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(6));
                            break;
                        case 2:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(54));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(45));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(36));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(27));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(18));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case 3:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(78));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(77));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(76));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(75));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(74));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 4:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(26));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(35));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(44));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(53));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(62));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                        default:
                            break;
                    }
                    break;
                case MapType.Rome:
                    switch (item.playerNumber)
                    {
                        case 1:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(11));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(12));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(13));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(14));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(15));
                            break;
                        case 2:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(55));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(46));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(37));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(28));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(19));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case 3:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(69));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(68));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(67));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(66));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(65));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 4:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(25));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(34));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(43));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(52));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(61));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                        default:
                            break;
                    }
                    break;
                case MapType.Ruhrgebiet:
                    switch (item.playerNumber)
                    {
                        case 1:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(3));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(4));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(5));
                            break;
                        case 2:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(45));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(36));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(27));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case 3:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(77));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(76));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(75));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 4:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(35));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(44));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(53));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                    }
                    break;
                case MapType.Tokyo:
                    switch (item.playerNumber)
                    {
                        case 1:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(1));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(2));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(3));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(4));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(5));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(6));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(7));
                            break;
                        case 2:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(63));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(54));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(45));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(36));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(27));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(18));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(9));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case 3:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(79));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(78));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(77));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(76));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(75));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(74));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(73));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 4:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(17));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(26));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(35));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(44));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(53));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(62));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(71));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                        default:
                            break;
                    }
                    break;
                case MapType.Tutorial:
                    switch (item.playerNumber)
                    {
                        case 1:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(2));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(3));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(4));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(5));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(6));
                            break;
                        case 2:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(54));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(45));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(36));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(27));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(18));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case 3:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(78));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(77));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(76));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(75));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(74));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 4:
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(26));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(35));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(44));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(53));
                            item.startingTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(62));
                            item.playerCamRoot.rotor.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        Volt_PlayerManager.S.I.playerCamRoot.Init();

        yield return new WaitUntil(() => Volt_PlayerManager.S.GetPlayers().Count == 4);

        foreach (var item in Volt_PlayerManager.S.GetPlayers())
        {
            item.playerCam.enabled = true;
        }
        Volt_GMUI.S.matchingScreenPanel.SetActive(false);

        ModuleType moduleType = Volt_ModuleDeck.S.GetRandomModuleType();
        Volt_ModuleCardBase cardBase = Volt_ModuleDeck.S.DrawRandomCard(moduleType);

        if (IsTutorialMode)
        {
            if (Volt_TutorialManager.S && RoundNumber == 0)
            {
                ItemSetupStart(30, 56, 30, Card.SHOCKWAVE);
            }
        }
        else if (IsTrainingMode)
        {
            List<int> tmpVpTiles = new List<int>();
            foreach (var item in Volt_ArenaSetter.S.GetTileArray())
            {
                if (item.pTileType == Volt_Tile.TileType.vpSpace)
                {
                    tmpVpTiles.Add(item.tileIndex);
                }
            }
            List<int> tmpKitTiles = new List<int>();
            foreach (var item in Volt_ArenaSetter.S.GetTileArray())
            {
                if (item.pTileType == Volt_Tile.TileType.workShop)
                {
                    tmpKitTiles.Add(item.tileIndex);
                }
            }
            List<int> tmpModuleTiles = new List<int>();
            foreach (var item in Volt_ArenaSetter.S.GetTileArray())
            {
                if (item.pTileType != Volt_Tile.TileType.pits &&
                    item.pTileType != Volt_Tile.TileType.startingSpace &&
                    item.pTileType != Volt_Tile.TileType.none)
                {
                    //Debug.Log(item + " is module place cadidate tile");
                    tmpModuleTiles.Add(item.tileIndex);
                }
            }
            int randomVpIdx = Random.Range(0, tmpVpTiles.Count);
            int randomKitIdx = Random.Range(0, tmpKitTiles.Count);
            int randomModuleIdx = Random.Range(0, tmpModuleTiles.Count);

            //Debug.Log(tmpModuleTiles[randomModuleIdx] + " is selected");

            ItemSetupStart(tmpVpTiles[randomVpIdx], tmpKitTiles[randomKitIdx], tmpModuleTiles[randomModuleIdx], cardBase.card);
        }
        else
        {
            PacketTransmission.SendFieldReadyCompletionPacket(cardBase.card);
        }
    }

    public void ItemSetupStart(int vpIdx, int repairKitIdx, int moduleIdx, Card cardType)
    {
        behaviour = new Volt_RobotBehavior();
        Debug.Log("ItemSetupStart");
        MyModuleSetupStateInit();
        Volt_GMUI.S.RoundNumber++;
        //noticeText.text = "Item Setting...";
        curPhase = Phase.ItemSetup;

        if (Volt_ArenaSetter.S.numOfVPSetupTile == 0)
            remainRoundCountToVpSetup--;

        StartCoroutine(ItemSetup(vpIdx, repairKitIdx, moduleIdx, cardType));
    }
    void MyModuleSetupStateInit()
    {
        if (!Volt_PlayerManager.S.I.playerRobot) return;
        if (Volt_GMUI.S.RoundNumber == 0) return;

        Volt_Robot myRobot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>();
        if (IsTutorialMode || IsTrainingMode)
        {
            int i = 0;
            foreach (var item in myRobot.moduleCardExcutor.GetCurEquipCards())
            {
                if (item is IActiveModuleCard)
                {
                    myRobot.moduleCardExcutor.SetOffActiveCard(i);
                    i++;
                }
            }
        }
        else
        {
            int i = 0;
            foreach (var item in myRobot.moduleCardExcutor.GetCurEquipCards())
            {
                if (item is IActiveModuleCard)
                {
                    PacketTransmission.SendModuleUnActivePacket(Volt_PlayerManager.S.I.playerNumber, i);
                    i++;
                }
            }
        }
    }
    IEnumerator ItemSetup(int vpIdx, int repairKitIdx, int moduleIdx, Card cardType)
    {
        Debug.Log("ItemSetup");
        //int totalVp = 0;
        
        if (remainRoundCountToVpSetup == 0)
            SetVPInTile(Volt_ArenaSetter.S.GetTileByIdx(vpIdx));
        else
            isVpSetupDone = true;

        //Debug.Log("VP Setup");
        yield return new WaitUntil(()=>isVpSetupDone);
        //Debug.Log("VP Setup Done");
        isVpSetupDone = false;

        int totalKit = 0;
        foreach (var tile in Volt_ArenaSetter.S.GetTileArray())
        {
            if(tile.isHaveRepairKit)
                totalKit += 1;
        }
        if (totalKit < 3)
            SetRepairKitInTile(Volt_ArenaSetter.S.GetTileByIdx(repairKitIdx));
        else
            isRepairkitSetupDone = true;

        //Debug.Log("kit Setup");
        yield return new WaitUntil(() => isRepairkitSetupDone);
        //Debug.Log("kit Setup done");
        isRepairkitSetupDone = false;

        if (Volt_ArenaSetter.S.numOfModule < 6)
            SetModuleInTile(Volt_ArenaSetter.S.GetTileByIdx(moduleIdx),cardType);
        else
            isModuleSetupDone = true;
        //Debug.Log("module Setup");
        yield return new WaitUntil(() => isModuleSetupDone);
        //Debug.Log("module Setup done");
        isModuleSetupDone = false;

        if (mapType == MapType.Ruhrgebiet)
        {
            if (Volt_ArenaSetter.S.numOfSetOnVoltageSpace < 6)
            {
                int[] voltageSpaces = { 21, 23, 29, 33, 47, 51, 57, 59 };
                SetVoltageSpace(Volt_ArenaSetter.S.GetTileByIdx(voltageSpaces[((moduleIdx + vpIdx + repairKitIdx) % 7)]));
            }
            else
                isVoltageSpaceSetupDone = true;

            yield return new WaitUntil(() => isVoltageSpaceSetupDone);
            isVoltageSpaceSetupDone = false;
        }
        yield return new WaitUntil(() => !Volt_PlayerManager.S.I.playerCamRoot.isMoving);
        Debug.Log("All Item Setup done");
        PlaceRobotStart();
    }
    void SetVPInTile(Volt_Tile tile)
    {
        //Debug.Log("SetVPInTile");
        tile.SetVictoryPoint();
        isVpSetupDone = true;
    }
    void SetRepairKitInTile(Volt_Tile tile)
    {
        //Debug.Log("SetRepairKitInTile");
        tile.SetRepairKit();
        isRepairkitSetupDone = true;
    }
    void SetModuleInTile(Volt_Tile tile, Card cardType)
    {
        //Debug.Log("SetModuleInTile");
        tile.SetModule(cardType);
        isModuleSetupDone = true;
    }
    void SetVoltageSpace(Volt_Tile tile)
    {
        isVoltageSpaceSetupDone = true;
        if (tile.IsOnVoltage) return;
        tile.SetVoltage();
        
    }
    


    public void PlaceRobotStart() //서버에서 로봇 배치 타이밍을 알려줄 때 호출 될 수 있도록 한다.
    {
        Debug.Log("PlaceRobotStart");
        if (!IsTutorialMode || !IsTrainingMode)
        {
            Volt_GMUI.S.IsTickOn = true;
            Volt_GMUI.S.TickTimer = 10;
        }
        else
        {
            Volt_GMUI.S.IsTickOn = false;
            Volt_GMUI.S.TickTimer = 99;
        }
        //noticeText.text = "Place your robot...!";
        if (Volt_ArenaSetter.S.robotsInArena.Count != 4)
        {
            if (!Volt_PlayerManager.S.I.GetRobot())
                Volt_GMUI.S.guidePanel.ShowGuideTextMSG(GuideMSGType.RobotSetup, true, Application.systemLanguage);
            else
                Volt_GMUI.S.guidePanel.ShowGuideTextMSG(GuideMSGType.WaitPlaceOtherPlayerRobot, true, Application.systemLanguage);
        }
        curPhase = Phase.robotSetup;
        StartCoroutine(WaitPlaceRobot());

        if (IsTutorialMode && RoundNumber == 1)
            Volt_TutorialManager.S.TutorialStart(0);
    }
    
    IEnumerator WaitPlaceRobot()
    {
        if (!Volt_PlayerManager.S.I.playerRobot)
        {
            Volt_PlayerManager.S.I.playerCamRoot.CamInit();
        }
        Volt_PlayerManager.S.I.playerCamRoot.SetSaturationDown(false);
        if (!IsTutorialMode)
        {
            if (!Volt_PlayerManager.S.I.playerRobot)
            {
                if (IsFullStartingTiles() && pCurPhase == Phase.robotSetup)
                {
                    AutoRobotSetup(Volt_PlayerManager.S.I.playerNumber);
                }
                else
                {
                    foreach (var item in Volt_PlayerManager.S.I.startingTiles)
                    {
                        if (item.GetRobotInTile() == null)
                        {
                            item.SetDefaultBlinkOption();
                            item.BlinkOn = true;
                        }
                    }
                }
            }
        }
        else
        { 
            //튜토리얼 모드
            if (RoundNumber == 1)
            {
                Volt_PlayerManager.S.I.startingTiles[1].SetDefaultBlinkOption();
                Volt_PlayerManager.S.I.startingTiles[1].BlinkOn = true;
            }
            else
            {
                Debug.Log("Tutorial place robot");
                if (!Volt_PlayerManager.S.I.playerRobot)
                {
                    if (IsFullStartingTiles() && pCurPhase == Phase.robotSetup)
                    {
                        AutoRobotSetup(Volt_PlayerManager.S.I.playerNumber);
                    }
                    else
                    {
                        foreach (var item in Volt_PlayerManager.S.I.startingTiles)
                        {
                            if (item.GetRobotInTile() == null)
                            {
                                item.SetDefaultBlinkOption();
                                item.BlinkOn = true;
                            }
                        }
                    }
                }
            }
        }
        //킬봇은 클라이언트 별로 알아서 배치한다.
        //호스트가 로봇이 없는 타일을 피해서 랜덤 타일 추출 후 배치.
        if (IsTutorialMode || IsTrainingMode)
        {
            AutoRobotSetup(2);
            AutoRobotSetup(3);
            AutoRobotSetup(4);
        }
        
        else
        {
            if (Volt_PlayerManager.S.I.PlayerType == PlayerType.HOSTPLAYER && !IsAllUserHasRobot() && pCurPhase == Phase.robotSetup)
            {
                foreach (var item in Volt_PlayerManager.S.GetPlayers())
                {
                    if (item.playerRobot == null)
                    {
                        if (item.PlayerType == PlayerType.AI)
                        {
                            AutoRobotSetup(item.playerNumber);
                        }
                        else if(!item.IsMobileActivated)
                        {
                            AutoRobotSetup(item.playerNumber);
                        }
                    }
                }
            }   
        }

        yield return new WaitUntil(() => IsAllUserHasRobot() || Volt_GMUI.S.TickTimer == 0);

        if (!IsAllUserHasRobot() && pCurPhase == Phase.robotSetup)
        {
            if(Volt_PlayerManager.S.I.PlayerType == PlayerType.HOSTPLAYER)
            {
                foreach (var item in Volt_PlayerManager.S.GetPlayers()) //나말고 다른애들이 로봇이 없으니까 자동배치 시켜주고.
                {
                    if ((!item.playerRobot && !item.IsMobileActivated) || (!item.playerRobot && item.isNeedSynchronization))
                        item.AutoRobotPlace();
                }
            }

            if (!Volt_PlayerManager.S.I.playerRobot) //내꺼 없으면 자동생성하고
            {
                Volt_PlayerManager.S.I.AutoRobotPlace();
            }
            
        }

        yield return new WaitUntil(() => IsAllUserHasRobot() && IsAllRobotIdleState());
        Volt_GMUI.S.IsTickOn = false;



        if (IsTutorialMode || IsTrainingMode)
        {
            if (Volt_TutorialManager.S && RoundNumber == 1)
            {
                Volt_TutorialManager.S.TutorialStart("ExplainationRobotPanelID");
                //yield return new WaitUntil(() => !Volt_TutorialManager.S.isShowingTutorial);
            }
            if (curPhase == Phase.gameOver) yield break;
            SelectBehaviourTypeStart();
            Volt_PlayerUI.S.ShowModuleButton(true);
        }
        else
        {
            PacketTransmission.SendCharacterPositionCompletionPacket();
        }
        //서버로 로봇배치완료 패킷 전송.
    }
    bool IsFullStartingTiles()
    {
        foreach (var item in Volt_PlayerManager.S.I.startingTiles)
        {
            if (!item.GetRobotInTile())
                return false;
        }
        return true;
    }
    public void AutoRobotSetup(int playerNumber)
    {
        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);

        //foreach (var item in player.startingTiles)
        //{
        //    Debug.Log($"{item.tileIndex} is {playerNumber}'s StartSpace");
        //}
        Volt_Tile targetTile = Volt_ArenaSetter.S.GetRandomTileToPlace(player.startingTiles);

        if (IsTutorialMode || IsTrainingMode)
        {
            if (!useCustomPosition)
            {
                PlayerRobotPlaceRequest(playerNumber, targetTile.tilePosInArray.x, targetTile.tilePosInArray.y);
            }
            else
            {
                PlayerRobotPlaceRequest(playerNumber, (int)playerInitPoints[playerNumber - 1].x, (int)playerInitPoints[playerNumber - 1].y);
            }
        }
        else
        {
            Volt_PlayerManager.S.SendCharacterPositionPacket(playerNumber, targetTile.tilePosInArray.x, targetTile.tilePosInArray.y);
        }
    }
    public bool IsAllUserHasRobot()
    {
        foreach (var item in Volt_PlayerManager.S.GetPlayers())
        {      
            if (item.playerRobot == null)
                return false;
        }
        return true;
    }

    public void PlayerRobotPlaceRequest(int playerNumber, int x, int y) //다른 플레이어가 서버를 통해 로봇 배치 메시지를 전달해올때 호출
    {
        Debug.Log(playerNumber + " PlayerRobotPlaceRequest Call");
        if (!Volt_PlayerManager.S.isCanGetPlayerKey(playerNumber)) return;

        Volt_PlayerManager.S.TakePlayerKey(playerNumber);

        RobotPlaceData data;
        data.playerNumber = playerNumber;
        data.x = x;
        data.y = y;

        if (Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber).playerRobot == null)
        {
            waitRobotPlaceQueue.Enqueue(data);
        }

        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(data.playerNumber);
        foreach (var item in player.startingTiles)
        {
            item.BlinkOn = false;
        }
        if (player == Volt_PlayerManager.S.I)
            StartCoroutine(Volt_ArenaSetter.S.GetTile(x, y).SpawnBlink());

        //if(waitRobotPlaceQueue.Count == 4)
        StartCoroutine(PlayerRobotPlace());
    }
    IEnumerator PlayerRobotPlace()
    {
        if (isPlayerRobotPlaceRunning) yield break;
        Debug.Log("Player Robot Place Start");
        isPlayerRobotPlaceRunning = true;
        
        yield return new WaitUntil(() => IsAllPlayerWaitForRobotPlace() && (pCurPhase == Phase.robotSetup || pCurPhase == Phase.synchronization));

        while (waitRobotPlaceQueue.Count != 0)
        {
            RobotPlaceData data = waitRobotPlaceQueue.Dequeue();
            Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(data.playerNumber);
            if (player.playerRobot == null)
            {
                player.skinType = Volt_PrefabFactory.S.SkinTypeDecisionByRobotType(player.RobotType, player.skinType);
                AsyncOperationHandle<GameObject> makeRobotHandle = Managers.Resource.InstantiateAsync($"Robots/{player.RobotType}/{player.RobotType}_{player.skinType}.prefab");
                yield return new WaitUntil(() => { return makeRobotHandle.IsDone; });
                player.playerRobot = makeRobotHandle.Result;
                //player.playerRobot = Volt_PrefabFactory.S.CreateRobot(player.playerNumber, player.RobotType, player.skinType).gameObject;

                Volt_Robot robot = player.playerRobot.GetOrAddComponent<Volt_Robot>();
                robot.Init(player, Volt_ArenaSetter.S.GetTile(data.x, data.y));
            }
            Volt_PlayerManager.S.ReturnPlayerKey(player.playerNumber);
            yield return null;
        }
        
        //해당 플레이어의 로봇넘버를 통해 로봇을 생성한다.
        //전송됀 타일 좌표에 배치한다.
        //생성됀 로봇들을 각각 초기화 해준다.
        if (IsTutorialMode && Volt_TutorialManager.S)
        {
            Volt_TutorialManager.S.FindContentsByName("PlaceRobotWait").gameObject.SetActive(false);
            //Debug.Log("PlaceRobotWait 끝");
        }
        //Debug.Log("Robot Place End");
        isPlayerRobotPlaceRunning = false;
    }
    bool IsAllPlayerWaitForRobotPlace()
    {
        //Debug.Log(waitRobotPlaceQueue.Count + Volt_ArenaSetter.S.robotsInArena.Count);
        if (waitRobotPlaceQueue.Count + Volt_ArenaSetter.S.robotsInArena.Count >= 4)
            return true;
        return false;
    }
    public bool IsAllRobotIdleState()
    {
        foreach (var item in Volt_PlayerManager.S.GetPlayers())
        {
            if (!item.playerRobot) continue;
            Volt_Robot robot = item.playerRobot.GetComponent<Volt_Robot>();
            if (robot.fsm.currentState == null) return false;
            if (robot.fsm.currentState.GetType() != typeof(IdleState))
                return false;
        }
        return true;
    }
    public void SelectBehaviourTypeStart()
    {
        waitRobotPlaceQueue.Clear();
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.panel.IndicateControl(true);
        }

        //조작시간을 부여. 타이머 설정해야함.
        if (mapType != MapType.Tutorial)
        {
            Volt_GMUI.S.IsTickOn = true;
            Volt_GMUI.S.TickTimer = 10;
        }
        if (IsTutorialMode && Volt_TutorialManager.S && Volt_GMUI.S.RoundNumber == 2)
        {
            Volt_TutorialManager.S.TutorialStart("ExplainationModuleType");
        }
        //noticeText.text = "What's your behaviour";
        Volt_GMUI.S.guidePanel.ShowGuideTextMSG(GuideMSGType.BehaviourSelect, true, Application.systemLanguage);
        curPhase = Phase.behavoiurSelect;
        Volt_PlayerUI.S.BehaviourSelectOn();
        
        StartCoroutine(SelectBehaviour());
    }
    IEnumerator SelectBehaviour()
    {
        yield return new WaitUntil(() => isBehaviourSelectDone || Volt_GMUI.S.TickTimer == 0);
        if (isBehaviourSelectDone)
        {
            Volt_PlayerUI.S.BehaviourSelectOff();
            SelectRangeStart();
        }
        else
        {
            Volt_PlayerUI.S.BehaviourSelectOff();
            Volt_PlayerInfo pInfo = Volt_PlayerManager.S.I;
            behaviour.SetBehaivor(0, BehaviourType.None, Volt_PlayerManager.S.I.playerNumber);
            Volt_PlayerManager.S.SendBehaviorOrderPacket(pInfo.playerNumber, behaviour);

            
            if (Volt_PlayerManager.S.I.PlayerType == PlayerType.HOSTPLAYER)
            {
                foreach (var item in Volt_PlayerManager.S.GetPlayers())
                {
                    if (item.PlayerType == PlayerType.PLAYER && !item.IsMobileActivated)
                    {
                        Volt_PlayerManager.S.SendBehaviorOrderPacket(item.playerNumber, behaviour);
                    }
                }
                //print("Detection!!!!!!!");
                DoAllKillbotsDetection();
            }
            yield return new WaitUntil(() => IsInputBehaviourAllPlayer());
            yield return new WaitForSeconds(1f);
            PacketTransmission.SendBehaviorOrderCompletionPacket();
        }
        isBehaviourSelectDone = false;
    }
    public void SelectBehaviourDoneCallback(GameObject btnType) // 0 = move , 1 = atk
    {
        if (IsTutorialMode)
        {
            if (Volt_TutorialManager.S && RoundNumber == 1)
            {
                Volt_TutorialManager.S.FindContentsByName("WaitSelectBehaviour").gameObject.SetActive(false);
                //Debug.Log("WaitSelectBehaviour 끝");
            }
        }
        if (btnType.name == "Move")
        {
            behaviour.SetBehaivor(0, BehaviourType.Move, Volt_PlayerManager.S.I.playerNumber);
        }
        else if (btnType.name == "Attack")
        {
            behaviour.SetBehaivor( 0, BehaviourType.Attack, Volt_PlayerManager.S.I.playerNumber);
        }
        else
        {
            behaviour.SetBehaivor(0, BehaviourType.None, Volt_PlayerManager.S.I.playerNumber);
        }
        isBehaviourSelectDone = true;
    }
    public void SelectBehaviourDoneCallback2(BehaviourType behaviourType) // 0 = move , 1 = atk
    {
        if (IsTutorialMode)
        {
            if (Volt_TutorialManager.S && RoundNumber == 1)
            {
                Volt_TutorialManager.S.FindContentsByName("WaitSelectBehaviour").gameObject.SetActive(false);
                //Debug.Log("WaitSelectBehaviour 끝");
            }
        }
        if (behaviourType == BehaviourType.Move)
        {
            behaviour.SetBehaivor(0, BehaviourType.Move, Volt_PlayerManager.S.I.playerNumber);
        }
        else if (behaviourType == BehaviourType.Attack)
        {
            behaviour.SetBehaivor(0, BehaviourType.Attack, Volt_PlayerManager.S.I.playerNumber);
        }
        else
        {
            behaviour.SetBehaivor(0, BehaviourType.None, Volt_PlayerManager.S.I.playerNumber);
        }
        Volt_PlayerUI.S.BehaviourSelectOff();
        isBehaviourSelectDone = true;
    }

    public void SelectRangeStart()
    {
        Volt_PlayerUI.S.BehaviourSelectOff();
        if (mapType != MapType.Tutorial)
        {
            Volt_GMUI.S.IsTickOn = true;
            Volt_GMUI.S.TickTimer = 15;
        }
        // noticeText.text = "Control your robot";
        Volt_GMUI.S.guidePanel.ShowGuideTextMSG(GuideMSGType.BehaviourSelect, true,Application.systemLanguage);
        curPhase = Phase.rangeSelect;
        StartCoroutine(SelectRange());
        Volt_PlayerManager.S.I.playerCamRoot.enabled = false;
    }
    IEnumerator SelectRange()
    {
        if (!Volt_PlayerManager.S.I.IsMobileActivated)
            yield break;
        Volt_PlayerManager.S.I.playerCamRoot.SaveLastInfo();
        //foreach (var item in Volt_PlayerManager.S.GetPlayers())
        //{
        //    if(item.playerRobot)
        //        item.playerRobot.GetComponent<Volt_Robot>().panel.GetComponent<UIPanel>().alpha = 0f;
        //}

        //yield return StartCoroutine(WaitCameraAnimation(false));
        StartCoroutine(WaitCameraAnimation(false));
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            if (item.VpCoinInstance)
                item.VpCoinInstance.GetComponent<FXV.FXVRotate>().RenewRotation(new Vector3(0f, 0f, 90f));
            else if(item.RepairKitInstance)
                item.RepairKitInstance.GetComponent<FXV.FXVRotate>().RenewRotation(new Vector3(90f, 0f, 0f));
        }

        if (IsTutorialMode && Volt_TutorialManager.S)
        {
            Volt_TutorialManager.S.TutorialStart("ExplainationVPTile");
        }
        
        if (!Volt_PlayerManager.S.I.IsMobileActivated)
            yield break;

        foreach (var item in Volt_ArenaSetter.S.GetCrossTiles(Volt_PlayerManager.S.I.playerRobot.transform.position, 6))
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = true;
        }
        Volt_Robot robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>();
        if (behaviour.BehaviourType == BehaviourType.Move)
        {
            if (robot.moduleCardExcutor.IsHaveModuleCard(Card.STEERINGNOZZLE))
            {
                foreach (var item in Volt_ArenaSetter.S.GetDiagonalTiles(Volt_PlayerManager.S.I.playerRobot.transform.position, 6))
                {
                    item.SetDefaultBlinkOption();
                    item.BlinkOn = true;
                }
            }
        }
        else if (behaviour.BehaviourType == BehaviourType.Attack)
        {
            foreach (var item in Volt_ArenaSetter.S.GetDiagonalTiles(Volt_PlayerManager.S.I.playerRobot.transform.position, 6))
            {
                item.SetDefaultBlinkOption();
                item.BlinkOn = true;
            }
        }
        else
        {
            Volt_Tile tile = Volt_ArenaSetter.S.GetTile(robot.transform.position, robot.transform.forward, 1);
            tile.SetDefaultBlinkOption();
            tile.BlinkOn = true;
        }
        
        StartCoroutine(WaitSelectRangeDone());
        yield break;
    }
    
    public IEnumerator WaitCameraAnimation(bool rewind)
    {
        isRangeSelectCameraMoving = true;
        
        CameraMovement camRoot = Volt_PlayerManager.S.I.playerCamRoot;
        
        if (!rewind)
        {
            Vector3 startPos = camRoot.cam.transform.position;
            Vector3 endPos = new Vector3(Volt_ArenaSetter.S.GetCenterTransform().position.x, 33f, Volt_ArenaSetter.S.GetCenterTransform().position.z);
            Vector3 middlePos = (endPos - startPos);
            

            Vector3 rot1;
            Vector3 rot2 = Vector3.zero;

            float t = 0f;
            middlePos = new Vector3(startPos.x, endPos.y, startPos.z);
            middlePos.y += 10f;
            rot1 = camRoot.cam.transform.eulerAngles;
            switch (Volt_PlayerManager.S.I.playerNumber)
            {
                case 1:
                    break;
                case 2:
                    rot2.y = 90f;
                    break;
                case 3:
                    rot2.y = 180f;
                    break;
                case 4:
                    rot2.y = 270f;
                    break;
                default:
                    break;
            }
            rot2.x = 90f;
       
            while (t <= 1f)
            {
                if (!Volt_PlayerManager.S.I.IsMobileActivated)
                    break;
                t += Time.deltaTime * 2f;// 여기서 속도 조절
                Vector3 camPos = Vector3.Lerp(Vector3.Lerp(startPos, middlePos, t), Vector3.Lerp(middlePos, endPos, t), t);
                camRoot.cam.transform.position = camPos;

                camRoot.cam.transform.rotation = Quaternion.Euler(Vector3.Lerp(rot1, rot2, t));

                if (curPhase == Phase.waitSync)
                {
                    camRoot.CamInit();
                    camRoot.SaveLastInfo();
                    yield break;
                }
                yield return null;
            }
            
            //isRangeSelectCameraMoving = false;
            //yield break;
        }
        else
        {
            Vector3 startPos = camRoot.cam.transform.position;
            Vector3 endPos = camRoot.lastCamWorldPos;
            Vector3 middlePos = (endPos - startPos);
            

            Vector3 rot1;
            Vector3 rot2;

            float t = 0f;
            middlePos = new Vector3(startPos.x, endPos.y, startPos.z);
            middlePos.y += 10f;
            rot1 = camRoot.cam.transform.eulerAngles;
            rot2 = camRoot.tilter.transform.localRotation.eulerAngles;

            switch (Volt_PlayerManager.S.I.playerNumber)
            {
                case 1:
                    break;
                case 2:
                    rot2.y = 90f;
                    break;
                case 3:
                    rot2.y = 180f;
                    break;
                case 4:
                    rot2.y = 270f;
                    break;
                default:
                    break;
            }

            while (t <= 1f)
            {
                t += Time.deltaTime * 2f;// 여기서 속도 조절
                Vector3 camPos = Vector3.Lerp(Vector3.Lerp(startPos, middlePos, t), Vector3.Lerp(middlePos, endPos, t), t);
                camRoot.cam.transform.position = camPos;

                camRoot.cam.transform.rotation = Quaternion.Euler(Vector3.Lerp(rot1, rot2, t));

                yield return null;
            }
            isRangeSelectCameraMoving = false;
        }
    }

    public void SelectRangeDoneCallback(Volt_Tile selectedTile)
    {
        if (IsTutorialMode)
        {
            if (Volt_TutorialManager.S && RoundNumber == 1)
            {
                Volt_TutorialManager.S.FindContentsByName("WaitSelectRange").gameObject.SetActive(false);
            }
            else
            {
                Volt_PlayerManager.S.I.playerCamRoot.enabled = true; //210223
            }
        }
        else
        {
            Volt_PlayerManager.S.I.playerCamRoot.enabled = true;
            Volt_PlayerManager.S.I.playerCamRoot.RTSCameraInit();
        }
        
        Volt_PlayerInfo I = Volt_PlayerManager.S.I;
        Volt_Robot robot = I.playerRobot.GetComponent<Volt_Robot>();
        Volt_Tile curStandingTile = Volt_ArenaSetter.S.GetTile(robot.transform.position);

        Vector3 direction = (selectedTile.transform.position - curStandingTile.transform.position).normalized;
        // Debug.Log($"[SelectRangeDonCallback] direction:{direction}");
        behaviour.SetBehaivor(Volt_ArenaSetter.S.GetDistanceBetweenTiles(curStandingTile, selectedTile), direction, behaviour.BehaviourType, Volt_PlayerManager.S.I.playerNumber, selectedTile);
        
        Volt_PlayerInfo pInfo = Volt_PlayerManager.S.I;
        if (IsTutorialMode || IsTrainingMode)
            RobotBehaviourInputToManager(behaviour);
        else
            Volt_PlayerManager.S.SendBehaviorOrderPacket(pInfo.playerNumber, behaviour);
        isRangeSelectDone = true;
    }
    public void RobotBehaviourInputToManager(Volt_RobotBehavior behaviour) //서버에서 브로드캐스팅 받을것임.
    {
        if (!Volt_PlayerManager.S.isCanGetPlayerKey(behaviour.PlayerNumber))
            return;
        foreach (var item in tmpBehaviours)
        {
            if (item.PlayerNumber == behaviour.PlayerNumber)
            {
                return;
            }
        }
        Volt_PlayerManager.S.TakePlayerKey(behaviour.PlayerNumber);
        tmpBehaviours.Add(behaviour);
        Volt_PlayerManager.S.ReturnPlayerKey(behaviour.PlayerNumber);

        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(behaviour.PlayerNumber);
        player.GetRobot().panel.IndicateControl(false);
    }
    bool IsInputBehaviourAllPlayer()
    {
        if (tmpBehaviours.Count >= Volt_PlayerManager.S.GetCurrentNumberOfActive())
        {
            return true;
        }
        else
        {
            Debug.Log(tmpBehaviours.Count + ", " + Volt_PlayerManager.S.GetCurrentNumberOfActive());
        }
        return false;
    }
    bool IsAllTimeBombPlayEnd()
    {
        Volt_TimeBomb[] timeBombInstances = FindObjectsOfType<Volt_TimeBomb>();
        foreach (var item in timeBombInstances)
        {
            if (item.isPlaying)
            {
                Debug.Log($"{item.transform.position}' Timebomb Playing yet");
                return false;
            }
        }
        return true;
    }
    IEnumerator WaitSelectRangeDone()
    {
        yield return new WaitUntil(()=>isRangeSelectDone|| Volt_GMUI.S.TickTimer == 0); //모든 로봇에 행동정보가 입력이 되었는가?
        Volt_GMUI.S.IsTickOn = false;
        
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = false;
        }
        if (!isRangeSelectDone)
        {
            Volt_PlayerManager.S.SendBehaviorOrderPacket(Volt_PlayerManager.S.I.playerNumber, new Volt_RobotBehavior());
        }
        isRangeSelectDone = false;


        //yield return StartCoroutine(WaitCameraAnimation(true));

        StartCoroutine(WaitCameraAnimation(true));
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            if (item.VpCoinInstance)
                item.VpCoinInstance.GetComponent<FXV.FXVRotate>().SetDefaultRotation();
            else if (item.RepairKitInstance)
                item.RepairKitInstance.GetComponent<FXV.FXVRotate>().SetDefaultRotation();
        }
        if (Volt_PlayerManager.S.I.PlayerType == PlayerType.HOSTPLAYER)
        {
            //print("Detection!!!!!!!");
            DoAllKillbotsDetection();
        }
        //
        yield return new WaitUntil(() => IsInputBehaviourAllPlayer());

        if (IsTutorialMode)
        {
            SimulationStart(1, 2, 3, 4);
            Volt_PlayerUI.S.ShowModuleButton(false);
        }
        else if (IsTrainingMode)
        {
            List<Volt_Tile> tmpTiles = new List<Volt_Tile>();
            foreach (var item in Volt_ArenaSetter.S.GetTileArray())
            {
                if (item.pTileType != Volt_Tile.TileType.none)
                {
                    tmpTiles.Add(item);
                }
            }
            SimulationStart(tmpTiles[Random.Range(0, tmpTiles.Count)].tileIndex, tmpTiles[Random.Range(0, tmpTiles.Count)].tileIndex, tmpTiles[Random.Range(0, tmpTiles.Count)].tileIndex, tmpTiles[Random.Range(0, tmpTiles.Count)].tileIndex);
            Volt_PlayerUI.S.ShowModuleButton(false);
        }
        else
        {
            yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
            yield return new WaitForSeconds(1f);
            PacketTransmission.SendBehaviorOrderCompletionPacket();
        }
    }

    public void SimulationStart(int option,int option2, int option3, int option4)
    {
        //Debug.Log("SimulationStart : " + option.ToString() +","+ option2.ToString() + "," + option3.ToString() + "," + option4.ToString());
        //noticeText.text = "Wait Simulation";
        curPhase = Phase.simulation;
        isLocalSimulationDone = false;
        SortedRobotList();
        //StopCoroutine(WaitAmargeddon);
        StartCoroutine(Simulate(option, option2, option3, option4));
    }
   
    public IEnumerator Simulate(int option, int option2, int option3, int option4)
    {
        yield return new WaitUntil(() => !isRangeSelectCameraMoving);
        //if (isGameOverWaiting)
        //    yield break;
        //Debug.Log("시뮬레이션 시작");

        yield return StartCoroutine(SortRobotSimulationOrder());
        while (behaviourStack.Count > 0)
        {
            if (isGameOverWaiting)
                break;

            yield return new WaitUntil(() => Volt_GMUI.S.IsCheatPanelOn == false);
            yield return new WaitUntil(() => !pause);
            foreach (var item in Volt_ArenaSetter.S.robotsInArena)
            {
                item.PushType = PushType.PushedCandidate;
            }
            Volt_RobotBehavior currentBehaviour = PopBehaviour();
            int currentBehaviourPlayerNumber = currentBehaviour.PlayerNumber;
            yield return StartCoroutine(RobotDoBehaviour(currentBehaviour));
            Volt_GamePlayData.S.ClearOtherRobotsAttackedByRobotsOnThisTurn();

            yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
            yield return new WaitUntil(() => IsAllRobotIdleState());
            RobotBehaviourObserver.Instance.currentPusher = null;

            if (AmargeddonCount > 0 && AmargeddonPlayer != 0)
            {
                //Debug.Log("Simulate : " + option.ToString() + "," + option2.ToString() + "," + option3.ToString() + "," + option4.ToString());
                //Debug.Log(AmargeddonPlayer +" : " + AmargeddonCount + "번 남았으");
                yield return StartCoroutine(WaitAmargeddon(option, option2, option3, option4));
                Volt_GamePlayData.S.ClearOtherRobotsAttackedByRobotsOnThisTurn();
            }

            yield return new WaitUntil(() => Volt_PlayerManager.S.I.playerCamRoot.isMoving == false);
            yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
            yield return new WaitUntil(() => IsAllRobotIdleState());
            foreach (var item in Volt_ArenaSetter.S.robotsInArena)
            {
                item.fsm.attackInfo = null;
            }
        }
        //카메라 원래대로 돌려놔야함
        optionIdx = 0;
        yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
        yield return new WaitUntil(() => Volt_ArenaSetter.S.IsAllTileBlinkOff());


        RobotOrderNumberInit(); //로봇 행동순서 UI 초기화

        if (!isGameOverWaiting)
        {
            yield return StartCoroutine(WaitTimeBombCount()); //시한폭탄 처리 기다림
            yield return new WaitUntil(() => Volt_ArenaSetter.S.IsAllTileBlinkOff());


            Volt_GamePlayData.S.ClearOtherRobotsAttackedByRobotsOnThisTurn();

            if (isOnSuddenDeath)
            {
                Debug.Log("Enter Sudden death");
                yield return StartCoroutine(WaitSuddenDeath(option, option2, option3, option4));
                yield return new WaitUntil(() => Volt_ArenaSetter.S.IsAllTileBlinkOff());
            }

            foreach (var item in Volt_ArenaSetter.S.robotsInArena)
            {
                item.PushType = PushType.PushedCandidate;
            }

            yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());

            Volt_GamePlayData.S.ClearOtherRobotsAttackedByRobotsOnThisTurn();
            if (IsTutorialMode || IsTrainingMode)
                ResolutionStart();
            else
                PacketTransmission.SendSimulationCompletionPacket();
            tmpBehaviours.Clear();
        }
        //print("All robot simulation over");
        isLocalSimulationDone = true;
        
    }
    public void ForcedStopSimulate()
    {
        StopCoroutine("Simulate");
    }
    IEnumerator SortRobotSimulationOrder()
    {
        List<Volt_RobotBehavior> tmpRobotBehaviourList = new List<Volt_RobotBehavior>(behaviourStack);
        int orderIdx = 1;
        foreach (var item in tmpRobotBehaviourList)
        {
            Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(item.PlayerNumber);
            Volt_Robot robot = player.playerRobot.GetComponent<Volt_Robot>();
            if(item.BehaviourType == BehaviourType.None)
            {
                robot.panel.OrderNumberSet(-1);
            }
            else
            {
                robot.panel.OrderNumberSet(orderIdx);
                orderIdx++;
            }
        }
        yield break;
    }
    //IEnumerator CameraFollowToCurrentTarget(GameObject robot)
    //{
    //    Volt_PlayerManager.S.I.playerCamRoot.CameraMoveStart(robot);
    //    yield return new WaitUntil(() => Volt_PlayerManager.S.I.playerCamRoot.isMoving == false);
    //}
    IEnumerator RobotDoBehaviour(Volt_RobotBehavior behavior)
    {
        if (behavior.BehaviourType == BehaviourType.None) yield break ;
        

        print("player : " + behavior.PlayerNumber + ", " + ", " + behavior.Direction + ", " + behavior.BehaviorPoints + ", " + behavior.BehaviourType);
        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(behavior.PlayerNumber);
        if (player.playerRobot == null) yield break;
        RobotBehaviourObserver.Instance.OnBehaviorFlag(player.playerNumber);
        Volt_Robot robot = player.playerRobot.GetComponent<Volt_Robot>();
        // 로봇의 행동이 이동이면 해당 로봇을 Pusher로 설정한다.
        if (behavior.BehaviourType == BehaviourType.Move)
        {
            robot.PushType = PushType.Pusher;
        }
        RobotBehaviourObserver.Instance.currentPusher = robot;

        robot.fsm.behavior = behavior;

        yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());

        Debug.Log("Cehck player vp");
        if (IsTutorialMode || IsTrainingMode)
        {
            foreach (var item in Volt_PlayerManager.S.GetPlayers())
            {
                if (isEndlessGame) break;

                Debug.Log($"player[{item.playerNumber}] vp:{item.VictoryPoint}");
                if (item.VictoryPoint >= 3)
                    GameOver(item.playerNumber);
            }
        }
        else
        {
            //if (isEndlessGame)
            //    PacketTransmission.SendVictoryPointPacket(robot.playerInfo.playerNumber, 0);
            //else
            //    PacketTransmission.SendVictoryPointPacket(robot.playerInfo.playerNumber, robot.playerInfo.VictoryPoint);
        }
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            if (robot != null)
                item.standingTile = Volt_ArenaSetter.S.GetTile(robot.transform.position);
        }
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = false;
        }
        foreach (var item in dodgeRobots)
        {
            item.GetComponent<Collider>().enabled = true;
        }
        dodgeRobots.Clear();

        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.lastAttackPlayer = 0;
        }
    }
    IEnumerator WaitAmargeddon(int option, int option2, int option3, int option4)
    {
        Volt_Robot armageddonRobot = Volt_PlayerManager.S.GetPlayerByPlayerNumber(AmargeddonPlayer).playerRobot.GetComponent<Volt_Robot>();
        int[] options = { option, option2, option3, option4 };

        --AmargeddonCount;
        isPlayAmargeddon = true;
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/SFx_200703/Amargeddon.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        GameObject amargeddonGo = Instantiate(amargeddonPrefab);
        //Volt_Tile bombSite = Volt_ArenaSetter.S.SearchBombSite();

        //Debug.Log("WaitAmargeddon : " + options[optionIdx]);

        Volt_Tile targetTile = Volt_ArenaSetter.S.GetTileByIdx(options[optionIdx]);
        optionIdx++;
        List<Volt_Tile> bombSites = new List<Volt_Tile>(targetTile.GetAdjecentTiles());
        bombSites.Add(targetTile);
        foreach (var bombSite in bombSites)
        {
            if (bombSite == null) continue;
            bombSite.SetBlinkOption(BlinkType.Attack, 0.3f);
            bombSite.BlinkOn = true;
        }

        if (targetTile == null)
        {
            //Debug.LogWarning("BombSite is null");
            List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;
            int idx = Random.Range(0, robots.Count);

            List<Volt_Tile> tiles = new List<Volt_Tile>();
            tiles.Add(Volt_ArenaSetter.S.GetTile(robots[idx].transform.position));

            List<Volt_Tile> adjTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTile(robots[idx].transform.position).GetAdjecentTiles());
            for (int i = 0; i < adjTiles.Count; i++)
            {
                if (adjTiles[i] == null)
                    continue;
                tiles.Add(adjTiles[i]);
            }

            idx = Random.Range(0, tiles.Count);
            targetTile = tiles[idx];
        }
        amargeddonGo.GetComponent<Volt_Amargeddon>().Init(targetTile);

        yield return new WaitUntil(() => { return !isPlayAmargeddon; });
        yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = false;
        }
        if (!IsTutorialMode && !IsTrainingMode)
        {
            //if (isEndlessGame)
            //    PacketTransmission.SendVictoryPointPacket(AmargeddonPlayer, 0);
            //else
            //{
                //Debug.Log("Send after amargeddon Vp Packet : " + AmargeddonPlayer);
                //Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(AmargeddonPlayer);
                //if (AmargeddonPlayer > 0 && AmargeddonPlayer < 5)
                //    PacketTransmission.SendVictoryPointPacket(AmargeddonPlayer, player.VictoryPoint);
            //}
        }
        if (AmargeddonCount == 0)
        {
            if (amargeddonPlayer != null)
                armageddonRobot.moduleCardExcutor.DestroyCard(Card.AMARGEDDON);
        }
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.lastAttackPlayer = 0;
        }
    }
    void RobotOrderNumberInit()
    {
        foreach (var item in Volt_PlayerManager.S.GetPlayers())
        {
            if (item.playerRobot)
                item.playerRobot.GetComponent<Volt_Robot>().panel.OrderNumberSet(-1);
        }
    }
    IEnumerator WaitTimeBombCount()
    {
        //foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        //{
        //    if (item.TimeBombInstance)
        //        item.TimeBombInstance.CountDown();
        //}
        foreach (var item in FindObjectsOfType<Volt_TimeBomb>())
        {
            item.CountDown();
        }
        yield return new WaitUntil(() => IsAllTimeBombPlayEnd());
        yield return new WaitUntil(() => IsAllRobotIdleState());
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = false;
        }
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.lastAttackPlayer = 0;
        }
    }

    IEnumerator WaitSuddenDeath(int option, int option2, int option3, int option4)
    {
        curPhase = Phase.suddenDeath;
        if(RoundNumber == 10)
        {
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/suddendeath_alram.mp3",
                (result) =>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                });
            Volt_GMUI.S.guidePanel.ShowSpriteAnimationMSG(GuideMSGType.SuddenDeath, true);
            yield return new WaitForSeconds(2.5f);
        }
        switch (mapType)
        {
            case MapType.TwinCity:
                List<Volt_Tile> suddenDeathTiles = Volt_ArenaSetter.S.GetSuddenDeathTile(option);

                foreach (var tile in suddenDeathTiles)
                {
                    tile.pitMonster.GetComponent<Volt_PitMonster>().MonsterTargetSearchAnimationStart();
                }
                yield return new WaitUntil(() => !IsAnyMonsterActive(suddenDeathTiles));
                //스코프 애니메이션 끝날때까지 대기.
                foreach (var item in suddenDeathTiles)
                {
                    item.pitMonster.GetComponent<Volt_PitMonster>().DoAttack(item.GetRobotDirectionInRandomAdjecentTiles(option));
                }
                yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff() && !IsAnyMonsterActive(suddenDeathTiles));
                break;
            case MapType.Rome:
                
                //0~13의 int값
                randomBallistaLaunchPoint = option % 13;
                //Debug.Log("randomBallistaLaunchPoint : "+ randomBallistaLaunchPoint);
                List<Volt_Tile> targetTiles = null;
                switch (randomBallistaLaunchPoint)
                {
                    case 0:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(9, 17));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(9));
                        break;
                    case 1:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(18, 26));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(18));
                        break;
                    case 2:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(27, 35));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(27));
                        break;
                    case 3:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(36, 44));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(36));
                        break;
                    case 4:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(45, 53));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(45));
                        break;
                    case 5:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(54, 62));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(54));
                        break;
                    case 6:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(63, 71));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(63));
                        break;
                    case 7:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(73, 1));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(73));
                        break;
                    case 8:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(74, 2));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(74));
                        break;
                    case 9:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(75, 3));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(75));
                        break;
                    case 10:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(76, 4));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(76));
                        break;
                    case 11:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(77, 5));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(77));
                        break;
                    case 12:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(78, 6));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(78));
                        break;
                    case 13:
                        targetTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTiles(79, 7));
                        targetTiles.Add(Volt_ArenaSetter.S.GetTileByIdx(79));
                        break;
                    default:
                        //Debug.Log("Error");
                        break;
                }

                foreach (var item in targetTiles)
                {
                    item.SetBlinkOption(BlinkType.Attack, 0.5f);
                    item.BlinkOn = true;
                }
                GameObject ballistaInstance = Volt_PrefabFactory.S.PopObject(Define.Objects.Ballista);
                if (ballistaInstance == null)
                    break;

                ballistaInstance.transform.position = ballistaLaunchPoints[randomBallistaLaunchPoint].transform.position;
                ballistaInstance.transform.rotation = Quaternion.identity;
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/suddendeath_Balista_Sound.wav",
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });
                GameObject ballistaEffect = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_BallistaSonicBoom);
                ballistaEffect.transform.rotation = Quaternion.identity;
                if (randomBallistaLaunchPoint <= 6)
                {
                    ballistaInstance.GetComponent<Volt_Ballista>().Init(Vector3.right, targetTiles);
                }
                else
                {
                    ballistaInstance.GetComponent<Volt_Ballista>().Init(Vector3.back, targetTiles);
                    ballistaEffect.transform.Rotate(0f, 90f, 0f);
                }
                Vector3 effectPos = targetTiles[targetTiles.Count / 2].transform.position;
                effectPos.y += ballistaLaunchPoints[randomBallistaLaunchPoint].transform.position.y;
                ballistaEffect.transform.position = effectPos;
                yield return new WaitUntil(() => !ballistaInstance.activeSelf);

                break;
            case MapType.Ruhrgebiet:
                if (isOnSuddenDeath)
                {
                    switch (RoundNumber)
                    {
                        case 10:
                            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/suddendeath_factory.mp3",
                                (result) =>
                                {
                                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                                });

                            foreach (var item in Volt_ArenaSetter.S.fallTiles1)
                            {
                                item.Fall();
                            }
                            yield return new WaitUntil(() => Volt_ArenaSetter.S.IsAllTileAnimationDone(Volt_ArenaSetter.S.fallTiles1.ToArray()));

                            foreach (var item in Volt_PlayerManager.S.GetPlayers())
                            {
                                Volt_PlayerManager.S.RuhrgebietChangeStartingTiles(item.playerNumber,1);
                            }


                            break;
                        case 13:
                            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/suddendeath_factory.mp3",
                                (result) =>
                                {
                                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                                });
                            foreach (var item in Volt_ArenaSetter.S.fallTiles2)
                            {
                                item.Fall();
                            }

                            yield return new WaitUntil(() => Volt_ArenaSetter.S.IsAllTileAnimationDone(Volt_ArenaSetter.S.fallTiles2.ToArray()));

                            foreach (var item in Volt_PlayerManager.S.GetPlayers())
                            {
                                Volt_PlayerManager.S.RuhrgebietChangeStartingTiles(item.playerNumber,2);
                            }
                            break;
                        default:
                            break;
                    }
                }
                // 가장자리부터 타일이 한줄씩 사라질것임. 값 필요 x;
                break;
            case MapType.Tokyo:
                
                int randomSatelitePoint = (option % 4);
                Volt_Tile targetTile = null;
                switch (randomSatelitePoint)
                {
                    case 0:
                        targetTile = Volt_ArenaSetter.S.GetTileByIdx(option);
                        break;
                    case 1:
                        targetTile = Volt_ArenaSetter.S.GetTileByIdx(option2);
                        break;
                    case 2:
                        targetTile = Volt_ArenaSetter.S.GetTileByIdx(option3);
                        break;
                    case 3:
                        targetTile = Volt_ArenaSetter.S.GetTileByIdx(option3);
                        break;
                    default:
                        break;
                }
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/suddendeath_Satlite.wav",
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });
               
                yield return new WaitForSeconds(1.9f);

                Vector3 pos = targetTile.transform.position;
                pos.y += 26f;
                GameObject satelitebeam = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_Satelitebeam);
                satelitebeam.transform.position = pos;
                satelitebeam.GetComponent<ParticleSystem>().Play();
                List<Volt_Tile> sateliteTiles = new List<Volt_Tile>();
                sateliteTiles.Add(targetTile);
                foreach (var item in targetTile.GetAdjecentTiles())
                {
                    if (item == null) continue;
                    sateliteTiles.Add(item);
                }

                Volt_PlayerManager.S.I.playerCamRoot.SetShakeType(CameraShakeType.Satlite);
                Volt_PlayerManager.S.I.playerCamRoot.CameraShake();

                foreach (var item in sateliteTiles)
                {
                    item.SetBlinkOption(BlinkType.Attack, 0.5f);
                    item.BlinkOn = true;
                    if (item.GetRobotInTile())
                    {
                        Volt_Robot robot = item.GetRobotInTile();
                        robot.GetDamage(new AttackInfo(robot.playerInfo.playerNumber,1,CameraShakeType.Satlite));
                        //robot.PlayDamagedEffect(robot.transform);
                    }
                }
                // 랜덤 위치에 폭격.
                yield return new WaitForSeconds(2.5f);
                Volt_PrefabFactory.S.PushEffect(satelitebeam.GetComponent<Poolable>());
                break;
            default:
                break;
        }
        yield return new WaitUntil(()=>RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
        yield return new WaitUntil(() => IsAllRobotIdleState());

        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = false;
        }

        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.lastAttackPlayer = 0;
        }
        //yield return new WaitForSeconds(0.8f);
        curPhase = Phase.simulation;
    }
    public void ResolutionStart() //서버로부터 
    {
        if (curPhase == Phase.waitSync) return;
        
        curPhase = Phase.resolution;
        //noticeText.text = "Resolution Wait...";
        StartCoroutine(Resolution());
        //현재 로봇들의 위치에 따라 결과 결정.
        //->이후 다시 승점, 모듈배치부터.
    }
    IEnumerator Resolution()
    {
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.DoResolutionStart();
        }
        
        yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff() && IsAllResolutionEnd());
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = false;
        }
        yield return new WaitForSeconds(2.2f);
        yield return new WaitUntil(() => Volt_ArenaSetter.S.IsAllTileBlinkOff());

        if (IsTutorialMode || IsTrainingMode)
        {
            int cnt = 1;
            foreach (var item in Volt_PlayerManager.S.GetPlayers())
            {
                if (isEndlessGame) break;
                if (item.VictoryPoint >= 3)
                {
                    GameOver(item.playerNumber);
                    break;
                }
                cnt++;
            }
        }
        else
        {
            foreach (var item in Volt_PlayerManager.S.GetPlayers())
            {
                if (item.VictoryPoint >= 3)
                    yield break;
            }
        }

        Volt_GMUI.S.guidePanel.ShowSpriteAnimationMSG(GuideMSGType.RoundEnd, true);

        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/etc/RoundOverSound.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        if (IsTutorialMode)
        {
            yield return new WaitForSeconds(1.5f);
            int[] randomVpTile = { 57, 47, 50, 30, 33, 23 };
            int vpIdx = Random.Range(0, randomVpTile.Length);
            int[] randomRepairTile = { 56, 40, 24 };
            int repairIdx = Random.Range(0, randomRepairTile.Length);
            int[] randomModuleTile = { 1, 7, 9, 10, 11, 12, 13, 15, 16, 17, 19, 21, 22, 25, 28, 29, 31, 32, 37, 38, 39, 41, 42, 43, 48, 49, 51, 52, 55, 58, 59, 61, 63, 64, 65, 67, 68, 69, 70, 71, 73, 79 };
            int moduleIdx = Random.Range(0, randomModuleTile.Length);


            Volt_ModuleCardBase cardBase = Volt_ModuleDeck.S.DrawRandomCard(Volt_ModuleDeck.S.GetRandomModuleType());


            ItemSetupStart(randomVpTile[vpIdx], randomRepairTile[repairIdx], randomModuleTile[moduleIdx], cardBase.card);
        }
        else if (IsTrainingMode)
        {
            yield return new WaitForSeconds(1.5f);

            Volt_ModuleCardBase cardBase = Volt_ModuleDeck.S.DrawRandomCard(Volt_ModuleDeck.S.GetRandomModuleType());

            List<int> tmpVpTiles = new List<int>();
            foreach (var item in Volt_ArenaSetter.S.GetTileArray())
            {
                if (item.pTileType == Volt_Tile.TileType.vpSpace)
                {
                    tmpVpTiles.Add(item.tileIndex);
                }
            }
            List<int> tmpKitTiles = new List<int>();
            foreach (var item in Volt_ArenaSetter.S.GetTileArray())
            {
                if (item.pTileType == Volt_Tile.TileType.workShop)
                {
                    tmpKitTiles.Add(item.tileIndex);
                }
            }
            List<int> tmpModuleTiles = new List<int>();
            foreach (var item in Volt_ArenaSetter.S.GetTileArray())
            {
                if (item.pTileType != Volt_Tile.TileType.pits &&
                    item.pTileType != Volt_Tile.TileType.startingSpace &&
                    item.pTileType != Volt_Tile.TileType.none)
                {
                    //Debug.Log(item + " is module place cadidate tile");
                    tmpModuleTiles.Add(item.tileIndex);
                }
            }
            int randomVpIdx = Random.Range(0, tmpVpTiles.Count);
            int randomKitIdx = Random.Range(0, tmpKitTiles.Count);
            int randomModuleIdx = Random.Range(0, tmpModuleTiles.Count);

            //Debug.Log(tmpModuleTiles[randomModuleIdx] + " is selected");

            ItemSetupStart(tmpVpTiles[randomVpIdx], tmpKitTiles[randomKitIdx], tmpModuleTiles[randomModuleIdx], cardBase.card);


        }
        else
        {
            yield return new WaitUntil(() => IsAllResolutionEnd() && IsAllRobotIdleState());
            SendCurrentGameData();
        }
        
    }
    public void RenewNeedSyncTile()
    {
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            if((item.isHaveRepairKit || 
                item.IsHaveTimeBomb ||
                item.ModuleInstance ||
                item.VpCoinInstance ||
                item.IsOnVoltage) && !Volt_ArenaSetter.S.needSyncTiles.Contains(item))
            {
                Volt_ArenaSetter.S.needSyncTiles.Add(item);
            }
        }
    }
    public void SendCurrentGameData()
    {
        if (isGameOverWaiting) return;

        if (!IsTutorialMode || !IsTrainingMode)
        {
            if (CommunicationInfo.IsMobileActive)
            {
                RenewNeedSyncTile();

                GameStateDataSet.SendTotalTurnOverGameDatas();
            }
        }
        Volt_GMUI.S.guidePanel.ShowGuideTextMSG(GuideMSGType.Synchronization, false, Application.systemLanguage);
        curPhase = Phase.synchronization;
    }
    public void TileDataSynchronizationStart()
    {
        //Debug.Log("TileDataSynchronizationStart");
        foreach (var item in Volt_ArenaSetter.S.GetTileArray())
        {
            item.DestroyModule();
            item.CoinDestroy();
            item.KitDestroy();
            //item.SetOffVoltage();
        }
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.moduleCardExcutor.DestroyCardAll();
            if (item.TimeBombInstance)
            {
                //Debug.Log($"{item.playerInfo.playerNumber} 기존 폭탄 존재");
                item.TimeBombInstance.GetComponent<Volt_TimeBomb>().Destroy();
            }
        }

        StartCoroutine(TileSynchronization());
    }

    IEnumerator TileSynchronization()
    {
        yield return new WaitUntil(() => IsAllRobotIdleState() && RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
        yield return new WaitUntil(() => curPhase == Phase.synchronization || curPhase == Phase.waitSync);
        for (int i = 0; i < Volt_ArenaSetter.S.GetTileArray().Length; i++)
        {
            TileData data = tileDataForSync.Dequeue();
            Volt_Tile syncTile = Volt_ArenaSetter.S.GetTileByIdx(data.tileIdx);
            syncTile.Synchronization(data);
        }
        isTileDataSyncDone = true;
        //Debug.Log("Tile Sync Done");

        //yield return new WaitUntil(() => Volt_PlayerManager.S.I.isNeedSynchronization == false);
        //isSynchronizationDone = false;
        //yield return new WaitForSeconds(3f);

    }
    public void ElseDataSynchronizationStart(int roundNumber, int armageddonCount, int armageddonPlayer, int remainVpSetupCount)
    {
        isElseDataSyncDone = false;
        StartCoroutine(ElseDataSynchronization(roundNumber, armageddonCount, armageddonPlayer, remainVpSetupCount));
    }
    IEnumerator ElseDataSynchronization(int roundNumber, int armageddonCount, int armageddonPlayer, int remainVpSetupCount)
    {
        yield return new WaitUntil(() => curPhase == Phase.synchronization || curPhase == Phase.waitSync);
        yield return new WaitUntil(() => isTileDataSyncDone);

        for (int i = 0; i < Volt_PlayerManager.S.GetPlayers().Count; i++)
        {
            PlayerData playerData = playerDataForSync.Dequeue();
            RobotData robotData = robotDataForSync.Dequeue();
            Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerData.playerNumber);
            player.Synchronization(playerData);

            if (playerData.isRobotAlive)
            {
                player.playerCamRoot.SetSaturationDown(false);
                if (player.GetRobot())
                {
                    player.GetRobot().SynchronizationStart(robotData);
                }
                else
                {
                    //TODO: Managers.Resources를 통해 로봇 생성
                    Managers.Resource.InstantiateAsync($"Robots/{player.RobotType}/{player.RobotType}_{player.skinType}.prefab",
                        (result) =>
                        {
                            player.playerRobot = result.Result;
                            Volt_Robot robot = player.playerRobot.GetOrAddComponent<Volt_Robot>();
                            robot.Init(player, null);
                            //Volt_PrefabFactory.S.CreateRobot(playerData.playerNumber, player.RobotType, robotData.skinType).gameObject;
                            player.GetRobot().SynchronizationStart(robotData);
                        });

                    
                }
            }
            else
            {
                if (player.GetRobot())
                    player.GetRobot().ForcedKillRobot();
            }
            if (player.IsMobileActivated)
                player.isNeedSynchronization = false;
            else
                player.isNeedSynchronization = true;
        }
        
        Volt_GMUI.S.Synchronization(roundNumber);
        remainRoundCountToVpSetup = remainVpSetupCount;

        if (mapType == MapType.Ruhrgebiet)
        {
            //SuddenDeathOn 시켜야함.
            if (roundNumber >= 10 && roundNumber < 13)
            {
                isOnSuddenDeath = true;
                foreach (var item in Volt_ArenaSetter.S.fallTiles1)
                {
                    item.Fall();
                }
                foreach (var item in Volt_PlayerManager.S.GetPlayers())
                {
                    Volt_PlayerManager.S.RuhrgebietChangeStartingTiles(item.playerNumber,1);
                }
            }
            else if (roundNumber >= 13)
            {
                isOnSuddenDeath = true;
                foreach (var item in Volt_ArenaSetter.S.fallTiles2)
                {
                    item.Fall();
                }

                foreach (var item in Volt_PlayerManager.S.GetPlayers())
                {
                    Volt_PlayerManager.S.RuhrgebietChangeStartingTiles(item.playerNumber,2);
                }
            }
        }

        yield return StartCoroutine(Volt_SynchronizationHandler.S.DelayedSync(armageddonCount, armageddonPlayer));
        yield return new WaitUntil(() => Volt_SynchronizationHandler.S.isSyncDone);
        Volt_SynchronizationHandler.S.isSyncDone = false;

        //Debug.Log("PostProcess Sync done");

        optionIdx = 0;
        if (!CommunicationInfo.IsMobileActive)
        {
            CommunicationInfo.IsMobileActive = true;
            PacketTransmission.SendTotalTurnOverPacket(null, null, null);
        }
        CommunicationInfo.IsMobileActive = true;
        Volt_PlayerManager.S.I.isNeedSynchronization = false;
        Volt_GMUI.S.syncWaitblockPanel.SetActive(false);
        

        //Debug.Log("Else Sync Done");
        isElseDataSyncDone = true;

        TotalTurnOverReturnWaitStart();
    }


    public void TotalTurnOverReturnWaitStart()
    {
        StartCoroutine(TotalTurnOverReturnWait());
    }
    IEnumerator TotalTurnOverReturnWait()
    {
        yield return new WaitUntil(() => isElseDataSyncDone && isTileDataSyncDone);
        isTileDataSyncDone = false;
        isElseDataSyncDone = false;
        Volt_ArenaSetter.S.needSyncTiles.Clear();

        ModuleType moduleType = Volt_ModuleDeck.S.GetRandomModuleType();
        Volt_ModuleCardBase cardBase = Volt_ModuleDeck.S.DrawRandomCard(moduleType);

        //Debug.Log("All Sync Done");
        for (int i = 0; i < 4; i++)
        {
            Volt_PlayerManager.S.ReturnPlayerKey(i + 1);
        }
        PacketTransmission.SendFieldReadyCompletionPacket(cardBase.card);
        yield break;
    }

    //=============================================================================================================
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));
        return result;
    }
    public void DoAllKillbotsDetection()
    {
        foreach (Volt_Robot robot in Volt_ArenaSetter.S.robotsInArena)
        {
            if (robot.playerInfo.PlayerType == PlayerType.AI)
            {
                Volt_Robot killbot = robot.gameObject.GetComponent<Volt_Robot>();
                killbot.DetectRobot();
            }
        }
    }
    
    public void OnReceivedKillbotBehaviourPacket(int playerNumber, Volt_RobotBehavior behaviour)
    {
        Volt_PlayerInfo pInfo = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        Volt_Robot killbot = pInfo.playerRobot.GetComponent<Volt_Robot>();
        killbot.fsm.behavior = behaviour;
    }
    
    private void SortedRobotList()
    {
        List<Volt_RobotBehavior> distinctBehaviours = new List<Volt_RobotBehavior>();
        List<int> behaviourKeys = new List<int>();

        foreach (var item in tmpBehaviours)
        {
            if (!behaviourKeys.Contains(item.PlayerNumber))
            {
                behaviourKeys.Add(item.PlayerNumber);
                distinctBehaviours.Add(item);
            }
        }

        for (int i = 0; i < distinctBehaviours.Count; i++)
        {
            int idx = i;
            // Behaviour type과 BehaviourPoints를 기준으로 행동 우선순위가 가장 높은 킬봇의 행동정보를 저장한다.
            // Behaviour type의 우선 순위는 공격이 이동보다 낮고, 이동은 정지 보다 우선순위가 낮다.(Stay > Move > Attack)
            // BehaviourPoints는 값이 낮을수록 우선 순위가 높다.
            Volt_RobotBehavior behaviour1 = distinctBehaviours[idx];
            // 최우선순위 번호
            int topPriorityNumber = behaviour1.BehaviorPoints;
            // 만약 행동 타입이 공격일 경우 추가로 6를 더한다. 
            //topPriorityNumber += behaviour1.BehaviourType == BehaviourType.Move ? 0 : 6; //201102 일짜 기준으로 이동과 공격의 우선정도를 같음으로 설정함에따라 해당 행을 주석으로 처리함.

            for (int j = i; j < distinctBehaviours.Count; j++)
            {
                // 비교대상이 되는 킬봇의 행동정보
                Volt_RobotBehavior behaviour2 = distinctBehaviours[j];
                if (behaviour2.BehaviourType == BehaviourType.None)
                    idx = j;

                int priorityNumber = behaviour2.BehaviorPoints;
                //priorityNumber += behaviour2.BehaviourType == BehaviourType.Move ? 0 : 6; //201102 일짜 기준으로 이동과 공격의 우선정도를 같음으로 설정함에따라 해당 행을 주석으로 처리함.

                // 계산된 priorityNumber의 값이 낮을수록 우선순위가 높다.
                // 비교대상의 우선순위 번호가 더 클경우
                if (priorityNumber > topPriorityNumber)
                    continue;
                // 비교대상의 우선순위 번호가 더 작을경우
                else if (priorityNumber < topPriorityNumber)
                {
                    topPriorityNumber = priorityNumber;
                    idx = j;
                }
                // 비교대상의 우선순위 번호가 같은경우
                else
                {
                    // 비교대상의 순서번호가 더 작을경우
                    if (behaviour2.OrderNumber < behaviour1.OrderNumber)
                    {
                        idx = j;
                    }
                }
            }
            if (i == idx)
                continue;
            Volt_RobotBehavior tbehaviour = distinctBehaviours[i];
            distinctBehaviours[i] = distinctBehaviours[idx];
            distinctBehaviours[idx] = tbehaviour;
        }
        for (int i = distinctBehaviours.Count-1; i >= 0; i--)
        {
            behaviourStack.Push(distinctBehaviours[i]);
        }
    }

    
    /// <summary>
    /// delayTime만큼 대기 후 methodName의 함수를 호출한다.
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="delayTime"></param>
    public void CallFunctionDelayed(string methodName, float delayTime)
    {
        Invoke(methodName, delayTime);
    }

    public void OnAmargeddonExploded()
    {
        isPlayAmargeddon = false;
    }
    public void GameOver(int winner)
    {
        curPhase = Phase.gameOver;

        isOnSuddenDeath = false;
        isGameOverWaiting = true;
        //StopAllCoroutines();
        if (IsTutorialMode)
        {
            PlayerPrefs.SetInt("Volt_TutorialDone", 1); 
        }
        Volt_GMUI.S.guidePanel.HideGuideText();
        foreach (var item in Volt_GMUI.S.playerPanels)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            item.panel.gameObject.SetActive(false);
        }

        StartCoroutine(GameOverCoroutine(winner));
    }
    IEnumerator GameOverCoroutine(int winner)
    {
        yield return new WaitUntil(() => isLocalSimulationDone);
        yield return new WaitUntil(() => !Volt_PlayerManager.S.I.playerCam.GetComponent<Volt_CameraEffect>().isShakePlaying);
        yield return new WaitUntil(() => !Volt_PlayerManager.S.I.playerCamRoot.isMoving);

        CommunicationWaitQueue.Instance.ResetOrder();
        CommunicationInfo.IsBoardGamePlaying = false;
        Volt_PlayerData.instance.NeedReConnection = false;

        Volt_Robot winnerRobot = null;
        if (Volt_PlayerManager.S.GetPlayerByPlayerNumber(winner).playerRobot)
        {
            winnerRobot = Volt_PlayerManager.S.GetPlayerByPlayerNumber(winner).playerRobot.GetComponent<Volt_Robot>();
        }

        if (winnerRobot != null)
        {
            foreach (var item in Volt_ArenaSetter.S.robotsInArena)
            {
                if (item)
                {
                    item.playerInfo.playerCamRoot.GameOver(item.gameObject);
                    if (item == winnerRobot.GetComponent<Volt_Robot>())
                    {
                        item.fsm.Animator.CrossFade("Win", 0.1f);
                    }
                    else
                    {
                        item.fsm.Animator.CrossFade("Lose", 0.1f);
                    }
                }
            }
        }

        noticeText.text = "Game Over!";
 
        if (winner == Volt_PlayerManager.S.I.playerNumber)
        {
            //DB 승리 카운트 증가
            Volt_GamePlayData.S.Rank = 1;
            Volt_GMUI.S.guidePanel.ShowSpriteAnimationMSG(GuideMSGType.Victory, false);
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_SE/01Victory.wav",
                (result) =>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                });
            
            if (Volt_PlayerManager.S.I.playerRobot)
            {
                Volt_Robot robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>();
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/SFx_200703/RobotVictory.wav",
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });
            }
        }
        else
        {
            Volt_GamePlayData.S.Rank = 2;
            Volt_GMUI.S.guidePanel.ShowSpriteAnimationMSG(GuideMSGType.Defeat, false);
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_SE/02Lose.wav",
                (result) =>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                });
            
            if (Volt_PlayerManager.S.I.playerRobot)
            {
                Volt_Robot robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>();
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/SFx_200703/RobotLose.wav",
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });
            }
        }
        
        yield return new WaitForSeconds(6f);

        Managers.Scene.LoadSceneAsync(Define.Scene.ResultScene);
        //Volt_LoadingSceneManager.S.RequestLoadScene("ResultScene 1");
        
    }
    bool IsAllResolutionEnd()
    {
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            if (!item.isResolutionDone)
            {
                //Debug.Log(item.name + "Resolution is Not Done");
                return false;
            }
        }
        return true;
    }
    bool IsAnyMonsterActive(List<Volt_Tile> monsterTile)
    {
        foreach (var item in monsterTile)
        {
            Volt_PitMonster monster = item.pitMonster.GetComponent<Volt_PitMonster>();
            if (monster.isMonsterActive || monster.isSearchActive)
                return true;
        }
        return false;
    }
    public void SendAchievementProgressPacketBeforeGameOver(int winner)
    {
        //------------------------------------------승리시 업적 판단 winner가 본인이면 진행하면 될 듯.
        if (Volt_PlayerManager.S.I.playerNumber == winner)
        {
            foreach (var item in Volt_GamePlayData.S.usedModuleCardCounts[winner])
            {
                if (item.Key == Card.DUMMYGEAR &&
                    item.Value > 0)
                {
                    //DB 2000025 업적진행
                    PacketTransmission.SendAchievementProgressPacket(2000025, winner,true);
                }
            }
            if (Volt_GamePlayData.S.IsKillAllTheOtherRobotsInThisGame(winner))
            {
                //DB 2000015 업적진행
                PacketTransmission.SendAchievementProgressPacket(2000015, winner,true);
            }
            if (Volt_GamePlayData.S.IsRobotEverDied(winner))
            {
                //DB 2000014 업적진행
                PacketTransmission.SendAchievementProgressPacket(2000014, winner,true);
            }
            if (Volt_GamePlayData.S.IsRobotKillAnyone(winner))
            {
                //DB 2000013 업적진행
                PacketTransmission.SendAchievementProgressPacket(2000013, winner,true);
            }
        }
        //----------------------------------------------
    }
}


