using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/HaveWaveModuleDecision")]
public class HaveWaveModuleDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        if (fsm.Owner.PushType == PushType.Pusher)
        {
            if (fsm.Owner.moduleCardExcutor.IsHaveModuleCard(Card.SHOCKWAVE) ||
                fsm.Owner.moduleCardExcutor.IsHaveModuleCard(Card.REPULSIONBLAST))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
}
