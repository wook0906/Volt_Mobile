using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/InActivityState")]
public class InActivityState : StateBase
{
    public override void OnEnterState(StateMachine fsm)
    {
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        Vector3 pos = fsm.Owner.GetCenterPosition();
        Volt_ParticleManager.Instance.PlayParticle(
            Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_EMPHit), pos);
        pos.y += 1.25f;
        Volt_ParticleManager.Instance.PlayParticle(
            Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_EMPStun), pos);
        fsm.Owner.SoundPlay("stun");
        fsm.Animator.CrossFade("Inactivity", .1f);
        fsm.Owner.moduleCardExcutor.DestroyCardAll();
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.AniEventHandler.isDoneInActivity = false;
        fsm.isInActivity = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {

    }
}
