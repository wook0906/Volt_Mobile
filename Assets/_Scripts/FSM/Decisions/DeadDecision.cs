using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/DeadDecision")]
public class DeadDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.Owner.HitCount >= 3;
    }
}
