using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/AttackState")]
public class AttackState : StateBase
{
    public override void OnEnterState(StateMachine fsm)
    {
        PacketTransmission.SendAttackPacket(fsm.Owner.playerInfo.playerNumber, Volt_GMUI.S.RoundNumber);

        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        fsm.isPlayAttackAnimation = false;
        fsm.AniEventHandler.isDoneAttack = false;

        Volt_ModuleCardBase moduleCard = DrawModuleCard(fsm);
        fsm.attack = AttackFactory.CreateAttackInstance(fsm, moduleCard);
        fsm.attack.Init(fsm, moduleCard);
        fsm.AniEventHandler.attackHandler = fsm.attack.AttackHandler;
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.AniEventHandler.isDoneAttack = false;
        fsm.behavior = null;
        fsm.isPlayAttackAnimation = false;
        fsm.lastAttackRobots.Clear();

        if (fsm.attackModule != null)
            fsm.Owner.moduleCardExcutor.DestroyCard(fsm.attackModule);
        fsm.attackModule = null;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
        if (fsm.isPlayAttackAnimation)
            return;

        RotateToAttackDir(fsm, deltaTime);
    }

    private void RotateToAttackDir(StateMachine fsm, float deltaTime)
    {
        fsm.transform.rotation = Quaternion.Slerp(fsm.transform.rotation,
            Quaternion.LookRotation(fsm.behavior.Direction), fsm.angular * deltaTime);
        float angle = Vector3.Angle(fsm.transform.forward, fsm.behavior.Direction);

        if (angle < 5f)
        {
            fsm.transform.rotation = Quaternion.LookRotation(fsm.behavior.Direction);
            fsm.Animator.Play(fsm.attack.AttackType.ToString());
            fsm.isPlayAttackAnimation = true;

            if (fsm.attackModule != null)
                MsgHandleByAttackModuleType(fsm, fsm.attackModule.card);
        }
    }

    private Volt_ModuleCardBase DrawModuleCard(StateMachine fsm)
    {
        Volt_ModuleCardExcutor moduleCardExcutor = fsm.Owner.moduleCardExcutor;
        foreach (Volt_ModuleCardBase card in moduleCardExcutor.GetCurEquipNormalCards())
        {
            if (card.ActiveType == 1 && card.activeTime == ActiveTime.Doing &&
                card.behaviourType == BehaviourType.Attack)
            {
                fsm.attackModule = card;
                return card;
            }
        }
        return null;
    }
    private void MsgHandleByAttackModuleType(StateMachine fsm, Card moduleCardType)
    {
        switch (moduleCardType)
        {
            case Card.CROSSFIRE:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseCrossFire, fsm.Owner.playerInfo.playerNumber);
                break;
            case Card.GRENADES:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseGrenade, fsm.Owner.playerInfo.playerNumber);
                break;
            case Card.PERNERATE:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.UsePernerate, fsm.Owner.playerInfo.playerNumber);
                break;
            case Card.POWERBEAM:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.UsePowerBeam, fsm.Owner.playerInfo.playerNumber);
                break;
            case Card.TIMEBOMB:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseTimeBomb
        , fsm.Owner.playerInfo.playerNumber);
                break;
            case Card.DOUBLEATTACK:
                Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseDoubleAttack, fsm.Owner.playerInfo.playerNumber);
                break;
            default:
                break;
        }
    }
}
