using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/InactivityDoneDecision")]
public class InactivityDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.AniEventHandler.isDoneInActivity;
    }
}
