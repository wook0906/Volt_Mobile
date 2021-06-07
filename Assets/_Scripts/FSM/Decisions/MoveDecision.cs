using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/MoveDecision")]
public class MoveDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
       
            if (fsm.behavior != null &&
                fsm.behavior.BehaviourType == BehaviourType.Move)
            {
                return true;
            }
            else
                return false;
    }
}
