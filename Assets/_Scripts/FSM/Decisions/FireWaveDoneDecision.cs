using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/FireWaveDoneDecision")]
public class FireWaveDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.AniEventHandler.isDoneFireWave;
    }
}
