using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : BaseAttack
{
    protected bool IsAttackBlocking(GameObject hitGO)
    {
        return hitGO.CompareTag("Wall");
    }

    protected void HitTargetInNoDamage(AttackInfo attackInfo)
    {
        foreach (var robot in fsm.lastAttackRobots)
        {
            if (robot.GetComponent<Volt_Robot>().fsm.isDead)
                continue;
            robot.SendMessage("GetDamage", attackInfo, SendMessageOptions.DontRequireReceiver);
        }
    }

    protected virtual void HitTarget(GameObject[] hits, AttackInfo attackInfo)
    {
        for (int i = 0; i < hits.Length; ++i)
        {
            if (IsAttackBlocking(hits[i].gameObject))
                break;
                
            Volt_Robot robot = hits[i].GetComponent<Volt_Robot>();
            //Debug.Assert(robot != null, $"robot이 null일 수 없는데 null이다. 뭘 맞췄는지 보자 name:{hits[i].name}");
           

            if (robot.fsm.isDead)
                continue;

            if (robot.AddOnsMgr.IsDodgeOn)
            {
                robot.fsm.attackInfo = attackInfo;
                robot.fsm.isDodgeAttack = true;
                continue;
            }

            PacketTransmission.SendAttackSuccessPacket(fsm.Owner.playerInfo.playerNumber,
                Volt_GMUI.S.RoundNumber);

            robot.SendMessage("GetDamage", attackInfo, SendMessageOptions.DontRequireReceiver);
            fsm.lastAttackRobots.Add(hits[i]);
            break;
        }
    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        AttackInfo attackInfo = CreateAttackInfo(attackData);

        PlayMuzzleEffect(fsm.transform, attackData);
        BlinkTiles(Volt_ArenaSetter.S.GetTiles(fsm.transform.position, fsm.behavior.Direction,
            fsm.behavior.BehaviorPoints));

        if (attackData.damage == 0)
        {
            HitTargetInNoDamage(attackInfo);
        }
        else
        {
            GameObject[] hits = fsm.Owner.GetHitRobotsInNearestOrder(fsm.behavior);
            HitTarget(hits, attackInfo);
        }
    }
}
