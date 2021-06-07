using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/WalkToState")]
public class WalkToState : StateBase
{
    public override void OnEnterState(StateMachine fsm)
    {
        //Debug.Assert(fsm.knockbackInfor != null, "야 넉백 정보 널이다. 뭔가 잘못됐다 확인해봐!");
        fsm.isMoving = true;
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);

        fsm.Animator.CrossFade("Movement", .1f);

        fsm.moveController = new WalkToController(fsm, fsm.transform, fsm.moveSpeed * 0.5f, fsm.angular);
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.isDoneWalkTo = false;

    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
        //Debug.Log($"[{fsm.Owner.playerInfo.playerNumber}] Walk To Action");
        fsm.moveController.Update(deltaTime);
    }
}
