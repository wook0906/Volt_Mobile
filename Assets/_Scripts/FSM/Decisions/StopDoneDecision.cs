using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/StopDecision")]
public class StopDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.AniEventHandler.isDoneStop;
    }
}
