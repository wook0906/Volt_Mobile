using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/DeadState")]
public class DeadState : StateBase
{
    enum DeathType
    {
        None,
        KilledByOpponent,
        KillSelf,
        Trap,
        SuddenDeath,
        FallSelf,
        Max
    }

    private float delayTime = 3f;

    private void Reset(StateMachine fsm)
    {
        fsm.Owner.moduleCardExcutor.DestroyCardAll();
        if (fsm.Owner.playerInfo.playerNumber == Volt_GameManager.S.AmargeddonPlayer)
        {
            Volt_GameManager.S.AmargeddonPlayer = 0;
            Volt_GameManager.S.AmargeddonCount = 0;
        }

        if (Volt_GMUI.S.IsCheatPanelOn)
            Volt_GMUI.S.cheatPanel.GetComponent<Volt_CheatPanel>().RobotDead(fsm.Owner);
    }

    private void PlayRobotDestroyEffect(GameObject effect, Vector3 pos)
    {
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Robot_Attack_Effects/Hound_ImpactSE.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        Volt_ParticleManager.Instance.PlayParticle(effect, pos);
    }

    private void HandleDeathEachGamePhase(StateMachine fsm)
    {
        // 게임의 Phase별로 죽음에 대한 처리를 다르게한다.
        DeathType type = DeathType.None;
        switch (Volt_GameManager.S.pCurPhase)
        {
            case Phase.suddenDeath:
                fsm.Owner.playerInfo.VictoryPoint--;
                PacketTransmission.SendVictoryPointPacket(fsm.Owner.playerInfo.playerNumber, fsm.Owner.playerInfo.VictoryPoint);
                type = DeathType.SuddenDeath;
                break;
            case Phase.resolution:
                type = DeathType.Trap;
                break;
            default:
                if (IsSelfKill(fsm))
                {
                    if (fsm.prevState.GetType() == typeof(DamagedState))
                        type = DeathType.KillSelf;
                    else if (fsm.prevState.GetType() == typeof(FallState))
                        type = DeathType.FallSelf;
                    //else
                        //Debug.LogError("자살로 죽었는데 데미지를 입거나 낙사를 하지 않았다.... 이상하다..;;;");
                }
                else
                {
                    type = DeathType.KilledByOpponent;
                }
                break;
        }

        ShowMessage(type, fsm);
    }

    private void ShowMessage(DeathType type, StateMachine fsm)
    {
        if (!Volt_PlayerManager.S.I.CompareTo(fsm.Owner.playerInfo))
            return;

        switch (type)
        {
            case DeathType.KillSelf:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.KillSelf, Volt_PlayerManager.S.I.playerNumber);
                break;
            case DeathType.FallSelf:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.FallSelf, Volt_PlayerManager.S.I.playerNumber);
                break;
            case DeathType.KilledByOpponent:
                if (fsm.attackInfo!=null)
                {
                    Volt_GMUI.S.Create2DMsg(MSG2DEventType.DeadToPlayer, fsm.attackInfo.AttackerNumber);
                }
                else
                {
                    Volt_GMUI.S.Create2DMsg(MSG2DEventType.DeadToPlayer, RobotBehaviourObserver.Instance.currentPusher.playerInfo.playerNumber);
                }
                
