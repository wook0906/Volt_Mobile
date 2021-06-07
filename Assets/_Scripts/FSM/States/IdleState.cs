using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/IdleState")]
public class IdleState : StateBase
{
    public override void OnEnterState(StateMachine fsm)
    {
        RobotBehaviourObserver.Instance.OffBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        fsm.Animator.CrossFade("Idle", .1f);
        fsm.remainState = fsm.currentState;

        // 이동관련 변수들 초기화
        fsm.startPos = Vector3.positiveInfinity;
        fsm.MoveDir = Vector3.positiveInfinity;
        fsm.destPos = Vector3.positiveInfinity;
        fsm.elapsedTime = 0f;
        fsm.u = 0f;
        fsm.moveTime = 0f;
        fsm.rotationTime = 0f;
        fsm.startTile = null;
        fsm.qTiles.Clear();
        fsm.collidedTrap = null;
        fsm.collidedWall = null;
        fsm.isDetectSomething = false;
        fsm.isDoneMove = true;
        fsm.behavior = null;
        fsm.isMoving = false;


        fsm.anlge = 0f;
        fsm.fromRot = Quaternion.identity;
        fsm.toRot = Quaternion.identity;

        
        fsm.isPlayAttackAnimation = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {

    }
}
