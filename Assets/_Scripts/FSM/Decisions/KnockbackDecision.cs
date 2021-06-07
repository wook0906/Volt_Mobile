using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/KnockbackDecision")]
public class KnockbackDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        if (fsm.knockbackInfor != null)
        {
            return true;
        }
        else
            return false;
    }
}