                break;
            case DeathType.Trap:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.DeadToTrap, Volt_PlayerManager.S.I.playerNumber);
                break;
            case DeathType.SuddenDeath:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.DeadToTrap, Volt_PlayerManager.S.I.playerNumber);
                Volt_GMUI.S.Create3DMsg(MSG3DEventType.PointDown, fsm.Owner.playerInfo);
                break;
            default:
                break;
        }
    }

    public override void OnEnterState(StateMachine fsm)
    {
        //DB 어떤 모듈에 죽었는지 판단하여 모듈 사용관련 일반업적에 반영
        if (fsm.attackInfo != null)
        {
            KillByModuleAchievementHandle(fsm.attackInfo.ModuleInfo, fsm.attackInfo.AttackerNumber);
            Volt_GamePlayData.S.RenewNumberOfRobotsKilledByPlayerInThisTurn(fsm.attackInfo.AttackerNumber, fsm.Owner.playerInfo.playerNumber);
        }

        Volt_PlayerManager.S.I.playerCamRoot.CameraMoveStart(fsm.gameObject);
        fsm.isDead = true;
        fsm.elapsedTime = 0f;
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        fsm.GetComponent<Collider>().enabled = false;

        // 공격자의 콜백 함수 호출
        if (fsm.Owner.lastAttackPlayer != 0)
        {
            Volt_PlayerInfo attackPlayer = Volt_PlayerManager.S.GetPlayerByPlayerNumber(fsm.Owner.lastAttackPlayer);
            if (attackPlayer != fsm.Owner.playerInfo)
                attackPlayer.OnKillRobot(fsm.Owner);
        }

        fsm.Animator.CrossFade("Dead", .1f);

        // 로봇 죽음 횟수 올림. 기록으로 사용될 것
        if(fsm.Owner == Volt_PlayerManager.S.I.GetRobot())
            Volt_GamePlayData.S.Death++;

        // 로봇 폭발 이펙트
        PlayRobotDestroyEffect(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_ExplosionRobot),
            fsm.Owner.GetCenterPosition());

        fsm.Owner.AddOnsMgr.UseBombModule();
        fsm.Owner.AddOnsMgr.UseDummyGearModule();

        HandleDeathEachGamePhase(fsm);

        if (Volt_PlayerManager.S.I == fsm.Owner.playerInfo)
        {
            Volt_PlayerManager.S.I.playerCamRoot.SetSaturationDown(true);
        }
        Reset(fsm);
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
        if(fsm.elapsedTime < delayTime)
        {
            fsm.elapsedTime += deltaTime;
            return;
        }

        if (fsm.Owner.TimeBombInstance)
        {
            if (fsm.Owner.TimeBombInstance.isPlaying)
                return;
            fsm.Owner.TimeBombInstance.Destroy();
        }

        //fsm.Owner.AddOnsMgr.UseBombModule();
        //Reset(fsm);
        foreach (var item in fsm.transform.GetComponentsInChildren<Poolable>())
        {
            Managers.Pool.Push(item);
        }
        Volt_ArenaSetter.S.robotsInArena.Remove(fsm.Owner);
        fsm.Owner.playerInfo.playerRobot = null;
        fsm.Owner.playerInfo.isRobotAlive = false;
        RobotBehaviourObserver.Instance.OffBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        Destroy(fsm.Owner.gameObject);
    }

    private bool IsSelfKill(StateMachine fsm)
    {
        //Debug.Assert(fsm.Owner.lastAttackPlayer != 0, "라스트 어택커 넘버가 0이다 그럴 수 없다!! 확인 바람!!");

        if (fsm.Owner.lastAttackPlayer == fsm.Owner.playerInfo.playerNumber)
            return true;

        else if (fsm.attackInfo != null)
        {
            if (fsm.attackInfo.AttackerNumber == fsm.Owner.playerInfo.playerNumber)
                return true;
        }

        return false;
    }

    void KillByModuleAchievementHandle(Card card, int attackNumber)
    {
        switch (card)
        {
            case Card.SAWBLADE:
                //DB 2000024
                PacketTransmission.SendAchievementProgressPacket(2000024, attackNumber,true);
                break;
            case Card.DOUBLEATTACK:
                //DB 2000022
                PacketTransmission.SendAchievementProgressPacket(2000022, attackNumber, true);
                break;
            case Card.AMARGEDDON:
                //DB 2000023
                PacketTransmission.SendAchievementProgressPacket(2000023, attackNumber, true);
                break;
            case Card.BOMB:
                //DB 2000029
                PacketTransmission.SendAchievementProgressPacket(2000029, attackNumber, true);
                break;
            default:
                //Debug.Log("어 그렇네? <- 뭐가 그런데? 오빤 항상 그런식이야 됐어 나 갈래");
                break;
        }
    }
}
