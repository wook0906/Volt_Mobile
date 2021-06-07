using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlinkType
{
    None, Tactic, Attack 
}
public class Volt_Tile : MonoBehaviour
{
    public int tileIndex;
    public static Color damageColor = Color.red;
    public static Color tacticColor = new Color(1f, 0.753f, 0f);
    public struct TilePosInArena
    {
        public int x, y;

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append($"Board (X:{x.ToString()}, Y:{y.ToString()})");
            return builder.ToString();
        }
    }
    public TilePosInArena tilePosInArray;
    public enum TileType
    {
        none, normal, voltageSpace, pits, startingSpace, vpSpace, workShop
    }
    [Header("Set in Scripts")]
    
    public Card tileInModuleType;

    GameObject moduleInstance;
    public GameObject ModuleInstance
    {
        get { return moduleInstance; }
        set
        {
            if (!Volt_ArenaSetter.S.needSyncTiles.Contains(this))
            {
                Volt_ArenaSetter.S.needSyncTiles.Add(this);
            }
            moduleInstance = value;
        }
    }
    GameObject vpCoinInstance;
    public GameObject VpCoinInstance
    {
        get { return vpCoinInstance; }
        set
        {
            if (!Volt_ArenaSetter.S.needSyncTiles.Contains(this))
            {
                Volt_ArenaSetter.S.needSyncTiles.Add(this);
            }
            vpCoinInstance = value;
        }
    }
    [SerializeField]
    GameObject repairKitInstance;
    public GameObject RepairKitInstance
    {
        get { return repairKitInstance; }
        set
        {
            if (!Volt_ArenaSetter.S.needSyncTiles.Contains(this))
            {
                Volt_ArenaSetter.S.needSyncTiles.Add(this);
            }
            repairKitInstance = value;
        }
    }
    private Volt_TimeBomb timebombInstance;
    public Volt_TimeBomb TimeBombInstance
    {
        get { return timebombInstance; }
        set
        {
            if (!Volt_ArenaSetter.S.needSyncTiles.Contains(this))
            {
                Volt_ArenaSetter.S.needSyncTiles.Add(this);
            }
            timebombInstance = value;
        }
    }
    public bool isHaveRepairKit = false;
    private bool blinkOn = false;
    public bool BlinkOn
    {
        get { return blinkOn; }
        set
        {
            blinkOn = value;
            if (blinkOn)
            {
                BlinkStart();
            }
        }
    }

    private bool isOnVoltage = false;
    public bool IsOnVoltage
    {
        get { return isOnVoltage; }
        set
        {
            if(!Volt_ArenaSetter.S.needSyncTiles.Contains(this))
                Volt_ArenaSetter.S.needSyncTiles.Add(this);
            isOnVoltage = value;
        }
    }

    public float blinkYieldTime = 0f;
    // The weighting value of the distance between tiles by type of tile.
    // normal: 1, voltageSpace: 1, pits: 4, startingSpace: 1, vpSpace: 1, workShop: 1, none: 99
    private int weightValue = 1;
    public int _weightValue { get { return weightValue; } set { weightValue = value; } }
    public bool isAnimationPlay = false;
    Animator animator;

    [SerializeField]
    Volt_Robot robotInTile;
    [SerializeField]
    Renderer rend;
    [SerializeField]
    Volt_Tile[] adjecentTiles;
    bool isHavetimeBomb;
    
    public bool IsHaveTimeBomb
    {
        get { return isHavetimeBomb; }
        set
        {
            if (!Volt_ArenaSetter.S.needSyncTiles.Contains(this))
            {
                Volt_ArenaSetter.S.needSyncTiles.Add(this);
            }
            isHavetimeBomb = value;
        }
    }
 
    [Header("Set in Inspector")]
    [SerializeField]
    private TileType tileType;
    public TileType pTileType
    {
        get { return tileType; }
        set
        {
            tileType = value;
        }
    }
    public GameObject voltageParticle;
    //Material voltageMaterial;
    public ParticleSystem responseParticle;
    float voltageOffsetSpeed = 3f;
    public float blinkDuration = 1.0f;
    [SerializeField]
    private Color blinkColor;
    private Color[] defaultColors;
    [SerializeField]
    public ArenaSection arenaSection;
    private BlinkType tileBlinkType;
    public GameObject pitMonster;


    public int numOfCoins;

    public Volt_Robot GetRobotInTile()
    {
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            if (item.fsm.isDead) continue;
            if (tilePosInArray.x == Volt_ArenaSetter.S.GetTilePos(item.transform.position).x && tilePosInArray.y == Volt_ArenaSetter.S.GetTilePos(item.transform.position).y)
            {
                return item;
            }
        }
        return null;
    }
    public List<Volt_Robot> GetRobotsInTile()
    {
        List<Volt_Robot> robots = new List<Volt_Robot>();
        foreach (var item in Volt_ArenaSetter.S.robotsInArena)
        {
            //if (item.IsDead) continue;
            if (tilePosInArray.x == Volt_ArenaSetter.S.GetTilePos(item.transform.position).x && tilePosInArray.y == Volt_ArenaSetter.S.GetTilePos(item.transform.position).y)
            {
                if(!robots.Contains(item))
                    robots.Add(item);
            }
        }
        return robots;
    }
    

    public void SetRobotInTile(Volt_Robot newRobot)
    {
        //Debug.Log("Call Set Robot in tile");
        if (newRobot == null)
        {
            robotInTile = null;
            return;
        }
        robotInTile = newRobot;

    }

    // Start is called before the first frame update
    void Start()
    {
        adjecentTiles = new Volt_Tile[8];
        if (GetComponent<Renderer>())
        {
            rend = GetComponent<Renderer>();
            defaultColors = new Color[GetComponent<MeshRenderer>().materials.Length];
            for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
            {
                defaultColors[i] = GetComponent<MeshRenderer>().materials[i].color;
            }
        }
        else if(GetComponentInChildren<Renderer>())
        {
            rend = GetComponentInChildren<Renderer>();
            defaultColors = new Color[GetComponentInChildren<MeshRenderer>().materials.Length];
            for (int i = 0; i < GetComponentInChildren<MeshRenderer>().materials.Length; i++)
            {
                defaultColors[i] = GetComponentInChildren<MeshRenderer>().materials[i].color;
            }
        }
        SetAdjecentTiles();
        
        blinkColor = Color.green;
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
        if (tileType == TileType.voltageSpace)
        {
            voltageParticle = transform.Find("VoltageParticle").gameObject;
            //voltageMaterial = voltageParticle.GetComponent<MeshRenderer>().material;
        }
        arenaSection = Volt_ArenaSetter.S.GetSection(this);
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (IsOnVoltage)
        //{
        //    voltageMaterial.mainTextureOffset += new Vector2(Time.fixedDeltaTime * voltageOffsetSpeed, 0);
        //}
        if (Volt_GameManager.S.pCurPhase == Phase.waitSync)
            BlinkOn = false;
    }

    public void SetBlinkOption(BlinkType blinkType, float blinkDuration)
    {
        //if (BlinkOn) return;
        if ((int)tileBlinkType > (int)blinkType) return;
        switch (blinkType)
        {
            case BlinkType.Tactic:
                blinkColor = tacticColor;
                break;
            case BlinkType.Attack:
                blinkColor = damageColor;
                break;
            default:
                break;
        }
        this.blinkDuration = blinkDuration;
        //blinkYieldTime = waitTime;
    }
    public void SetDefaultBlinkOption()
    {
        this.blinkColor = Color.green;
        this.blinkDuration = 1f;
        tileBlinkType = BlinkType.None;
        blinkYieldTime = 0f;
    }
    public void BlinkStart()
    {
        Debug.Log("Tile Blink Start");
        StartCoroutine(Blink());
    }
    IEnumerator Blink()
    {
        if (!rend)
        {
            yield break;
        }
        float lerp;

        while (BlinkOn)
        {
            lerp = Mathf.PingPong(Time.time, blinkDuration) / blinkDuration;
            for (int i = 0; i < rend.materials.Length; i++)
            {
                rend.materials[i].color = Color.Lerp(defaultColors[i], blinkColor, lerp);
            }
            //foreach (var item in rend.materials)
            //{
            //    item.color = Color.Lerp(defaultColor, blinkColor, lerp);
            //}
            yield return null;
        }
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].color = defaultColors[i];
        }
        //foreach (var item in rend.materials)
        //{
        //    item.color = defaultColor;
        //}


    }
    public IEnumerator SpawnBlink()
    {
        if (!responseParticle) yield break;
        responseParticle.gameObject.SetActive(true);
        responseParticle.Play();
        yield return new WaitUntil(() => Volt_PlayerManager.S.I.playerRobot != null);
        responseParticle.Stop();
        responseParticle.gameObject.SetActive(false);
    }
    

    bool IsThereAnyRobot(Volt_Tile tile)
    {
        if (robotInTile == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetAdjecentTiles()//앞뒤좌우 ↖↗↙↘ 순서
    {
        adjecentTiles[0] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + 1),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + 0));
        adjecentTiles[1] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + -1),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + 0));
        adjecentTiles[2] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + 0),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + -1));
        adjecentTiles[3] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + 0),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + 1));
        adjecentTiles[4] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + 1),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + -1));
        adjecentTiles[5] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + 1),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + 1));
        adjecentTiles[6] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + -1),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + -1));
        adjecentTiles[7] = Volt_ArenaSetter.S.GetTile((Volt_ArenaSetter.S.GetTilePos(this).x + -1),
                                                     (Volt_ArenaSetter.S.GetTilePos(this).y + 1));

        //List<Volt_Tile> tempTiles = new List<Volt_Tile>();
        //foreach (var tile in adjecentTiles)
        //{
        //    if (tile != null)
        //        tempTiles.Add(tile);
        //}
        //adjecentTiles.Clear();
        //adjecentTiles.AddRange(tempTiles);
    }
    public Volt_Tile[] GetAdjecentTiles()
    {

        return adjecentTiles;
    }
 


    public void SetVictoryPoint()
    {
        numOfCoins++;
        Volt_ArenaSetter.S.numOfVPSetupTile++;
        Volt_GameManager.S.remainRoundCountToVpSetup = 5;
        if (VpCoinInstance == null)
        {
            VpCoinInstance = Volt_PrefabFactory.S.PopObject(Define.Objects.Coin);//Instantiate(Volt_PrefabFactory.S.coinPrefab);
            if (VpCoinInstance != null)
            {
                Vector3 pos = this.transform.position;
                pos.y += Volt_ArenaSetter.S.tileScale.y + (Volt_ArenaSetter.S.tileScale.y * 0.1f) + (Volt_ArenaSetter.S.tileScale.y * 0.5f); ;
                VpCoinInstance.transform.position = pos;
            }
        }
        if(Volt_GameManager.S.pCurPhase == Phase.ItemSetup)
            Volt_PlayerManager.S.I.playerCamRoot.CameraMoveStart(VpCoinInstance);
        //if (numOfCoins >= 2)
        //{
        //    vpCoinInstance.GetComponentInChildren<TextMesh>().text = "x" + numOfCoins.ToString();
        //}
        //else
        //    Volt_ArenaSetter.S.numOfVPSetupTile++;
    }
    public void SetModule(Card cardType)
    {
        //프리팹생성, 배치한다.
        //tmp
        //if (tileType == TileType.pits) return;

        if (ModuleInstance == null)
        {
            ModuleInstance = Volt_PrefabFactory.S.PopObject(Define.Objects.ModuleBox);
            if (ModuleInstance == null)
                return;

            Volt_RandomBox moduleBox = ModuleInstance.GetComponent<Volt_RandomBox>();
            moduleBox.SpecificInit(cardType);
            tileInModuleType = moduleBox.moduleInBox.card; //Error! NullReference!!

            Vector3 pos = transform.position;
            pos.y += 1.05f * Volt_ArenaSetter.S.tileScale.y;
            ModuleInstance.transform.position = pos;

            Volt_ArenaSetter.S.numOfModule++;
        }
        else
        {
            DestroyModule();
            ModuleInstance = Volt_PrefabFactory.S.PopObject(Define.Objects.ModuleBox);
            if (moduleInstance == null)
            {
                Debug.LogError("PopObject failed is Null");
                return;
            }
                

            Volt_RandomBox moduleBox = ModuleInstance.GetComponent<Volt_RandomBox>();
            moduleBox.SpecificInit(cardType);
            tileInModuleType = moduleBox.moduleInBox.card;

            Vector3 pos = transform.position;
            pos.y += 1.05f * Volt_ArenaSetter.S.tileScale.y;
            ModuleInstance.transform.position = pos;

            Volt_ArenaSetter.S.numOfModule++;
        }
    }
    public void SetVoltage()
    {
        if (tileType != TileType.voltageSpace) return;
        IsOnVoltage = true;
        voltageParticle.gameObject.SetActive(true);
        Volt_ArenaSetter.S.numOfSetOnVoltageSpace++;
    }
    public void SetRepairKit()
    {
        //프리팹생성, 배치한다.
        //tmp
        if (RepairKitInstance == null)
        {
            RepairKitInstance = Volt_PrefabFactory.S.PopObject(Define.Objects.RepairKit);
            if (RepairKitInstance == null)
                return;

            Vector3 pos = transform.position;
            pos.y += 1.7f * Volt_ArenaSetter.S.tileScale.y;
            RepairKitInstance.transform.position = pos;
            //tmpEnd
        }
        isHaveRepairKit = true;
        Volt_ArenaSetter.S.numOfRepairKit++;
    }
    public void Synchronization(TileData data)
    {
        //Debug.LogWarning(tileIndex + " Sync Start");
        if (ModuleInstance)
            DestroyModule();
        if(data.tileInModule != Card.NONE)
            SetModule(data.tileInModule);


        if (VpCoinInstance)
            CoinDestroy();
        if(data.numOfVP != 0)
            SetVictoryPoint();


        if (RepairKitInstance)
            KitDestroy();
        if(data.isHaveRepairKit)
            SetRepairKit();

        if(TimeBombInstance)
            TimeBombInstance.Destroy();
        if (data.isHaveTimeBomb)
        {
            TimeBombInstance = Volt_PrefabFactory.S.PopObject(Define.Objects.TIMEBOMB).GetComponent<Volt_TimeBomb>();
            TimeBombInstance.StartWaitMoveCoroutine();
            if (TimeBombInstance != null)
            {
                IsHaveTimeBomb = true;
                TimeBombInstance.SetOwner(data.timeBombOwnerPlayerNumber);
                TimeBombInstance.count = data.timeBombCount;

                Vector3 pos = transform.position;
                pos.y += (1.1f * Volt_ArenaSetter.S.tileScale.y);
                TimeBombInstance.transform.position = pos;

                TimeBombInstance.GetComponent<Collider>().enabled = true;
                //Debug.Log(tileIndex + "Time bomb refresh " + pos);
            }
        }
        if (data.isOnVoltage)
            SetVoltage();
        else
            SetOffVoltage();
    }

    public void CountDownTimeBomb()
    {
        if(TimeBombInstance)
            TimeBombInstance.CountDown();
    }

    public void OnTouchBegin()
    {
        switch (Volt_GameManager.S.pCurPhase)
        {
            case Phase.none:
                break;
            case Phase.matching:
                break;
            case Phase.fieldSetup:
                break;
            case Phase.playerSetup:
                break;
            case Phase.ItemSetup:
                break;
            case Phase.robotSetup:
                break;
            case Phase.behavoiurSelect:
                break;
            case Phase.rangeSelect:

                if (BlinkOn)
                {
                    foreach (var item in Volt_ArenaSetter.S.tileObjList)
                    {
                        item.blinkColor = Color.green;
                    }
                }
                break;
            case Phase.simulation:
                break;
            case Phase.resolution:
                break;
            default:
                break;
        }
    }
    public void OnTouch()
    {

        switch (Volt_GameManager.S.pCurPhase)
        {
            case Phase.none:
                break;
            case Phase.matching:
                break;
            case Phase.fieldSetup:
                break;
            case Phase.playerSetup:
                break;
            case Phase.ItemSetup:
                break;
            case Phase.robotSetup:
                break;
            case Phase.behavoiurSelect:
                break;
            case Phase.rangeSelect:
                Volt_Robot robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>();
                if (BlinkOn)
                {
                    if (Volt_GameManager.S.IsTutorialMode)
                    {
                        if (Volt_TutorialManager.S)
                        {
                            if (Volt_TutorialManager.S.curContents.name == "WaitSelectRange")
                            {
                                foreach (var item in Volt_ArenaSetter.S.tileObjList)
                                {
                                    item.blinkColor = Color.green;
                                }
                                Volt_Tile[] tiles = Volt_ArenaSetter.S.GetTiles(Volt_ArenaSetter.S.GetTile(robot.transform.position), Volt_ArenaSetter.S.GetTileByIdx(30));
                                foreach (var item in tiles)
                                {
                                    if (Volt_GameManager.S.behaviour.BehaviourType == BehaviourType.Move)
                                        item.blinkColor = Color.blue;
                                    else
                                        item.blinkColor = Color.red;
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in Volt_ArenaSetter.S.tileObjList)
                            {
                                item.blinkColor = Color.green;
                            }
                            Volt_Tile[] tiles = Volt_ArenaSetter.S.GetTiles(Volt_ArenaSetter.S.GetTile(robot.transform.position), this);
                            foreach (var item in tiles)
                            {
                                if (Volt_GameManager.S.behaviour.BehaviourType == BehaviourType.Move)
                                    item.blinkColor = Color.blue;
                                else
                                    item.blinkColor = Color.red;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in Volt_ArenaSetter.S.tileObjList)
                        {
                            item.blinkColor = Color.green;
                        }
                        Volt_Tile[] tiles = Volt_ArenaSetter.S.GetTiles(Volt_ArenaSetter.S.GetTile(robot.transform.position), this);
                        foreach (var item in tiles)
                        {
                            if (Volt_GameManager.S.behaviour.BehaviourType == BehaviourType.Move)
                                item.blinkColor = Color.blue;
                            else
                                item.blinkColor = Color.red;
                        }
                    }
                }
                break;
            case Phase.simulation:
                break;
            case Phase.resolution:
                break;
            default:
                break;
        }
    }
    public void OnTouchEnd()
    {

        switch (Volt_GameManager.S.pCurPhase)
        {
            case Phase.none:
                break;
            case Phase.matching:
                break;
            case Phase.fieldSetup:
                break;
            case Phase.playerSetup:
                break;
            case Phase.ItemSetup:
                break;
            case Phase.robotSetup:
                if (!BlinkOn)
                    return;
                if (Volt_PlayerManager.S.I.startingTiles.Contains(this) && Volt_PlayerManager.S.I.playerRobot == null)
                {
                    if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
                    {
                        if (Volt_TutorialManager.S && Volt_GameManager.S.IsTutorialMode)
                        {
                            if ((Volt_TutorialManager.S.curContents.name == "PlaceRobotWait" && Volt_TutorialManager.S.isShowingTutorial))
                            {
                                Volt_GameManager.S.PlayerRobotPlaceRequest(Volt_PlayerManager.S.I.playerNumber, 0, 3);
                            }
                        }
                        else
                        {
                            if (Volt_GameManager.S.useCustomPosition)
                                Volt_GameManager.S.PlayerRobotPlaceRequest(Volt_PlayerManager.S.I.playerNumber, (int)Volt_GameManager.S.playerInitPoints[0].x, (int)Volt_GameManager.S.playerInitPoints[0].y);
                            else
                                Volt_GameManager.S.PlayerRobotPlaceRequest(Volt_PlayerManager.S.I.playerNumber, Volt_ArenaSetter.S.GetTilePos(this.transform.position).x, Volt_ArenaSetter.S.GetTilePos(this.transform.position).y);
                        }
                    }
                    else
                    {
                        Volt_PlayerManager.S.SendCharacterPositionPacket(Volt_PlayerManager.S.I.playerNumber, tilePosInArray.x, tilePosInArray.y);
                    }
                }
                break;
            case Phase.behavoiurSelect:
                break;
            case Phase.rangeSelect:
                if (!BlinkOn)
                {
                    foreach (var item in Volt_ArenaSetter.S.tileObjList)
                    {
                        item.blinkColor = Color.green;
                    }
                }
                else
                {
                    if (Volt_GameManager.S.IsTutorialMode)
                    {
                        //Volt_GameManager.S.SelectRangeDoneCallback(this);
                        if (Volt_TutorialManager.S)
                        {
                            if ((Volt_TutorialManager.S.curContents.name == "WaitSelectRange" && Volt_TutorialManager.S.isShowingTutorial))
                            {
                                Volt_GameManager.S.SelectRangeDoneCallback(Volt_ArenaSetter.S.GetTileByIdx(30));
                            }
                        }
                        else
                        {
                            Volt_GameManager.S.SelectRangeDoneCallback(this);
                        }
                    }
                    else
                    {
                        if (!Volt_PlayerManager.S.isCanGetPlayerKey(Volt_PlayerManager.S.I.playerNumber))
                            return;
                        Volt_GameManager.S.SelectRangeDoneCallback(this);
                    }
                }
                break;
            case Phase.simulation:
                break;
            case Phase.resolution:
                break;
            default:
                break;
        }
        
    }

    public void DestroyModule()
    {
        if (!ModuleInstance) return;
        ModuleInstance.GetComponent<Volt_RandomBox>().DestroyModule(true);
        Debug.Log($"{tileIndex} + {tileInModuleType.ToString()} + Destroyed");
        tileInModuleType = Card.NONE;
        ModuleInstance = null;
    }

    public void RobotAcquireKit(Volt_Robot robot)
    {
        robot.HitCount--;
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/Field/Get_Repair.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });

        GameObject particle = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_GetRepairItem);
        Vector3 pos = transform.position;
        pos.y += (1.1f *Volt_ArenaSetter.S.tileScale.y);
        particle.transform.position = pos;

        KitDestroy();
        //Debug.Log("RobotAcquireKit");
    }
    public void KitDestroy()
    {
        if (!RepairKitInstance) return;
        isHaveRepairKit = false;
        //Destroy(RepairKitInstance);
        Volt_PrefabFactory.S.PushObject(RepairKitInstance.GetComponent<Poolable>());
        RepairKitInstance = null;
        Volt_ArenaSetter.S.numOfRepairKit--;
        //Debug.Log("KitDestroy");
    }

    public void RobotAcquireVP(int playerNumber)
    {
        numOfCoins = 0;
        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        player.VictoryPoint++;
        PacketTransmission.SendVictoryPointPacket(player.playerNumber, player.VictoryPoint);
        
        Volt_GMUI.S.Create3DMsg(MSG3DEventType.PointUp, player);

        GameObject particle = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_GetVictoryCoinItem);//Instantiate(Volt_PrefabFactory.S.vpGetParticle);
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/GetPoint.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        Vector3 pos = transform.position;
        pos.y += (1.1f * Volt_ArenaSetter.S.tileScale.y) * 1.5f;
        particle.transform.position = pos;

        CoinDestroy();
    }
    public void CoinDestroy()
    {
        if (!VpCoinInstance) return;
        Volt_PrefabFactory.S.PushObject(vpCoinInstance.GetComponent<Poolable>());
        //Destroy(VpCoinInstance);
        VpCoinInstance = null;
        numOfCoins = 0;
        Volt_ArenaSetter.S.numOfVPSetupTile--;
    }
    public void SetOffVoltage()
    {
        if (tileType != TileType.voltageSpace) return;
        //Debug.Log(this.name + " Voltage Off");
        IsOnVoltage = false;
        voltageParticle.gameObject.SetActive(false);
        Volt_ArenaSetter.S.numOfSetOnVoltageSpace--;
    }
    public void ChangeTileType(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.none:
                break;
            case TileType.normal:
                break;
            case TileType.voltageSpace:
                break;
            case TileType.pits:
                this.pTileType = TileType.pits;
                transform.Find("Pit").gameObject.SetActive(true);
                break;
            case TileType.startingSpace:
                break;
            case TileType.vpSpace:
                break;
            case TileType.workShop:
                break;
            default:
                break;
        }

    }
    public void Fall()
    {
        isAnimationPlay = true;
        if (animator)
        {
            animator.Play("TileFall");
        }
    }
    public void TileFallEffectPlay()
    {
        GameObject effect = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_FallTile);//Instantiate(Volt_PrefabFactory.S.effect_TileFall);
        Vector3 pos = transform.position;
        pos.y += 1f;
        effect.transform.position = pos;

        if (ModuleInstance)
            DestroyModule();
        if (TimeBombInstance)
            TimeBombInstance.GetComponent<Volt_TimeBomb>().Destroy();
        ChangeTileType(TileType.pits);
    }
    public void AnimationDoneCallback()
    {
        isAnimationPlay = false;
    }
    public Vector3 GetRobotDirectionInRandomAdjecentTiles(int option)
    {
        Volt_Tile[] tmpTiles = new Volt_Tile[adjecentTiles.Length];
        int nullCnt = 0;
        for (int i = 0; i < adjecentTiles.Length; i++)
        {
            if (adjecentTiles[i].GetRobotInTile() != null)
            {
                tmpTiles[i] = adjecentTiles[i];
                //Debug.Log($"Candidate : {adjecentTiles[i]}");
            }
            else 
            {
                nullCnt++;
                continue;
            }
        }
        //Debug.Log(nullCnt + ", " + adjecentTiles.Length);
        if (nullCnt == adjecentTiles.Length)
            return Vector3.zero;

        int randomIndex = 0;
        int addToOption = 0;
        while (true)
        {
            randomIndex = (option + addToOption) % tmpTiles.Length;
            Debug.Log(randomIndex);
            if (tmpTiles[randomIndex] != null)
                break;
            addToOption++;
        }
        //Debug.Log($"{tmpTiles[randomIndex]} 당첨");
        Vector3 dir = (tmpTiles[randomIndex].transform.position - transform.position).normalized;
        return dir;
    }
    public void SetTimeBomb(Volt_TimeBomb timeBomb)
    {
        StartCoroutine(WaitSetTimeBomb(timeBomb));
    }

    IEnumerator WaitSetTimeBomb(Volt_TimeBomb newTimeBomb)
    {
        if (this.TimeBombInstance != null)
        {
            this.TimeBombInstance.Explosion();
        }

        yield return new WaitUntil(() => this.TimeBombInstance == null);
        this.TimeBombInstance = newTimeBomb;
        IsHaveTimeBomb = true;
        newTimeBomb.GetComponent<Collider>().enabled = true;
    }
}
