using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/WalkToDoneDecision")]
public class WalkToDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.isDoneWalkTo;
    }
}
