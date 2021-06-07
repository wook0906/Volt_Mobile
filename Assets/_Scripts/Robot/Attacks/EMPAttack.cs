using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPAttack : MeleeAttack
{
    protected override void BlinkTiles(Volt_Tile[] tiles)
    {
        foreach (Volt_Tile tile in tiles)
        {
            tile.SetBlinkOption(BlinkType.Tactic, 0.5f);
            tile.BlinkOn = true;
        }
    }

    protected override void HitTarget(GameObject[] hits, AttackInfo attackInfo)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (IsAttackBlocking(hits[i]))
                break;

            Volt_Robot robot = hits[i].GetComponent<Volt_Robot>();
            //Debug.Assert(robot != null, $"robot이 null일 수 없는데 null이다. 뭘 맞췄는지 보자 name:{hits[i].name}");
            if (robot.fsm.isDead)
                continue;

            
            //if (robot.AddOnsMgr.IsDodgeOn)
            //{
            //    //robot.fsm.isDodgeAttack = true;
            //    robot.fsm.isInActivity = true;
            //}
            //else
            //{
                PacketTransmission.SendAttackSuccessPacket(fsm.Owner.playerInfo.playerNumber, Volt_GMUI.S.RoundNumber);
                robot.fsm.isInActivity = true;
                break;
            //}
        }
    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseEMP, fsm.Owner.playerInfo.playerNumber);
        BlinkTiles(Volt_ArenaSetter.S.GetTiles(fsm.transform.position,
            fsm.behavior.Direction, fsm.behavior.BehaviorPoints));
        PlayMuzzleEffect(fsm.transform, attackData, fsm.behavior);



        GameObject[] hits = fsm.Owner.GetHitRobotsInNearestOrder(fsm.behavior);
        HitTarget(hits, null);
    }
}
