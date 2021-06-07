using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFireAttack : MeleeAttack
{
    private Vector3[] PlusCross = new Vector3[4]
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left
        };
    private Vector3[] XCross = new Vector3[4]
        {
            (Vector3.forward + Vector3.right).normalized,
            (Vector3.right + Vector3.back).normalized,
            (Vector3.left + Vector3.back).normalized,
            (Vector3.forward + Vector3.left).normalized
        };


    private bool IsAttackDirDiagonal(Vector3 face)
    {
        if (Volt_Utils.IsForward(face) || Volt_Utils.IsBackward(face) ||
            Volt_Utils.IsLeft(face) || Volt_Utils.IsRight(face))
            return false;
        return true;
    }

    private Vector3[] GetAttackDirections(Vector3 forward)
    {
        if (IsAttackDirDiagonal(fsm.behavior.Direction))
        {
            return XCross;
        }
        else
        {
            return PlusCross;
        }
    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();
        Vector3[] attackDirs = GetAttackDirections(fsm.behavior.Direction);
        AttackInfo attackInfo = CreateAttackInfo(attackData);
        PlayMuzzleEffect(fsm.transform, attackData);

        // 4방향 BlinkTiles
        foreach (Vector3 dir in attackDirs)
        {
            BlinkTiles(Volt_ArenaSetter.S.GetTiles(fsm.transform.position,
                dir, fsm.behavior.BehaviorPoints));
        }

        if (attackData.damage == 0)
        {
            HitTargetInNoDamage(attackInfo);
        }
        else
        {
            foreach (Vector3 dir in attackDirs)
            {
                GameObject[] hits = fsm.Owner.GetHitRobotsInNearestOrder(dir, fsm.behavior.BehaviorPoints);
                HitTarget(hits, attackInfo);
            }
        }
    }
}
