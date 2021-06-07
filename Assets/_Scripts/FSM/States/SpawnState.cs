using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/SpawnState")]
public class SpawnState : StateBase
{
    public override void OnEnterState(StateMachine fsm)
    {
        fsm.isSpawn = false;
        fsm.Animator.CrossFade("Spawn", .1f);
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.AniEventHandler.isDoneSpawn = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {

    }
}
