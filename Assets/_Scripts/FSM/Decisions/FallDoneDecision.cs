using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "StateMachine/Decisions/FallDoneDecision")]
public class FallDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.isDoneFall;
    }
}
