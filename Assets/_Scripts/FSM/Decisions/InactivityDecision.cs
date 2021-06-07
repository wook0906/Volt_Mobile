using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/InactivityDecision")]
public class InactivityDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.isInActivity;
    }
}
