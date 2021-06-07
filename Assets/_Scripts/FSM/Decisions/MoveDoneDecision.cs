using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/MoveDoneDeicision")]
public class MoveDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.isDoneMove;
    }
}
