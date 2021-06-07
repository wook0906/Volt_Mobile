using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/KnockbackDoneDecision")]
public class KnockbackDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.isDoneKnockback;
    }
}
