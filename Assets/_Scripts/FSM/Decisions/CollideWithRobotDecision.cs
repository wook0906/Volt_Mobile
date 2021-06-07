using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/CollideWithOtherRobotDecision")]
public class CollideWithRobotDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;
        foreach (Volt_Robot robot in robots)
        {
            if (robot == fsm.Owner)
                continue;

            if (robot.fsm.isDead)
                continue;

            float distance = (fsm.transform.position - robot.transform.position).magnitude;
            if(distance <= 1f)
            {
                if (fsm.collidedRobot == robot.gameObject)
                    return false;

                fsm.collisionData = robot.GetCollisionData();
                fsm.collidedRobot = robot.gameObject;
                //Debug.Log($"{fsm.Owner.playerInfo.playerNumber} collided with {robot.playerInfo.playerNumber}");
                return true;
            }
        }
        fsm.collidedRobot = null;
        return false;
        //SphereCollider collider = fsm.GetComponent<SphereCollider>();

        //Collider[] colliders = Physics.OverlapSphere(collider.bounds.center,
        //    .5f, fsm.robotMask);
        
        //for(int i = 0; i < colliders.Length; ++i)
        //{
        //    if (colliders[i].gameObject == fsm.gameObject)
        //        continue;

        //    if (fsm.collidedRobot == colliders[i].gameObject)
        //        return false;

        //    fsm.collisionData = colliders[i].GetComponent<Volt_Robot>().GetCollisionData();
        //    fsm.collidedRobot = colliders[i].gameObject;
        //    return true;
        //}
        //fsm.collidedRobot = null;
        //return false;
    }
}
