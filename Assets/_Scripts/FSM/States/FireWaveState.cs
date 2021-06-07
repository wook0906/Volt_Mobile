using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/FireWaveState")]
public class FireWaveState : StateBase
{
    public override void OnEnterState(StateMachine fsm)
    {
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        fsm.StartCoroutine(CoAction(fsm));
        //fsm.Animator.CrossFade("FireWave", .1f);
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.AniEventHandler.isDoneFireWave = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {

    }

    private bool IsStopAllRobots(int myPlayerNumber)
    {
        bool isOff = true;
        for (int i = 1; i <= 4; ++i)
        {
            if (i == myPlayerNumber)
                continue;

            isOff = RobotBehaviourObserver.Instance.IsRobotBehaviourFlagOff(i);
            if (!isOff)
                break;
        }
        return isOff;
    }

    private IEnumerator CoAction(StateMachine fsm)
    {
        while (!IsStopAllRobots(fsm.Owner.playerInfo.playerNumber))
        {
            yield return null;
        }

        foreach (var robot in Volt_ArenaSetter.S.robotsInArena)
        {
            if (robot.playerInfo.playerNumber == fsm.Owner.playerInfo.playerNumber)
                continue;
            robot.PushType = PushType.PushedCandidate;
        }
        fsm.Animator.CrossFade("FireWave", .1f);
    }
}
