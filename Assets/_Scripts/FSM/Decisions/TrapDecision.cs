using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/TrapDecision")]
public class TrapDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        Collider[] colliders = Physics.OverlapSphere(fsm.transform.position, .2f, fsm.trapMask);

        if (colliders.Length > 0)
        {
            if (fsm.collidedTrap == colliders[0].gameObject)
                return false;

            Vector3 trapPos = colliders[0].gameObject.transform.position;
            Vector3 dir = (trapPos - fsm.transform.position).normalized;
            if (Volt_Utils.IsBackLeft(dir) || Volt_Utils.IsBackRight(dir) ||
                Volt_Utils.IsForwardLeft(dir) || Volt_Utils.IsForwardRight(dir))
                return false;

            fsm.collidedTrap = colliders[0].gameObject;
            fsm.attackInfo = new AttackInfo(fsm.Owner.playerInfo.playerNumber,
                1, CameraShakeType.None, Define.Effects.VFX_ElectricTrapHit);
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/Module_Sound/ElectricTrapShock.wav",
                (result)=>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                });
            
            if(fsm.knockbackInfor != null)
            {
                fsm.knockbackInfor = null;
                fsm.destPos = Vector3.positiveInfinity;
            }
            return true;
        }
        //fsm.collidedTrap = null;
        return false;
    }
}
