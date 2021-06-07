 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/DamagedState")]
public class DamagedState : StateBase
{
    private int CalculateDamage(Volt_Robot robot, int damage)
    {
        int result = damage;

        if (robot.AddOnsMgr.ShieldPoints == 0)
            return result;


        robot.ShieldEffectPlay();

        result = damage - robot.AddOnsMgr.ShieldPoints;
        robot.AddOnsMgr.ShieldPoints -= damage;

        if (Volt_PlayerManager.S.I.playerNumber == robot.playerInfo.playerNumber)
            PacketTransmission.SendAchievementProgressPacket(2000028, robot.playerInfo.playerNumber, true);

        if (robot.AddOnsMgr.ShieldPoints <= 0)
        {
            robot.moduleCardExcutor.DestroyCard(Card.SHIELD);
            robot.AddOnsMgr.ShieldPoints = 0;
        }

        return Mathf.Max(0, result);
    }

    private string GetAnimationClipName(int damage)
    {
        return damage > 0 ? "Damaged" : "Shield";
    }

    private void ForwardToAttacker(Transform my, Volt_Robot attacker)
    {
        if (attacker == null)
            return;

        Vector3 toAttacker = (attacker.transform.position - my.position).normalized;
        my.rotation = Quaternion.LookRotation(toAttacker);
    }

    private void PlayHitEffect(Volt_Robot robot, AttackInfo attackInfo, Vector3 pos)
    {
        if (attackInfo == null)
            return;

        if (attackInfo.effectType != Define.Effects.None)
            Volt_ParticleManager.Instance.PlayParticle(
                Volt_PrefabFactory.S.PopEffect(attackInfo.effectType), pos);

        if (attackInfo.Damage > 0)
        {
            if (robot == Volt_PlayerManager.S.I.GetRobot())
                Volt_PlayerManager.S.I.playerCamRoot.camEffect.SetVignette(Color.red);

            Volt_PlayerManager.S.I.playerCamRoot.camEffect.SetShakeType(attackInfo.CameraShakeType);
            Volt_PlayerManager.S.I.playerCamRoot.CameraShake();
        }
    }

    public override void OnEnterState(StateMachine fsm)
    {
        fsm.isDamaged = false;
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);

        Volt_GamePlayData.S.RenewOtherRobotsAttackedByRobotsOnThatTurn(fsm.attackInfo.AttackerNumber, fsm.Owner.playerInfo.playerNumber, fsm.Owner.HitCount);

        PlayHitEffect(fsm.Owner, fsm.attackInfo, fsm.Owner.GetCenterPosition());


        if (fsm.attackInfo != null)
        {
            fsm.Owner.lastAttackPlayer = fsm.attackInfo.AttackerNumber;
        }
        //if (fsm.Owner.lastAttackPlayer == 0)
        //{
        //    fsm.Owner.lastAttackPlayer = fsm.attackInfo.AttackerNumber;
        //}

        // 노데미지가 들어오면 이전에 재생시킨 애니메이션을 재생한다.
        if (fsm.attackInfo.Damage == 0)
        {
            if (string.IsNullOrEmpty(fsm.prevDamagedAnimation))
                fsm.prevDamagedAnimation = "Damaged";

            fsm.Animator.CrossFade(fsm.prevDamagedAnimation, .1f, -1, 0f);
            return;
        }

        Volt_GMUI.S.Create3DMsg(MSG3DEventType.hpDown, fsm.Owner.playerInfo,
            fsm.attackInfo.Damage);

        int damage = CalculateDamage(fsm.Owner, fsm.attackInfo.Damage);
        string animationName = GetAnimationClipName(damage);

        fsm.Owner.HitCount += damage;
        fsm.Animator.CrossFade(animationName, .1f, -1, 0f);
        fsm.prevDamagedAnimation = animationName;

        if (fsm.attackInfo.AttackerNumber == 0)
            return;

        if (fsm.attackInfo.AttackerNumber == fsm.Owner.playerInfo.playerNumber)
            return;


        //때린놈이 있으면 때린놈 쪽을 본다
        ForwardToAttacker(fsm.transform, Volt_PlayerManager.S.GetPlayerByPlayerNumber(fsm.attackInfo.AttackerNumber).GetRobot());
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.AniEventHandler.isDoneDamaged = false;
        //fsm.attackInfo = null;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {

    }

    
}
