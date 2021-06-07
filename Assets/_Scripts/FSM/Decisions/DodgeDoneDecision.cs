using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/DodgeDoneDecision")]
public class DodgeDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.AniEventHandler.isDoneDodge;     
    }
}
