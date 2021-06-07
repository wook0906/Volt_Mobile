using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerbeamAttack : MeleeAttack
{
    private int CalculateDamage(Volt_Robot robot, int damage)
    {
        int result = damage;

        if (robot.AddOnsMgr.ShieldPoints == 0)
            return result;

        robot.ShieldEffectPlay();

        result = damage - robot.AddOnsMgr.ShieldPoints;
        robot.AddOnsMgr.ShieldPoints -= damage;

        if (Volt_PlayerManager.S.I.playerNumber == robot.playerInfo.playerNumber)
            PacketTransmission.SendAchievementProgressPacket(2000028, robot.playerInfo.playerNumber, true);

        if (robot.AddOnsMgr.ShieldPoints <= 0)
        {
            robot.moduleCardExcutor.DestroyCard(Card.SHIELD);
            robot.AddOnsMgr.ShieldPoints = 0;
        }

        return Mathf.Max(0, result);
    }


    protected override void HitTarget(GameObject[] hits, AttackInfo attackInfo)
    {
        GameObject powerbeamModule = Volt_PrefabFactory.S.PopObject(Define.Objects.POWERBEAM);
        if (powerbeamModule == null)
            return;

        powerbeamModule.GetComponent<SciFiArsenal.SciFiBeamScript>().onEnd = () =>
        {
            Volt_PrefabFactory.S.PushObject(powerbeamModule.GetComponent<Poolable>());
        };
        //powerbeamModule.GetComponent<SciFiArsenal.SciFiBeamScript>().BeamStart(targetTile.transform, fsm.Owner.GetCenterPosition() + (fsm.transform.forward.normalized));
        Vector3 fromBeam = fsm.Owner.GetCenterPosition() + (fsm.transform.forward.normalized);

        for (int i = 0; i < hits.Length; ++i)
        {
            if (IsAttackBlocking(hits[i]))
            {
                powerbeamModule.GetComponent<SciFiArsenal.SciFiBeamScript>().BeamStart(hits[i].transform,
                    fromBeam);
                return;
            }

            Volt_Robot robot = hits[i].GetComponent<Volt_Robot>();
            //Debug.Assert(robot != null, $"robot이 null일 수 없는데 null이다. 뭘 맞췄는지 보자 name:{hits[i].name}");
            if (robot.fsm.isDead)
                continue;

            if (robot.AddOnsMgr.IsDodgeOn)
            {
                robot.fsm.isDodgeAttack = true;
            }
            else
            {
                PacketTransmission.SendAttackSuccessPacket(fsm.Owner.playerInfo.playerNumber, Volt_GMUI.S.RoundNumber);

                int range = fsm.behavior.BehaviorPoints <= 3 ? 1 : 2;
                int damage = range;
                damage = CalculateDamage(robot, damage);
                robot.HitCount += damage;

                robot.fsm.attackInfo = new AttackInfo(fsm.Owner.playerInfo.playerNumber, 0, CameraShakeType.PowerBeam, Card.POWERBEAM);
                
                if(fsm.Owner.HitCount >= 3)
                    return;

                robot.fsm.knockbackInfor = new KnockbackInfor(fsm.gameObject,
                    fsm.behavior.Direction, null, range,
                    attackInfo.CameraShakeType);
                robot.fsm.isKnockback = true;

                powerbeamModule.GetComponent<SciFiArsenal.SciFiBeamScript>().BeamStart(robot.transform,
                    fromBeam);
                return;
            }
        }

        Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position, fsm.behavior.Direction, fsm.behavior.BehaviorPoints);
        powerbeamModule.GetComponent<SciFiArsenal.SciFiBeamScript>().BeamStart(targetTile.transform, fsm.Owner.GetCenterPosition() + (fsm.transform.forward.normalized));
    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();

        AttackInfo attackInfo = CreateAttackInfo(attackData);

        //        Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position,
        //          fsm.behavior.Direction, fsm.behavior.BehaviorPoints);

        BlinkTiles(Volt_ArenaSetter.S.GetTiles(fsm.transform.position, fsm.behavior.Direction, fsm.behavior.BehaviorPoints));
        PlayMuzzleEffect(fsm.transform, attackData);

        GameObject[] hits = fsm.Owner.GetHitRobotsInNearestOrder(fsm.behavior);
        HitTarget(hits, attackInfo);
    }
}