using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/StopState")]
public class StopState : StateBase
{
    public float slidingDist;
    public float slidingTime;
    
    public override void OnEnterState(StateMachine fsm)
    {
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);

        fsm.Animator.CrossFade("Stop", .1f);
        //fsm.Animator.Play("Stop", -1, 0);

        if(fsm.prevState.GetType() == typeof(RobotMoveState) ||
            fsm.prevState.GetType() == typeof(KillbotMoveState))
        {
            fsm.MoveDir = fsm.transform.forward;
        }
        else if(fsm.prevState.GetType() == typeof(KnockbackState))
        {
            fsm.MoveDir = -fsm.transform.forward;
        }
        else
        {
            //Debug.LogError($"Error!! Stop 상태의 직전 상태가 잘못 되었습니다. prevState:{fsm.prevState.GetType()}");
        }

        fsm.startPos = fsm.transform.position;
        fsm.destPos = fsm.startPos + fsm.MoveDir * slidingDist;
        fsm.elapsedTime = 0f;
        fsm.u = 0f;
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.AniEventHandler.isDoneStop = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
        fsm.elapsedTime += deltaTime;
        fsm.u = fsm.elapsedTime / slidingTime;

        if(fsm.u > 0f && fsm.u < 0.5f)
        {
            Vector3 pos = Vector3.Lerp(fsm.startPos, fsm.destPos, fsm.u * 2f);
            fsm.transform.position = pos;
        }
        else if(fsm.u >= 0.5f && fsm.u < 1f)
        {
            Vector3 pos = Vector3.Lerp(fsm.destPos, fsm.startPos, (fsm.u - 0.5f) * 2f);
            fsm.transform.position = pos;
        }
        else if(fsm.u >= 1f)
        {
            fsm.transform.position = fsm.startPos;
            fsm.destPos = Vector3.positiveInfinity;
        }
    }
}
