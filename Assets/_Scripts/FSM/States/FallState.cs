using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/FallState")]
public class FallState : StateBase
{
    public float fallTime = 0.7f;

    private Vector3 startScale;
    private Vector3 endScale = new Vector3(0.02f, 0.02f, 0.02f);

    public override void OnEnterState(StateMachine fsm)
    {
        Volt_PlayerManager.S.I.playerCamRoot.CameraMoveStart(fsm.gameObject);
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);
        fsm.GetComponent<Collider>().enabled = false;

        if (Volt_GameManager.S.mapType == MapType.Rome)
        {
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/roma_FalingWater.wav",
                (result)=>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false, 0.1f);
                });
            
            
            Volt_ParticleManager.Instance.DelayedPlayParticleOwnPosition(Define.Effects.VFX_FallInWater, fsm.transform, 0.1f);
        }
        
        if (fsm.Owner.lastAttackPlayer == 0)
        {
            if(RobotBehaviourObserver.Instance.currentPusher != null)
                fsm.Owner.lastAttackPlayer = RobotBehaviourObserver.Instance.currentPusher.playerInfo.playerNumber;
        }
        fsm.Animator.CrossFade("Fall", .1f);
        fsm.isDead = true;

        startScale = fsm.transform.localScale;
        fsm.elapsedTime = 0f;
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.isDoneFall = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
        fsm.elapsedTime += deltaTime;
        if(fsm.elapsedTime < fallTime)
        {
            fsm.transform.position += Vector3.down * fsm.fallSpeed * deltaTime;
            float u = fsm.elapsedTime / fallTime;
            Vector3 newScale = Vector3.Lerp(startScale, endScale, u);
            fsm.transform.localScale = newScale;
        }
        else
        {
            fsm.isDoneFall = true;
        }
    }
}
