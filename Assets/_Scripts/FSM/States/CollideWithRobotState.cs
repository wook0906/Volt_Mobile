using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/CollideWithRobotState")]
public class CollideWithRobotState : StateBase
{
    private void PlaySawbladeHitEffect(Volt_Robot robot)
    {
        Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_SawbladeHit),
            robot.GetCenterPosition() + robot.transform.forward);
        Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_SawbladeMark),
            true, robot.transform);

        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_SAWBLADE.mp3",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
    }

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

    public override void OnEnterState(StateMachine fsm)
    {
        System.Type prevState = fsm.prevState.GetType();
        fsm.AniEventHandler.isDoneCollideWithRobot = false;
        //Debug.Log($"[{fsm.Owner.playerInfo.playerNumber} player] PushType:{fsm.Owner.PushType}, prevState:{fsm.prevState}, isMoving:{fsm.isMoving}");
        if (fsm.isMoving)
        {
            //Debug.Log($"[{fsm.Owner.playerInfo.playerNumber} player] play collideWithRobot animation");
            fsm.Animator.CrossFade("CollideWithRobot", .1f, -1, 0f);
        }
        else
        {
            //Debug.Log($"[{fsm.Owner.playerInfo.playerNumber} player] don't play collideWithRobot animation");
            fsm.AniEventHandler.isDoneCollideWithRobot = true;
        }

        if (!fsm.collisionData.isHaveSawblade)
            return;
       
        fsm.collisionData.robot.GetComponent<Volt_ModuleCardExcutor>().GetModuleCardByCardType(Card.SAWBLADE).OnUseCard();

        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseSawBlade, fsm.collisionData.robot.GetComponent<Volt_Robot>().playerInfo.playerNumber);
        fsm.attackInfo = new AttackInfo(fsm.collisionData.robot.GetComponent<Volt_Robot>().playerInfo.playerNumber, 0, CameraShakeType.SawBlade, Card.SAWBLADE);
        
        int damage = fsm.collisionData.behaviorPoints < 4 ? 1 : 2;
        damage = CalculateDamage(fsm.Owner, damage);
        fsm.Owner.HitCount += damage;
        string aniName = GetAnimationClipName(damage);

        fsm.Owner.lastAttackPlayer = fsm.collidedRobot.GetComponent<Volt_Robot>().playerInfo.playerNumber;
        PlaySawbladeHitEffect(fsm.Owner);

        fsm.Animator.CrossFade(aniName, .1f);
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.AniEventHandler.isDoneCollideWithRobot = false;
        fsm.isDonePushDecision = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {

    }
}
