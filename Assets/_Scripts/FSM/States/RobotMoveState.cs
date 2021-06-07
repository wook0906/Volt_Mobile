using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/RobotMoveState")]
public class RobotMoveState : StateBase
{
    public override void OnEnterState(StateMachine fsm)
    {
        Debug.Assert(fsm.behavior != null, "Error!! 행동 정보가 null입니다.");

        fsm.isMoving = true;
        fsm.isDoneMove = false;
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        fsm.Animator.CrossFade("Movement", .01f);

        BlinkTileInAttackType(fsm);
        
        if(fsm.Owner.playerInfo.PlayerType == PlayerType.AI)
        {
            fsm.moveController = new KillbotMoveController(fsm, fsm.transform, fsm.moveSpeed, fsm.angular);
            //fsm.StartCoroutine(CoKillbotMove(fsm));
        }
        else
        {
            fsm.moveController = new MoveController(fsm, fsm.transform, fsm.moveSpeed, fsm.angular);
        }

        if (CheckUseSteeringNozzleModule(fsm.Owner.moduleCardExcutor, fsm.behavior.Direction))
        {
            Volt_Module_SteeringNozzle module = fsm.Owner.moduleCardExcutor.GetModuleCardByCardType(Card.STEERINGNOZZLE) as Volt_Module_SteeringNozzle;
            module.OnUseCard();
            Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseSteeringNozzle, fsm.Owner.playerInfo.playerNumber);
            fsm.Owner.moduleCardExcutor.DestroyCard(Card.STEERINGNOZZLE);
        }
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.isDoneMove = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
        fsm.moveController.Update(deltaTime);
        
    }

    private void BlinkTileInAttackType(StateMachine fsm)
    {
        if (fsm.Owner.AddOnsMgr.IsHaveSawBlade)
        {
            foreach (var item in Volt_ArenaSetter.S.GetTiles(fsm.Owner.transform.position, fsm.behavior.Direction, fsm.behavior.BehaviorPoints))
            {
                item.SetBlinkOption(BlinkType.Attack, 0.5f);
                item.BlinkOn = true;
            }
        }
    }

    private bool CheckUseSteeringNozzleModule(Volt_ModuleCardExcutor moduleCardExcutor, Vector3 moveDir)
    {
        return moduleCardExcutor.IsHaveModuleCard(Card.STEERINGNOZZLE) && IsDiagonalDirection(moveDir);
    }

    private bool IsDiagonalDirection(Vector3 direction)
    {
        if (Vector3.Angle(direction, Vector3.forward) < 5f)
            return false;
        else if (Vector3.Angle(direction, Vector3.back) < 5f)
            return false;
        else if (Vector3.Angle(direction, Vector3.right) < 5f)
            return false;
        else if (Vector3.Angle(direction, Vector3.left) < 5f)
            return false;
        else
            return true;
        //if (Vector3.Angle(direction, Vector3.forward + Vector3.left) < 5f)
        //    return true;
        //else if (Vector3.Angle(direction, Vector3.forward + Vector3.right) < 5f)
        //    return true;
        //else if (Vector3.Angle(direction, Vector3.back + Vector3.left) < 5f)
        //    return true;
        //else if (Vector3.Angle(direction, Vector3.back + Vector3.right) < 5f)
        //    return true;
        //else
        //    return false;
    }
}
