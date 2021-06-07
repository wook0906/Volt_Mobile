using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "StateMachine/Decisions/DodgeDecision")]
public class DodgeDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.isDodgeAttack;
    }
}
