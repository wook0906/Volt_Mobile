using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Volt_RobotAddOnsManager 
{
    private Volt_ModuleCardExcutor moduleCardExcutor;
    private Volt_Robot owner;
    public Volt_RobotAddOnsManager(Volt_ModuleCardExcutor excutor, Volt_Robot robot)
    {
        moduleCardExcutor = excutor;
        this.owner = robot;
        //if(excutor)
        //    Debug.Log(owner.playerInfo.playerNumber + " Create addons");
        this.owner.isInitAddOnMgr = true;
    }
    [SerializeField]
    private bool isHaveAnchor = false;
    public bool IsHaveAnchor
    {
        get { return isHaveAnchor; }
        set { isHaveAnchor = value; }
    }
    public bool IsSteeringNozzleOn
    {
        get;
        set;
    }
    [SerializeField]
    private bool isDodgeOn = false;
    public bool IsDodgeOn
    {
        get
        {
            //if (isDodgeOn)
            //{
            //    isDodgeOn = false;
            //    // 이거 사용하기 일텐데 왜 여기 넣음?
            //    //DB 2000026 업적진행
            //    Debug.Log("Dodge : "+isDodgeOn);
            //    return true;
            //}
            return isDodgeOn;
        }
        set { isDodgeOn = value; }
    }
    private int shieldPoints = 0;
    public int ShieldPoints {
        get { return shieldPoints; }
        set
        {
            shieldPoints = value;
            if(shieldPoints < 0)
                shieldPoints = 0;
            
            owner.panel.RenewShield(shieldPoints);
            if (shieldPoints == 2)
                owner.ShieldEffectOn(true);
            else if (shieldPoints <= 0)
            {
                shieldPoints = 0;
                owner.ShieldEffectOn(false);
            }
        }
    }
    
    [SerializeField]
    private bool isHaveBomb = false;
    public bool IsHaveBomb
    {
        get
        {
            if (isHaveBomb)
            {
                isHaveBomb = false;
                GameObject go = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_ExplosionTimebomb);//Volt_PrefabFactory.S.GetInstance(Volt_PrefabFactory.S.effect_TimeBomb);
                Vector3 pos = moduleCardExcutor.transform.position;
                pos.y += 1f;
                go.transform.position = pos;
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Robot_Attack_Effects/Hound_ImpactSE.wav",
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });
                
                moduleCardExcutor.DestroyCard(Card.BOMB);
                return true;
            }
            return isHaveBomb;
        }
        set { isHaveBomb = value; }
    }

    [SerializeField]
    private bool isHaveSawBlade = false;
    public bool IsHaveSawBlade
    {
        get
        {
            //Debug.Log("#####################Call IsSawBlade");
            return isHaveSawBlade;
        }
        set
        {
            //if (value == false)
                //moduleCardExcutor.DestroyCard(Card.SAWBLADE);
            isHaveSawBlade = value;
            //if (moduleCardExcutor == null)
            //    Debug.Log("ModulecardEx is null");
            //else if (moduleCardExcutor.GetComponent<Volt_Robot>())
            //    Debug.Log("Volt_Robot is null");
            //else if (moduleCardExcutor.GetComponent<Volt_Robot>().playerInfo == null)
            //    Debug.Log("Playerinfor is null");
            //Debug.Log(" ###################SetHere!!!!!!!!!! " + isHaveSawBlade);
        }
    }

    [SerializeField]
    private bool isHackingOn = false;
    public bool IsHackingOn
    {
        get
        {
            if (isHackingOn)
            {
                isHackingOn = false;
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_HACKING.mp3",
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });

                GameObject go = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_HackingModule);
                go.transform.position = owner.transform.position;
                return true;
            }
            return isHackingOn;
        }
        set { isHackingOn = value; }
    }

    [SerializeField]
    private bool isDummyGearOn = false;
    public bool IsDummyGearOn
    {
        get
        {
            return isDummyGearOn;
            //if (isDummyGearOn)
            //{
            //    isDummyGearOn = false;
                
            //    moduleCardExcutor.DestroyCard(Card.DUMMYGEAR);
            //    return true;
            //}
            //return isDummyGearOn;
        }
        set { isDummyGearOn = value; }
    }

    public void UseAnchorModule()
    {
        owner.moduleCardExcutor.GetModuleCardByCardType(Card.ANCHOR).OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseAnchor, owner.playerInfo.playerNumber);
        IsHaveAnchor = false;
        //DB 2000032 업적진행
        if (Volt_PlayerManager.S.I.playerNumber == owner.playerInfo.playerNumber)
        {
            
        }
        GameObject go = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_Anchor);
        Vector3 pos = moduleCardExcutor.transform.position;
        pos.y += Volt_ArenaSetter.S.tileScale.y + (Volt_ArenaSetter.S.tileScale.y * 0.1f)+0.5f;
        go.transform.position = pos;
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/Module_Sound/Anchor.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        moduleCardExcutor.DestroyCard(Card.ANCHOR);
    }

    public void UseBombModule()
    {
        if (!isHaveBomb || owner.fsm.prevState is FallState)
            return;

        owner.moduleCardExcutor.GetModuleCardByCardType(Card.BOMB).OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseBomb, owner.playerInfo.playerNumber);
        isHaveBomb = false;
        owner.moduleCardExcutor.DestroyCard(Card.BOMB);

        // 자폭 모듈을 가지고 있었다면.
        List<Volt_Tile> adjecentTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTile(owner.transform.position).GetAdjecentTiles());
        adjecentTiles.Add(Volt_ArenaSetter.S.GetTile(owner.transform.position));
        foreach (var item in adjecentTiles)
        {
            if (item == null) continue;
            item.SetBlinkOption(BlinkType.Attack, 0.5f);
            item.BlinkOn = true;
            List<Volt_Robot> tileInRobots = item.GetRobotsInTile();
            if (tileInRobots.Count == 0)
                continue;

            foreach (var robot in tileInRobots)
            {
                if (robot == owner) continue;
                robot.GetDamage(new AttackInfo(owner.playerInfo.playerNumber, 1, CameraShakeType.Bomb, owner.playerInfo.GetHitEffect(),Card.BOMB));
            }
        }
        //Volt_SoundManager.S.RequestSoundPlay(Resources.Load<AudioClip>("Sounds/VOLT_Robot_Attack_Effects/Hound_ImpactSE"), false);
        
        Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_ExplosionTimebomb),
            owner.transform.position);
    }

    public void UseDummyGearModule()
    {
        if (!isDummyGearOn)
            return;

        owner.moduleCardExcutor.GetModuleCardByCardType(Card.DUMMYGEAR).OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseDummy, owner.playerInfo.playerNumber);

        isDummyGearOn = false;
        owner.moduleCardExcutor.DestroyCard(Card.DUMMYGEAR);

        // 더미 모듈 이펙트 플레이~
        GameObject dummyEffect = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_DummyGearModule);
        Vector3 effectPos = Volt_ArenaSetter.S.GetTile(owner.transform.position).transform.position;
        effectPos.y += 2f;
        dummyEffect.transform.position = effectPos;
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_DUMMY.mp3",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
    }
}
