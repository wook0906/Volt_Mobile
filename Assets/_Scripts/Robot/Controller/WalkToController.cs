using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToController : MoveController
{
    public WalkToController(StateMachine fsm, Transform robot, float speed, float angular)
        : base(fsm, robot, speed, angular)
    {

    }

    public override void UpdatePrev(float deltaTime)
    {
        if(IsDestinationSet())
        {
            Collider[] colliders = Physics.OverlapBox(fsm.transform.position,
                Vector3.one * 0.5f, Quaternion.identity, LayerMask.GetMask("Tile"));

            //Debug.Assert(colliders.Length > 0, "야 이거 타일 한개도 없댜 이게 우찌된 일이고!!");

            if (colliders.Length == 1)
            {
                fsm.destPos = Volt_ArenaSetter.S.GetTile(fsm.transform.position).transform.position;
                fsm.destPos.y = fsm.transform.position.y;
                //Debug.Log($"collide with only one tile [{fsm.Owner.playerInfo.playerNumber}] destPos:{fsm.destPos}");
            }
            else
            {
                fsm.destPos = Vector3.positiveInfinity;
                for (int i = 0; i < colliders.Length; ++i)
                {
                    Vector3 to = colliders[i].transform.position;
                    to.y = fsm.transform.position.y;
                    Vector3 dir = (to - fsm.transform.position).normalized;
                    if (Vector3.Angle(dir, fsm.MoveDir) > 15f)
                        continue;

                    fsm.destPos = colliders[i].transform.position;
                }

                //Debug.Assert(fsm.destPos.magnitude != float.PositiveInfinity, "야 이거 목적지 잘못됐다! 확인바람!");
                fsm.destPos.y = fsm.transform.position.y;
                //Debug.Log($"Collide with {colliders.Length} tiles [{fsm.Owner.playerInfo.playerNumber}] destPos:{fsm.destPos}");
            }
        }
        moveTime = (fsm.destPos - robot.position).magnitude / speed;
        //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] moveTime:{moveTime} direction:{fsm.destPos - robot.position}");
        fsm.MoveDir = (fsm.destPos - robot.position).normalized;
        state = State.Rotation;
        //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] to rotation");
    }

    public override void UpdateFinish()
    {
        fsm.knockbackInfor = null;
        fsm.isKnockback = false;
        fsm.isDoneWalkTo = true;
    }
}
