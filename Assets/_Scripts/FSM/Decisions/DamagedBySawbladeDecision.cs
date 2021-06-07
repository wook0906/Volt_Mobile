using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/DamagedBySawblade")]
public class DamagedBySawblade : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return false;
        //if (!fsm.collisionData.isHaveSawblade)
        //    return false;

        //Volt_Robot opponent = fsm.collisionData.robot.GetComponent<Volt_Robot>();
        //int damage = fsm.collisionData.behaviorPoints < 4 ? 1 : 2;
        //fsm.attackInfo = new AttackInfo(opponent.playerInfo.playerNumber,
        //    damage, CameraShakeType.SawBlade);
        //return true;
    }
}
