using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/CollideWithWallDecision")]
public class CollideWithWallDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        SphereCollider collider = fsm.GetComponent<SphereCollider>();

        Collider[] colliders = Physics.OverlapSphere(collider.bounds.center,
            .5f, fsm.wallMask);

        if (colliders.Length > 0)
        {
            if (fsm.collidedWall == colliders[0].gameObject)
                return false;
            if (!colliders[0].GetComponent<WallBlockPoint>().IsBlock(fsm.MoveDir))
            {
                Debug.Log("Wall Block is false");
                return false;
            }

            fsm.collidedWall = colliders[0].gameObject;
            Collider[] tileColliders = Physics.OverlapBox(fsm.transform.position,
            Vector3.one * 0.5f, Quaternion.identity, LayerMask.GetMask("Tile"));

            if (tileColliders.Length == 1)
            {
                Volt_Tile nextTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                    fsm.MoveDir);
                fsm.knockbackInfor = new KnockbackInfor(fsm.gameObject,
                    -fsm.MoveDir, nextTile);
            }
            else
            {
                for (int i = 0; i < tileColliders.Length; ++i)
                {
                    Vector3 to = tileColliders[i].transform.position;
                    to.y = fsm.transform.position.y;
                    Vector3 dir = (to - fsm.transform.position).normalized;
                    float angle = Vector3.Angle(fsm.MoveDir, dir);
                    if (angle > 5f)
                        continue;
                    fsm.knockbackInfor = new KnockbackInfor(fsm.gameObject,
                        -fsm.MoveDir, tileColliders[i].GetComponent<Volt_Tile>());
                    break;
                }
            }
            fsm.behavior = null;
            return true;
        }
        fsm.collidedWall = null;
        return false;
    }
}
