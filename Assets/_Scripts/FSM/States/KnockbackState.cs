using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/KnockbackState")]
public class KnockbackState : StateBase
{
    private void ChangePushType(StateMachine fsm)
    {
        switch (fsm.Owner.PushType)
        {
            case PushType.PushedCandidate:
                fsm.Owner.PushType = PushType.Pushed;
                break;
            case PushType.Pushed:
                fsm.Owner.PushType = PushType.Immune;
                break;
            default:
                break;
        }
    }

    public override void OnEnterState(StateMachine fsm)
    {
        //Debug.Log($"{fsm.Owner.playerInfo.playerNumber} is enter knockback state");
        //Debug.Assert(fsm.knockbackInfor != null, "Knocback infor가 왜 널이야!! 왜 널이냐고!!");
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        fsm.isMoving = true;
        fsm.isKnockback = false;

        if(fsm.Owner.lastAttackPlayer == 0 && Volt_GameManager.S.pCurPhase != Phase.suddenDeath)
            fsm.Owner.lastAttackPlayer = RobotBehaviourObserver.Instance.currentPusher.playerInfo.playerNumber;

        fsm.Owner.SoundPlay("guard");
        fsm.Animator.CrossFade("Knockback", .1f);
        
        Volt_ParticleManager.Instance.PlayParticle(
            Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_RobotCollision),
            fsm.Owner.GetCenterPosition());

        ChangePushType(fsm);

        fsm.startPos = fsm.transform.position;
        if (fsm.knockbackInfor.startTile == null)
        {
            fsm.destPos = Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                fsm.knockbackInfor.direction, fsm.knockbackInfor.range).transform.position;
        }
        else
        {
            fsm.destPos = Volt_ArenaSetter.S.GetTile(fsm.knockbackInfor.startTile.transform.position,
                fsm.knockbackInfor.direction).transform.position;
        }
        fsm.destPos.y = fsm.transform.position.y;
        fsm.MoveDir = fsm.knockbackInfor.direction;
        fsm.moveTime = (fsm.destPos - fsm.startPos).magnitude / fsm.knockbackSpeed;
        fsm.elapsedTime = 0f;
        fsm.u = 0f;

        if (fsm.collisionData == null)   
            return;
        
        // Pusher가 나라면(벽이랑 부딪힘), 내가 보는 방향을 유지한다.
        if (fsm.transform.gameObject == fsm.knockbackInfor.pusher)
            return;

        // pusher가 다른 로봇이라면, 그 로봇을 바라본다. 
        Vector3 toPusher = (fsm.knockbackInfor.pusher.transform.position - fsm.transform.position).normalized;
        fsm.transform.rotation = Quaternion.LookRotation(toPusher);
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.isDoneKnockback = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
        if(fsm.u > 0f && fsm.u < 1f)
        {
            Vector3 pos = Vector3.Lerp(fsm.startPos, fsm.destPos, fsm.u);
            fsm.transform.position = pos;
        }
        else if(fsm.u >= 1f)
        {
            fsm.transform.position = fsm.destPos;
            fsm.isDoneKnockback = true;
            fsm.knockbackInfor = null;
            fsm.destPos = Vector3.positiveInfinity;
            fsm.isMoving = false;
        }
        fsm.elapsedTime += deltaTime;
        fsm.u = fsm.elapsedTime / fsm.moveTime;
    }
}
