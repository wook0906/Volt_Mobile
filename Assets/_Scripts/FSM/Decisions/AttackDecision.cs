using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/AttackDecision")]
public class AttackDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        if (fsm.behavior != null &&
            fsm.behavior.BehaviourType == BehaviourType.Attack)
        {
            return true;
        }
        else
            return false;
    }
}
