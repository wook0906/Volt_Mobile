using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PernerateAttack : MeleeAttack
{
    protected override void HitTarget(GameObject[] hits, AttackInfo attackInfo)
    {
        for (int i = 0; i < hits.Length; ++i)
        {

            if (IsAttackBlocking(hits[i]))
                //break;
                continue;

            Volt_Robot robot = hits[i].GetComponent<Volt_Robot>();
            //Debug.Assert(robot != null, $"robot이 null일 수 없는데 null이다. 뭘 맞췄는지 보자 name:{hits[i].name}");
            if (robot.fsm.isDead)
                continue;

            if (robot.AddOnsMgr.IsDodgeOn)
            {
                robot.fsm.attackInfo = attackInfo;
                robot.fsm.isDodgeAttack = true;
            }
            else
            {
                PacketTransmission.SendAttackSuccessPacket(fsm.Owner.playerInfo.playerNumber, Volt_GMUI.S.RoundNumber);
                robot.SendMessage("GetDamage", attackInfo, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/SFx_200703/PernerateHit.wav",
            (result)=>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        base.AttackHandler(fsm, attackData);
    }
}
