using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/CollideWithRobotDoneDecision")]
public class CollideWithRobotDoneDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        return fsm.AniEventHandler.isDoneCollideWithRobot;
    }
}
