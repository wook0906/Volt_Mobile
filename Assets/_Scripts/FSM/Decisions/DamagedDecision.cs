using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/DamagedDecision")]
public class DamagedDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.isDamaged;
    }
}
