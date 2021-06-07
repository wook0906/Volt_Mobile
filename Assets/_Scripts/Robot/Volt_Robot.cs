using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Robot : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Volt_PlayerInfo playerInfo; //나중에는 생성될때 알아서...
    public Volt_Tile standingTile;
    public Volt_ModuleCardExcutor moduleCardExcutor;
    public Volt_RobotAudioController audioController;
    public Volt_RobotPanel panel;

    [Header("Set in Script")]
    [SerializeField]
    private PushType pushType;
    public  PushType PushType { get { return pushType; } set { pushType = value; } }
    [SerializeField]
    protected Volt_RobotAddOnsManager addOnsMgr;
    public Volt_RobotAddOnsManager AddOnsMgr { get { return addOnsMgr; } }
    [SerializeField]
    private Volt_TimeBomb timeBombInstance;
    public Volt_TimeBomb TimeBombInstance
    {
        get { return timeBombInstance; }
        set { timeBombInstance = value; }
    }
    [SerializeField]
    private int hitCount = 0; //데미지토큰 반영
    public int HitCount
    {
        get
        {
            return hitCount;
        }
        set
        {
            // 데미지 없음~
            if (hitCount == value)
                return;

            hitCount = value;
            if (hitCount > 3)
            {
                hitCount = 3;
                
            }
            else if (hitCount < 0)
            {
                hitCount = 0;
            }

            if(hitCount == 3 || hitCount == 0)
            {
                if (spark1 != null)
                {
                    Volt_PrefabFactory.S.PushEffect(spark1.GetComponent<Poolable>());
                    spark1 = null;
                }
                if (spark2 != null)
                {
                    Volt_PrefabFactory.S.PushEffect(spark2.GetComponent<Poolable>());
                    spark2 = null;
                }
            }

            if (hitCount == 1)
            {
                if(spark2 != null)
                {
                    Volt_PrefabFactory.S.PushEffect(spark2.GetComponent<Poolable>());
                    spark2 = null;
                }
                spark1 = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_RobotSpark1);
                spark1.transform.position = GetCenterPosition();
                spark1.transform.SetParent(transform);
            }
            else if(hitCount == 2)
            {
                if (spark1 != null)
                {
                    Volt_PrefabFactory.S.PushEffect(spark1.GetComponent<Poolable>());
                    spark1 = null;
                }
                spark2 = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_RobotSpark2);
                spark2.transform.position = GetCenterPosition();
                spark2.transform.SetParent(transform);
            }
            panel.HpRenew(hitCount);
            
        }
    }
    public bool IsTimeBombOn { get; set; }
    public int TimeBombCount { get; set; }

    public bool isInitAddOnMgr = false;
    public bool isResolutionDone = true;

    FXV.FXVShield shieldEffectObj;
    public int lastAttackPlayer;

    private GameObject spark1 = null;
    private GameObject spark2 = null;

    public void SynchronizationStart(RobotData data)
    {
        playerInfo = Volt_PlayerManager.S.GetPlayerByPlayerNumber(data.ownerPlayerNumber);
        playerInfo.playerRobot = this.gameObject;
        StartCoroutine(Synchronization(data));
    }

    public IEnumerator Synchronization(RobotData data)
    {
        playerInfo = Volt_PlayerManager.S.GetPlayerByPlayerNumber(data.ownerPlayerNumber);
        playerInfo.playerRobot = this.gameObject;

        //Debug.Log(data.ownerPlayerNumber + " robot Sync Start");
        yield return new WaitUntil(() => isInitAddOnMgr);
        standingTile = Volt_ArenaSetter.S.GetTileByIdx(data.tileIdx);
        Vector3 pos = standingTile.transform.position;
        pos.y += (1.1f * Volt_ArenaSetter.S.tileScale.y);
        this.transform.position = pos;
        panel.IDSet(data.ownerPlayerNumber);
        //Init

        

        //moduleCardExcutor.DestroyCardAll();
        if (data.slot1Module != Card.NONE)
            moduleCardExcutor.ForcedSetModule(0, Volt_ModuleDeck.S.GetModuleFromDeck(data.slot1Module), data.module1State);
        if (data.slot2Module != Card.NONE)
            moduleCardExcutor.ForcedSetModule(1, Volt_ModuleDeck.S.GetModuleFromDeck(data.slot2Module), data.module2State);

        IsTimeBombOn = data.isHaveTimeBomb;

        
        if (data.isHaveTimeBomb)
        {
            //Debug.Log(playerInfo.playerNumber + " 는 폭탄을 가졌어...!");

            Volt_TimeBomb newTimeBomb = Volt_PrefabFactory.S.PopObject(Define.Objects.TIMEBOMB).GetComponent<Volt_TimeBomb>();//Instantiate(Volt_PrefabFactory.S.timeBombPrefab).GetComponent<Volt_TimeBomb>();
            newTimeBomb.StartWaitMoveCoroutine();
            if (newTimeBomb != null)
            {
                newTimeBomb.SetOwner(data.timeBombOwnerNumber);
                newTimeBomb.count = data.timeBombCount;
                SetTimeBomb(newTimeBomb);
            }
        }

        HitCount = data.hitCount;
        addOnsMgr.ShieldPoints = data.shieldPoint;
        transform.rotation = Quaternion.LookRotation(Volt_GameManager.StringToVector3(data.lookDirection), Vector3.up);

        GetComponent<Collider>().enabled = true;
        //Debug.Log(data.ownerPlayerNumber + " robot Sync End");
    }

    public void Init(Volt_PlayerInfo newPlayerInfo, Volt_Tile startTile)
    {
        moduleCardExcutor = gameObject.GetOrAddComponent<Volt_ModuleCardExcutor>();
        fsm = gameObject.GetOrAddComponent<StateMachine>();
        audioController = gameObject.GetOrAddComponent<Volt_RobotAudioController>();
        gameObject.GetOrAddComponent<Volt_WinMuzzleEffect>().Init(newPlayerInfo.RobotType);

        // StatusBar 생성
        
        Transform panelRoot = Util.FindChild<Transform>(gameObject, "StatusBar_Root", true);
        panel = Volt_PrefabFactory.S.Instantiate(Volt_PrefabFactory.S.robotPanel, panelRoot).GetComponent<Volt_RobotPanel>();

        Volt_ArenaSetter.S.robotsInArena.Add(this);
        addOnsMgr = new Volt_RobotAddOnsManager(moduleCardExcutor, this);
        
        this.playerInfo = newPlayerInfo;
        this.playerInfo.isRobotAlive = true;
        standingTile = startTile;

        if (startTile == null)
            transform.position = Vector3.positiveInfinity;
        else
        {
            Vector3 pos = startTile.transform.position;
            pos.y += (1.11f * Volt_ArenaSetter.S.tileScale.y);
            this.transform.position = pos;
        }

        switch (newPlayerInfo.playerNumber)
        {
            case 1:
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0f, 90, 0f);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 4:
                transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;
            default:
                break;
        }
        panel.IDSet(playerInfo.playerNumber);
        if (playerInfo.PlayerType == PlayerType.AI)
        {
            if (Volt_GameManager.S.IsTutorialMode)
                HitCount = 2;

            GameObject spawnEffect = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_KillboySpawn);//Instantiate(Volt_PrefabFactory.S.effect_RobotSpawn[4]);
            spawnEffect.transform.position = this.transform.position;
            spawnEffect.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Define.Effects effectType = (Define.Effects)Enum.Parse(typeof(Define.Effects),
                $"VFX_{playerInfo.RobotType}Spawn");

            GameObject spawnEffect = Volt_PrefabFactory.S.PopEffect(effectType);//Instantiate(Volt_PrefabFactory.S.effect_RobotSpawn[(int)playerInfo.RobotType]);
            spawnEffect.transform.position = this.transform.position;
            spawnEffect.GetComponent<ParticleSystem>().Play();
        }
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/character_SPAWN.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        GetComponent<Collider>().enabled = true;
        //Debug.Log(playerInfo.playerNumber + " robot Initialized");
    }

    public void ShieldEffectOn(bool on)
    {
        if (on)
        {
            if (shieldEffectObj == null)
            {
                //Debug.Log("쉴드 이펙트 획득");
                shieldEffectObj = Volt_ParticleManager.Instance.GetShieldEffect();
                shieldEffectObj.transform.SetParent(transform);
                Vector3 pos = transform.position;
                pos.y += 1f;
                shieldEffectObj.transform.position = pos;
            }
        }
        else
        {
            if (shieldEffectObj != null)
            {
                //Debug.Log("쉴드 이펙트 반납");
                shieldEffectObj.transform.SetParent(null);
                shieldEffectObj.transform.position = new Vector3(1000f, 1000f, 100f);
                Volt_ParticleManager.Instance.ReturnShieldEffect(shieldEffectObj);
                shieldEffectObj = null;
            }
        }
    }

    

    public void DoResolutionStart()
    {
        isResolutionDone = false;
        StartCoroutine(DoResolution());
    }

    IEnumerator DoResolution()
    {
        Collider[] hits = Physics.OverlapSphere(GetCenterPosition(), 0.7f);
        foreach (var item in hits)
        {
            if (item.CompareTag("VP"))
            {
                GetVPInThisTile(item.gameObject);
            }
        }
        Volt_Tile standingTile = Volt_ArenaSetter.S.GetTile(transform.position);
        if (standingTile.IsOnVoltage)
        {
            Volt_ArenaSetter.S.numOfSetOnVoltageSpace--;
            Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_ElectricTrapHit),
                GetCenterPosition());
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/Module_Sound/ElectricTrapShock.wav",
                (result) =>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                });
            
            AttackInfo attackInfo = new AttackInfo(playerInfo.playerNumber, 1, CameraShakeType.Attack);
            GetDamage(attackInfo);
            
            if (Volt_PlayerManager.S.I.playerRobot == this)
            {
                //카메라 색상 후처리만.
                Volt_PlayerManager.S.I.playerCamRoot.CameraShake();
            }
            
            standingTile.SetOffVoltage();
        }
        yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
        isResolutionDone = true;
    }

    protected Vector3 ModifyDirection(int playerNumber, Vector3 dir)
    {
        Vector3 modifiedDir = dir;
        switch (playerInfo.playerNumber)
        {
            case 1:
                break;
            case 2:
                if (dir == Vector3.forward)
                    modifiedDir = Vector3.right;
                else if (dir == Vector3.back)
                    modifiedDir = Vector3.left;
                else if (dir == Vector3.left)
                    modifiedDir = Vector3.forward;
                else if (dir == Vector3.right)
                    modifiedDir = Vector3.back;

                else if (dir == Vector3.forward + Vector3.left)
                    modifiedDir = Vector3.forward + Vector3.right;

                else if (dir == Vector3.forward + Vector3.right)
                    modifiedDir = Vector3.back + Vector3.right;

                else if (dir == Vector3.back + Vector3.left)
                    modifiedDir = Vector3.forward + Vector3.left;

                else if (dir == Vector3.back + Vector3.right)
                    modifiedDir = Vector3.back + Vector3.left;
                break;
            case 3:
                if (dir == Vector3.forward)
                    modifiedDir = Vector3.back;
                else if (dir == Vector3.back)
                    modifiedDir = Vector3.forward;
                else if (dir == Vector3.left)
                    modifiedDir = Vector3.right;
                else if (dir == Vector3.right)
                    modifiedDir = Vector3.left;

                else if (dir == Vector3.forward + Vector3.left)
                    modifiedDir = Vector3.back + Vector3.right;

                else if (dir == Vector3.forward + Vector3.right)
                    modifiedDir = Vector3.back + Vector3.left;

                else if (dir == Vector3.back + Vector3.left)
                    modifiedDir = Vector3.forward + Vector3.right;

                else if (dir == Vector3.back + Vector3.right)
                    modifiedDir = Vector3.forward + Vector3.left;
                break;
            case 4:
                if (dir == Vector3.forward)
                    modifiedDir = Vector3.left;
                else if (dir == Vector3.back)
                    modifiedDir = Vector3.right;
                else if (dir == Vector3.left)
                    modifiedDir = Vector3.back;
                else if (dir == Vector3.right)
                    modifiedDir = Vector3.forward;

                else if (dir == Vector3.forward + Vector3.left)
                    modifiedDir = Vector3.back + Vector3.left;

                else if (dir == Vector3.forward + Vector3.right)
                    modifiedDir = Vector3.forward + Vector3.left;

                else if (dir == Vector3.back + Vector3.left)
                    modifiedDir = Vector3.back + Vector3.right;

                else if (dir == Vector3.back + Vector3.right)
                    modifiedDir = Vector3.forward + Vector3.right;
                break;
            default:
                break;
        }
        return modifiedDir.normalized;
    }

    public void SetTimeBomb(Volt_TimeBomb timeBomb)
    {
        StartCoroutine(WaitSetTimeBomb(timeBomb));
    }

    IEnumerator WaitSetTimeBomb(Volt_TimeBomb newTimbBomb)
    {
        //Debug.Log(newTimbBomb.ownerPlayerNumber + "'s TimeBomb Wait Set");

        newTimbBomb.GetComponent<Collider>().enabled = false;

        if (this.TimeBombInstance != null)
        {
            //Debug.Log("Explosion prev TimeBomb");
            this.TimeBombInstance.Explosion();
        }
        if (HitCount >= 3)
        {
            newTimbBomb.isTriggered = false;
            newTimbBomb.GetComponent<Collider>().enabled = true;
            yield break;
        }

        Volt_Tile tile = Volt_ArenaSetter.S.GetTile(transform.position);
        tile.TimeBombInstance = null;
        tile.IsHaveTimeBomb = false;

        Vector3 pos = transform.position;
        pos.y += 3.5f;
        newTimbBomb.transform.position = pos;
        newTimbBomb.transform.SetParent(transform);

        yield return new WaitUntil(() => this.TimeBombInstance == null);
        newTimbBomb.targetRobot = this;
        //Debug.Log("Set new TimeBomb to : " + playerInfo.playerNumber);
        //DB 2000031 업적진행
        if(Volt_PlayerManager.S.I.playerNumber == newTimbBomb.ownerPlayerNumber)
            PacketTransmission.SendAchievementProgressPacket(2000031, newTimbBomb.ownerPlayerNumber, true);
        
        this.TimeBombInstance = newTimbBomb;
        //yield return new WaitUntil(() => this.timeBomb.IsEndMove());
        
        IsTimeBombOn = true;
    }

    public Volt_TimeBomb GetTimeBomb()
    {
        return TimeBombInstance;
    }

    public void CountDownTimeBomb()
    {
        if (TimeBombInstance)
        {
            Debug.Log(playerInfo.playerNumber + "count Down");
            TimeBombInstance.CountDown();
        }
    }
    

    public delegate void AttackDelegate(StateMachine fsm, AttackData attackData);
    public AttackDelegate attackHandler;

    public StateMachine fsm;


    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        if(IsCollideWithModuleCard(other))
        {
            Volt_ModuleCardBase card = other.GetComponent<Volt_RandomBox>().moduleInBox;
            if (moduleCardExcutor.IsCanEquip(card))
            {
                OnPickupNewModuleCard(card);
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_GET_MODULEBOX.wav",
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });
                
                Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_GetModuleItem),
                    transform);
                other.GetComponent<Volt_RandomBox>().DestroyModule(false);
            }
            else
            {
                other.GetComponent<Volt_RandomBox>().DestroyModule(true);
            }
        }
        else if(IsCollideWithRepairKit(other))
        {
            Volt_Tile currentTile = Volt_ArenaSetter.S.GetTile(transform.position);
            currentTile.RobotAcquireKit(this);
        }
        else if(IsCollideWithVP(other))
        {
            if (!addOnsMgr.IsHackingOn)
                return;
            //Debug.Log("해킹 발동!");
            moduleCardExcutor.GetModuleCardByCardType(Card.HACKING).OnUseCard();
            Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseHacking, fsm.Owner.playerInfo.playerNumber);
            GetVPInThisTile(other.gameObject);
            moduleCardExcutor.DestroyCard(Card.HACKING);
        }
        
    }
  
    private void OnDestroy()
    {
        Volt_ArenaSetter.S.robotsInArena.Remove(this);
        if (Volt_PlayerManager.S.I == playerInfo)
        {
            Volt_PlayerUI.S.RobotDeadModuleBtnInit();
            Volt_PlayerManager.S.I.playerCamRoot.SetSaturationDown(true);
        }
        RobotBehaviourObserver.Instance.OffBehaviorFlag(playerInfo.playerNumber);
        playerInfo.playerRobot = null;
        playerInfo.isRobotAlive = false;

        if (Volt_GMUI.S.IsCheatPanelOn)
            Volt_GMUI.S.cheatPanel.GetComponent<Volt_CheatPanel>().RobotDead(this);

        moduleCardExcutor.DestroyCardAll();
        if (playerInfo.playerNumber == Volt_GameManager.S.AmargeddonPlayer)
        {
            Volt_GameManager.S.AmargeddonPlayer = 0;
            Volt_GameManager.S.AmargeddonCount = 0;
        }
    }
    #endregion

    public void GetDamage(AttackInfo attackInfo)
    {
        if (fsm.isDead) return;

        //Debug.Log(playerInfo.playerNumber + " GetDamaged from" + attackInfo.AttackerNumber);
        fsm.attackInfo = attackInfo;
        fsm.isDamaged = true;

    }

    public GameObject[] GetHitRobotsInNearestOrder(Volt_RobotBehavior behavior)
    {
        return GetHitRobotsInNearestOrder(behavior.Direction, behavior.BehaviorPoints);
    }

    public GameObject[] GetHitRobotsInNearestOrder(Vector3 direction, int range)
    {
        RaycastHit[] hits;
        CheckHitRay(direction, range, out hits);

        List<GameObject> hitedGOs = new List<GameObject>();
        for (int i = 0; i < hits.Length; ++i)
        {
            float closetDist = float.MaxValue;
            int index = i;

            for (int j = i; j < hits.Length; ++j)
            {
                float dist = hits[j].distance;
                if (dist < closetDist)
                {
                    index = j;
                    closetDist = dist;
                }
            }

            RaycastHit temp = hits[index];
            hits[index] = hits[i];
            hits[i] = temp;
            hitedGOs.Add(hits[i].transform.gameObject);
        }

        return hitedGOs.ToArray();
    }

    private void CheckHitRay(Vector3 direction, int range, out RaycastHit[] hits)
    {
        Volt_Tile from = Volt_ArenaSetter.S.GetTile(transform.position);
        Volt_Tile to = Volt_ArenaSetter.S.GetTile(transform.position,
            direction, range);

        float rayDist = (to.transform.position - from.transform.position).magnitude + 1;
        Vector3 rayPos = transform.position;
        rayPos.y += 1f;
        Ray ray = new Ray(rayPos, direction);

        gameObject.layer = Physics.IgnoreRaycastLayer;
        hits = Physics.RaycastAll(ray, rayDist, fsm.robotMask | fsm.wallMask);

        gameObject.layer = LayerMask.NameToLayer("Robot");
#if UNITY_EDITOR
        Debug.DrawRay(rayPos, direction * range, Color.blue, 3f);
#endif
    }

    public Vector3 GetCenterPosition()
    {
        if (transform.Find("Bip001"))
        {
            return transform.Find("Bip001").position;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y += 1;
            return pos;
        }
    }
    public CollisionData GetCollisionData()
    {
        CollisionData data = new CollisionData();
        data.robot = gameObject;
        data.pushType = (PushType)System.Enum.Parse(typeof(PushType), pushType.ToString());
        data.isHaveAnchor = addOnsMgr.IsHaveAnchor;
        //Debug.Log("Collision1");
        if (pushType == PushType.Pusher && fsm.isMoving)
        {
            data.isHaveSawblade = addOnsMgr.IsHaveSawBlade;
            data.behaviorPoints = fsm.behavior != null ? fsm.behavior.BehaviorPoints : 0;

            // IsHaveSawBlade가 true이면 이번에 반드시 사용되기 때문에 false로
            // 변경한다. 만약 false이어도 어차피 false이니깐 false를 대입해도 문제가 없다.
            //Debug.Log("Collision2");
            addOnsMgr.IsHaveSawBlade = false;
        }

        if(fsm.destPos.magnitude == float.PositiveInfinity)
        {
            //Debug.Log($"{playerInfo.playerNumber} dest tile is null");
            data.destTile = null;
        }
        else
        {
            Volt_Tile to = Volt_ArenaSetter.S.GetTile(fsm.destPos);
            data.destTile = to;

            if (pushType == PushType.Pushed && fsm.knockbackInfor != null)
            {
                fsm.destPos = Vector3.positiveInfinity;
                fsm.knockbackInfor = null;
            }
        }

        return data;
    }

    public void ShieldEffectPlay()
    {
        FXV.FXVShield shieldEffect = shieldEffectObj.GetComponent<FXV.FXVShield>();
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_SHIELD HIT.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        shieldEffect.OnHit((Vector3.up * ((shieldEffect.transform.localScale.x / 10f) * shieldEffect.GetComponent<SphereCollider>().radius)) + shieldEffect.transform.position, 2f);
    }

    public void OnEndBeamCallback()
    {
        GameObject go = GameObject.FindGameObjectWithTag("PowerbeamModule");
        //Debug.Assert(go != null, "너 xx 뭐야 왜 널리야 너 Tag에 문제있어?");
        go.GetComponent<SciFiArsenal.SciFiBeamScript>().BeamEnd();
    }

    public bool IsMyRobot(int playerNumber)
    {
        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        if (player.playerRobot == this.gameObject)
            return true;
        return false;
    }

    public void SoundPlay(string clipName)
    {
        switch (clipName)
        {
            case "attack":
                audioController.PlayOneShot(audioController.attack);
                break;
            case "movement":
                audioController.PlayOneShot(audioController.movement, 0.5f);
                break;
            case "dodge":
                audioController.PlayOneShot(audioController.dodge);
                break;
            case "death":
                audioController.PlayOneShot(audioController.death);
                break;
            case "victory":
                audioController.PlayOneShot(audioController.victory);
                break;
            case "lose":
                audioController.PlayOneShot(audioController.lose);
                break;
            case "hit":
                audioController.PlayOneShot(audioController.hit);
                break;
            case "stun":
                audioController.PlayOneShot(audioController.stun);
                break;
            case "select":
                audioController.PlayOneShot(audioController.select);
                break;
            case "guard":
                audioController.PlayOneShot(audioController.guard);
                break;
            case "doubleAttack":
                audioController.PlayOneShot(audioController.doubleAttack);
                break;
            default:
                break;
        }
    }

    public void UseExtraCard()
    {
        moduleCardExcutor.ExcuteExtraCard();
    }

    public void OnPickupNewModuleCard(Volt_ModuleCardBase newCard)
    {
        newCard.Initialize(this);
        moduleCardExcutor.PutNewCardInCards(newCard);

        if (Volt_PlayerManager.S.I.playerRobot == this.gameObject)
            Volt_PlayerUI.S.NewModuleEquip(newCard);

        Volt_CheatPanel.S.NoticeGetModuleCard(playerInfo.playerNumber, newCard.card);

    }

    //public string CalculateHitCount(int damage)
    //{
    //    int resultDamage = damage;

    //    if (addOnsMgr.ShieldPoints > 0)
    //    {
    //        ShieldEffectPlay();

    //        //fsm.Animator.Play("Shield", -1, 0);
    //        resultDamage = damage - addOnsMgr.ShieldPoints;
    //        addOnsMgr.ShieldPoints -= damage;
            
    //        if (Volt_PlayerManager.S.I.playerNumber == playerInfo.playerNumber)
    //            PacketTransmission.SendAchievementProgressPacket(2000028, playerInfo.playerNumber, true);

    //        if (addOnsMgr.ShieldPoints <= 0)
    //        {
    //            moduleCardExcutor.DestroyCard(Card.SHIELD);
    //            addOnsMgr.ShieldPoints = 0;
    //        }

    //        if (resultDamage <= 0)
    //        {
    //            return "Shield";
    //        }
    //    }

    //    HitCount += resultDamage;
    //    return "Damaged";
    //}

    

    private bool IsCollideWithRepairKit(Collider other)
    {
        if (other.CompareTag("RepairKit"))
            return true;
        return false;
    }

    private bool IsCollideWithVP(Collider other)
    {
        if (other.CompareTag("VP"))
            return true;
        return false;
    }

    private bool IsCollideWithModuleCard(Collider other)
    {
        if (other.CompareTag("ModuleCard"))
            return true;
        return false;
    }

    void GetVPInThisTile(GameObject coinInstance)
    {
        Volt_Tile vpPlaceTile = Volt_ArenaSetter.S.GetTile(coinInstance.transform.position);
        if (playerInfo == Volt_PlayerManager.S.I)
            Volt_GamePlayData.S.Coin++;
        vpPlaceTile.RobotAcquireVP(playerInfo.playerNumber);
    }
    public void ChargeEffectPlay(int characterNumber)
    {
        switch (characterNumber)
        {
            case 0: //볼트
                Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_VoltCharging),
                    transform.Find("Bip001/Bip001_Pelvis/Bip001_Spine/Bip001_Spine1/Bip001_Spine2/Bone_R_UpperArm/Bone_R_Forearm/Bone_R_hand/Dummy_R_Hand/LaunchPoint"));
                break;
            default:
                break;
        }

    }
    public void ForcedKillRobot()
    {
        Volt_ArenaSetter.S.robotsInArena.Remove(this);
        moduleCardExcutor.DestroyCardAll();
        playerInfo.playerRobot = null;
        if (Volt_PlayerManager.S.I == fsm.Owner.playerInfo)
        {
            Volt_PlayerUI.S.RobotDeadModuleBtnInit();
            Volt_PlayerManager.S.I.playerCamRoot.SetSaturationDown(true);
        }
        if (fsm.Owner.playerInfo.playerNumber == Volt_GameManager.S.AmargeddonPlayer)
        {
            Volt_GameManager.S.AmargeddonPlayer = 0;
            Volt_GameManager.S.AmargeddonCount = 0;
        }
        if (Volt_GMUI.S.IsCheatPanelOn)
            Volt_GMUI.S.cheatPanel.GetComponent<Volt_CheatPanel>().RobotDead(this);
        Destroy(this.gameObject);
    }
    public void MoveEffectPlay()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += 180f;
        Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_RobotRunSmoke),
            fsm.transform.position, Quaternion.Euler(rot));
    }

    #region Killbot method
    
    public void DetectRobot()
    {
        Vector3 forward = transform.forward;
        if (Volt_Utils.IsForward(forward) == false &&
            Volt_Utils.IsBackward(forward) == false &&
            Volt_Utils.IsRight(forward) == false &&
            Volt_Utils.IsLeft(forward) == false)
        {
            Vector3[] directions = new Vector3[4] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
            float minAngleDiff = float.MaxValue;
            Vector3 dir = Vector3.zero;
            for(int i = 0; i < 4; ++i)
            {
                float angleDiff = Vector3.Angle(directions[i], forward);
                if(angleDiff < minAngleDiff)
                {
                    minAngleDiff = angleDiff;
                    forward = directions[i];
                }
            }
        }

        Volt_Tile targetTile;
        Detect(out targetTile, WhatIsTarget.Bot);

        if (IsOnVPCoin(Volt_ArenaSetter.S.GetTile(transform.position)))
        {
            //Debug.Log("킬봇이 승점 코인 위에 있다.");
            Volt_RobotBehavior behavior = new Volt_RobotBehavior(0, Vector3.zero,
                BehaviourType.None, playerInfo.playerNumber);
            if(targetTile != null)
            {
                Vector3 targetPos = targetTile.transform.position;
                targetPos.y = transform.position.y;
                int behaivourPoints = Volt_ArenaSetter.S.GetDistanceBetweenTiles(
                    Volt_ArenaSetter.S.GetTile(transform.position),
                    targetTile);
                behavior = new Volt_RobotBehavior(behaivourPoints, (targetPos - transform.position).normalized, BehaviourType.Attack, playerInfo.playerNumber);
            }

            if (Volt_GameManager.S.isKillbotBehaviourOff)
                behavior.BehaviourType = BehaviourType.None;

            if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
            {
                Volt_GameManager.S.RobotBehaviourInputToManager(behavior);
            }
            else
            {
                Volt_PlayerManager.S.SendBehaviorOrderPacket(playerInfo.playerNumber, behavior);
            }

        }
        else if (targetTile == null)
        {
            int behaviourPoints = UnityEngine.Random.Range(2, 4);//Random.Range(2, 4);//2; //추후 1~3 랜덤하게 설정
            Volt_RobotBehavior behavior = new Volt_RobotBehavior(behaviourPoints, forward, BehaviourType.Move, playerInfo.playerNumber);
            if (Volt_GameManager.S.isKillbotBehaviourOff)
                behavior.BehaviourType = BehaviourType.None;

            if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
            {
                Volt_GameManager.S.RobotBehaviourInputToManager(behavior);
            }
            else
            {
                Volt_PlayerManager.S.SendBehaviorOrderPacket(playerInfo.playerNumber, behavior);
            }
        }
        else
        {
            Vector3 targetPos = targetTile.transform.position;
            targetPos.y = transform.position.y;
            int behaivourPoints = Volt_ArenaSetter.S.GetDistanceBetweenTiles(
                Volt_ArenaSetter.S.GetTile(transform.position),
                targetTile);
            Volt_RobotBehavior behavior = new Volt_RobotBehavior(behaivourPoints, (targetPos - transform.position).normalized, BehaviourType.Attack, playerInfo.playerNumber);

            if (Volt_GameManager.S.isKillbotBehaviourOff)
                behavior.BehaviourType = BehaviourType.None;

            if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
            {
                Volt_GameManager.S.RobotBehaviourInputToManager(behavior);
            }
            else
            {
                Volt_PlayerManager.S.SendBehaviorOrderPacket(playerInfo.playerNumber, behavior);
            }
            
        }
    }

    private Volt_Tile[][] GetDetectingTiles()
    {
        //Debug.Log(playerInfo.playerNumber + " killbot adj tiles");
        
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        if (Volt_Utils.IsForward(forward) == false &&
            Volt_Utils.IsBackward(forward) == false &&
            Volt_Utils.IsRight(forward) == false &&
            Volt_Utils.IsLeft(forward) == false)
        {
            Vector3[] directions = new Vector3[4] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
            float minAngleDiff = float.MaxValue;
            for (int i = 0; i < 4; ++i)
            {
                float angleDiff = Vector3.Angle(directions[i], forward);
                if (angleDiff < minAngleDiff)
                {
                    minAngleDiff = angleDiff;
                    forward = directions[i];
                }
            }
            right = Vector3.Cross(forward, Vector3.up).normalized;
        }

        Volt_Tile[][] tiles = new Volt_Tile[3][];
        tiles[0] = Volt_ArenaSetter.S.GetTiles(transform.position, forward, 3);
        tiles[1] = Volt_ArenaSetter.S.GetTiles(transform.position, right, 3);
        tiles[2] = Volt_ArenaSetter.S.GetTiles(transform.position, -right, 3);

        return tiles;
    }

    // The killbot detect other robots around it or pointCard before moving.
    public void Detect(out Volt_Tile closetTile, WhatIsTarget whatIsTarget)
    {
        //Get forward, right, left 3 tiles
        Volt_Tile[][] tiles = GetDetectingTiles();
        System.Func<Volt_Tile, bool> detectFunc = GetDetectFunction(whatIsTarget);

        int closetDistance = int.MaxValue;
        // Default is a tile in front of killbot 
        closetTile = null;
        for (int i = 0; i < tiles.Length; i++)
        {
            int distance = 0;
            for (int j = 0; j < tiles[i].Length; j++)
            {
                if (tiles[i][j].pTileType == Volt_Tile.TileType.none)
                    continue;
                if (tiles[i][j] == Volt_ArenaSetter.S.GetTile(transform.position))
                    continue;

                distance += tiles[i][j]._weightValue;
                if (detectFunc(tiles[i][j]))
                {
                    if (distance < closetDistance)
                    {
                        closetDistance = distance;
                        closetTile = tiles[i][j];
                    }
                }
            }
        }
    }

    private System.Func<Volt_Tile, bool> GetDetectFunction(WhatIsTarget whatIsTarget)
    {
        System.Func<Volt_Tile, bool> detectFunc;
        switch (whatIsTarget)
        {
            case WhatIsTarget.Bot:
                detectFunc = tile =>
                {
                    List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;
                    foreach (var robot in robots)
                    {
                        if (Volt_GameManager.S.IsTutorialMode)
                        {
                            if (robot == this || robot.playerInfo.PlayerType == PlayerType.AI)//robot.CompareTag("Killbot"))
                                continue;
                        }
                        else
                        {
                            if (robot == this)
                                continue;
                        }
                        Volt_Tile robotStandTile = Volt_ArenaSetter.S.GetTile(robot.transform.position);
                        if (tile == robotStandTile)
                        {
                            Vector3 origin = transform.position + Vector3.up * .5f;
                            Vector3 toTarget = (robotStandTile.transform.position - Volt_ArenaSetter.S.GetTile(transform.position).transform.position);
                            Ray ray = new Ray(origin, toTarget.normalized);
                            if(Physics.Raycast(ray, toTarget.magnitude, LayerMask.GetMask("Wall")))
                            {
                                return false;
                            }
                            return true;
                        }
                    }
                    return false;
                };
                break;
            case WhatIsTarget.Coin:
                detectFunc = tile =>
                {
                    if (tile.numOfCoins > 0)
                        return true;
                    return false;
                };
                break;
            case WhatIsTarget.All:
                detectFunc = tile =>
                {
                    if (tile.numOfCoins > 0)
                    {
                        //Debug.Log("Detect coin");
                        return true;
                    }

                    List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;
                    foreach (var robot in robots)
                    {
                        if (robot == this)// || robot.playerInfo.PlayerType == PlayerType.AI)//robot.CompareTag("Killbot"))
                            continue;
                        Volt_Tile robotStandTile = Volt_ArenaSetter.S.GetTile(robot.transform.position);
                        if (tile == robotStandTile)
                            return true;
                    }
                    return false;
                };
                break;
            default:
                //Debug.Log("The killbot not detect anything");
                detectFunc = tile => false;
                break;
        }
        return detectFunc;
    }

    public bool IsOnVPCoin(Volt_Tile standingTile)
    {
        Volt_Tile tile = standingTile;

        if (tile.numOfCoins > 0)
            return true;
        return false;
    }
    #endregion

    
    
}
