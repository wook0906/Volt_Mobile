using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/KillbotMoveDecision")]
public class KillbotMoveDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        if (fsm.Owner.playerInfo.PlayerType == PlayerType.AI)
        {
            if (fsm.behavior != null &&
                fsm.behavior.BehaviourType == BehaviourType.Move)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
}
